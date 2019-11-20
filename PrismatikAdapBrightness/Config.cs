using System;
using System.Collections;
using System.IO;
using System.IO.Ports;
using System.Text.Json;

namespace PrismatikAdapBrightness
{
	internal class Config
	{
		private static string ConfigFileName
		{
			get => Path.Combine(
				Path.GetDirectoryName(typeof(Config).Assembly.Location),
				"config.json");
		}
		public string Port { get => port; set => port = value; }
		public int Bauderate { get => baudeRate; set => baudeRate = value; }

		private string port;
		private int baudeRate;
		
		internal void Save()
		{
			File.WriteAllText(ConfigFileName, JsonSerializer.Serialize<Config>(this));
		}

		internal static Config Load()
		{
			if (File.Exists(ConfigFileName))
			{
				try
				{
					var config = JsonSerializer.Deserialize<Config>((File.ReadAllText(ConfigFileName)));
					if (config.CheckPort())
						return config;
				}
				catch { }
			}
			return null;
		}

		internal bool CheckPort()
		{
			//if (((IList)SerialPort.GetPortNames()).Contains(Port))
			{
				using var port = new SerialPort(Port, Bauderate);
				try
				{
					port.Open();
					port.Close();
					return true;
				}
				catch { }
			}
			return false;
		}
	}
}