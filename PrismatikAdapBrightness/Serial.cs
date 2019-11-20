using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Serial
{
	/// <summary>
	/// Lists ports with their descriptions
	/// </summary>
	internal static class SerialTool
	{
		internal static Dictionary<string, string> ComPorts()
		{
			return ListPorts().Select((e) => new { k = e.Device, v = e.Description }).ToDictionary(x => x.k, x => x.v);
		}

		internal static IEnumerable<ListPortInfo> ListPorts()
		{
			const int DIGCF_PRESENT = 2;
			const int DICS_FLAG_GLOBAL = 1;
			const int DIREG_DEV = 0x00000001;
			const int KEY_READ = 0x20019;
			const int ERROR_INSUFFICIENT_BUFFER = 122;
			const int SPDRP_HARDWAREID = 1;
			const int SPDRP_LOCATION_PATHS = 35;
			const int SPDRP_FRIENDLYNAME = 12;
			const int SPDRP_MFG = 11;

			Guid[] guid = new Guid[8];
			SetupDiClassGuidsFromName("Ports", ref guid[0], 8, out uint reqSize);
			for (int i = 0; i < reqSize; i++)
			{
				int? bInterfaceNumber = null;
				var g_hdi = SetupDiGetClassDevs(ref guid[i], null, IntPtr.Zero, DIGCF_PRESENT);
				var devinfo = new SP_DEVINFO_DATA();

				devinfo.cbSize = (uint)Marshal.SizeOf(devinfo);
				uint index = 0;

				while (SetupDiEnumDeviceInfo(g_hdi, index, ref devinfo))
				{
					index += 1;

					var hkey = SetupDiOpenDevRegKey(
								g_hdi,
								ref devinfo,
								DICS_FLAG_GLOBAL,
								0,
								DIREG_DEV,
								KEY_READ);

					var port_name_buffer = new StringBuilder(250);

					var port_name_length = (uint)port_name_buffer.Capacity; // should be 250;

					RegQueryValueEx(
						hkey,
						"PortName",
						0,
						out uint lpType,
						port_name_buffer,
						ref port_name_length);

					RegCloseKey(hkey);

					//# unfortunately does this method also include parallel ports.
					//# we could check for names starting with COM or just exclude LPT
					//# and hope that other "unknown" names are serial ports...
					if (port_name_buffer.ToString().StartsWith("LPT"))
						continue;

					//# hardware ID
					var szHardwareID = new StringBuilder(250);
					//# try to get ID that includes serial number
					if (!SetupDiGetDeviceInstanceId(
							g_hdi,
							ref devinfo,
							szHardwareID,
							szHardwareID.Capacity - 1,
							out int RequiredSize))
						//# fall back to more generic hardware ID if that would fail
						if (!SetupDiGetDeviceRegistryProperty(
								g_hdi,
								ref devinfo,
								SPDRP_HARDWAREID,
								out uint properyRegDataType,
								szHardwareID,
								(uint)(szHardwareID.Capacity - 1),
								out uint requiredSize))
							//# Ignore ERROR_INSUFFICIENT_BUFFER
							if (GetLastError() != ERROR_INSUFFICIENT_BUFFER)
								throw new Exception("WinError");
					//# stringify
					var szHardwareID_str = szHardwareID.ToString();


					var info = new ListPortInfo(port_name_buffer.ToString(), true);

					//# in case of USB, make a more readable string, similar to that form
					//# that we also generate on other platforms
					if (szHardwareID_str.StartsWith("USB"))
					{

						var m = Regex.Match(szHardwareID_str, @"VID_([0-9a-f]{4})(&PID_([0-9a-f]{4}))?(&MI_(\d{2}))?(\\(\w+))?", RegexOptions.IgnoreCase);

						if (m.Success)
						{
							info.Vid = int.Parse(m.Groups[1].Value, NumberStyles.HexNumber);

							if (m.Groups[3].Value != "")
								info.Pid = int.Parse(m.Groups[3].Value, NumberStyles.HexNumber);

							if (m.Groups[5].Value != "")
								bInterfaceNumber = int.Parse(m.Groups[5].Value);

							if (m.Groups[7].Value != "")
								info.SerialNumber = m.Groups[7].Value;
						}
						//# calculate a location string
						var loc_path_str = new StringBuilder(250);

						if (SetupDiGetDeviceRegistryProperty(
								g_hdi,
								ref devinfo,
								SPDRP_LOCATION_PATHS,
								out uint uint_,
								loc_path_str,
								(uint)(loc_path_str.Capacity - 1),
								out uint req_))
						{

							var m2 = Regex.Matches(loc_path_str.ToString(), @"USBROOT\((\w+)\)|#USB\((\w+)\)");

							var location = new List<string>();

							foreach (Match g in m2)
							{
								if (g.Groups[1].Value != "")
									location.Add((int.Parse(g.Groups[1].Value) + 1).ToString());

								else
								{
									if (location.Count > 1)
										location.Add(".");

									else
										location.Add("-");
								}

								location.Add(g.Groups[2].Value);
							}
							if (bInterfaceNumber.HasValue)
								location.Add(string.Format(":{0}.{1}",
									'x', // # XXX how to determine correct bConfigurationValue?
									bInterfaceNumber));

							if (location.Count > 0)
								info.Location = string.Join("", location);
						}
						info.Hwid = info.usb_info();

					}
					else if (szHardwareID_str.StartsWith("FTDIBUS"))
					{
						var m = Regex.Match(@"VID_([0-9a-f]{4})\+PID_([0-9a-f]{4})(\+(\w+))?", szHardwareID_str, RegexOptions.IgnoreCase);

						if (m.Success)
						{
							info.Vid = int.Parse(m.Groups[1].Value, NumberStyles.HexNumber);

							info.Pid = int.Parse(m.Groups[2].Value, NumberStyles.HexNumber);

							if (m.Groups[4].Value != "")
								info.SerialNumber = m.Groups[4].Value;
						}
						//# USB location is hidden by FDTI driver :(
						info.Hwid = info.usb_info();
					}
					else
					{
						info.Hwid = szHardwareID_str;
					}
					//# friendly name
					var szFriendlyName = new StringBuilder(250);

					if (SetupDiGetDeviceRegistryProperty(
							g_hdi,
							ref devinfo,
							SPDRP_FRIENDLYNAME,
							out uint @uint,
							szFriendlyName,
							(uint)(szFriendlyName.Capacity - 1),
							out uint req))
						info.Description = szFriendlyName.ToString();
					//#~ else:
					//# Ignore ERROR_INSUFFICIENT_BUFFER
					//#~ if ctypes.GetLastError() != ERROR_INSUFFICIENT_BUFFER:
					//	#~ raise IOError("failed to get details for %s (%s)" % (devinfo, szHardwareID.value))
					//# ignore errors and still include the port in the list, friendly name will be same as port name

					//# manufacturer
					var szManufacturer = new StringBuilder(250);

					if (SetupDiGetDeviceRegistryProperty(
							g_hdi,
							ref devinfo,
							SPDRP_MFG,
							out @uint,
							szManufacturer,
							(uint)(szManufacturer.Capacity - 1),
							out req))
						info.Manufacturer = szManufacturer.ToString();

					yield return info;


				}
				SetupDiDestroyDeviceInfoList(g_hdi);

			}
		}

		internal static IEnumerable<string> GetPortNames()
		{
			return (from s in SerialPort.GetPortNames() select s.Split('\0')[0]);
		}

		[StructLayout(LayoutKind.Sequential)]
		struct SP_DEVINFO_DATA
		{
			public UInt32 cbSize;
			public Guid ClassGuid;
			public UInt32 DevInst;
			public IntPtr Reserved;
		}

		[DllImport("kernel32.dll")]
		static extern uint GetLastError();

		[DllImport("setupapi.dll", SetLastError = true)]
		static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

		[DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
		static extern bool SetupDiGetDeviceInstanceId(
			   IntPtr DeviceInfoSet,
			   ref SP_DEVINFO_DATA DeviceInfoData,
			   StringBuilder DeviceInstanceId,
			   int DeviceInstanceIdSize,
			   out int RequiredSize
			);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern bool SetupDiGetDeviceRegistryProperty(
				IntPtr deviceInfoSet,
				ref SP_DEVINFO_DATA deviceInfoData,
				uint property,
				out UInt32 propertyRegDataType,
				byte[] propertyBuffer,
				uint propertyBufferSize,
				out UInt32 requiredSize
				);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern bool SetupDiGetDeviceRegistryProperty(
				IntPtr deviceInfoSet,
				ref SP_DEVINFO_DATA deviceInfoData,
				uint property,
				out UInt32 propertyRegDataType,
				StringBuilder propertyBuffer,
				uint propertyBufferSize,
				out UInt32 requiredSize
				);


		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegQueryValueExW", SetLastError = true)]
		static extern int RegQueryValueEx(
				IntPtr hKey,
				string lpValueName,
				int lpReserved,
				out uint lpType,
				System.Text.StringBuilder lpData,
				ref uint lpcbData);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern int RegCloseKey(
				IntPtr hKey);

		[DllImport("setupapi.dll", SetLastError = true)]
		static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, uint MemberIndex, ref SP_DEVINFO_DATA DeviceInfoData);

		[DllImport("setupapi.dll", SetLastError = true)]
		static extern bool SetupDiClassGuidsFromName(string ClassName, ref Guid ClassGuidArray1stItem, UInt32 ClassGuidArraySize, out UInt32 RequiredSize);

		[DllImport("Setupapi", CharSet = CharSet.Auto, SetLastError = true)]
		static extern IntPtr SetupDiOpenDevRegKey(
					IntPtr hDeviceInfoSet,
					ref SP_DEVINFO_DATA deviceInfoData,
					int scope,
					int hwProfile,
					int parameterRegistryValueKind,
					int samDesired);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto)]
		static extern IntPtr SetupDiGetClassDevs(
											  ref Guid ClassGuid,
											  [MarshalAs(UnmanagedType.LPTStr)] string Enumerator,
											  IntPtr hwndParent,
											  uint Flags
											 );

	}

	internal class ListPortInfo
	{
		public string Device;
		public string Name;
		public string Description;
		public string Hwid;
		public int Vid;
		public int Pid;
		public string SerialNumber;
		public string Location;
		public string Manufacturer;
		public string Product;
		public string Interface;

		public ListPortInfo(string device, bool skip_link_detection)
		{
			Device = device;
			Name = Path.GetFileName(device);
			Description = "n/a";
			Hwid = "n/a";

			//# USB specific data
			Vid = 0;
			Pid = 0;
			SerialNumber = "";
			Location = "";
			Manufacturer = "";
			Product = "";
			Interface = "";
			//# special handling for links
			//if not skip_link_detection and device is not None and os.path.islink(device):
			//	self.hwid = 'LINK={}'.format(os.path.realpath(device))
		}

		internal string usb_info()
		{
			return string.Format("USB VID:PID={0:X4}:{1:X4}{2}{3}",
			Vid,
			Pid,
			SerialNumber == "" ? "" : string.Format(" SER={0}", SerialNumber),
			Location == "" ? "" : string.Format(" LOCATION={0}", Location));
		}

		public override string ToString()
		{
			return $"{Device}, {Description}";
		}
	}
}