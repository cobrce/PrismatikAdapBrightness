#define DIRECT_UPDATE
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace PrismatikAdapBrightness
{
	class BrightnessAdapter
	{
		private readonly Config config;
		private readonly SerialPort port;
		private readonly Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

		internal event EventHandler<int> BrightnessUpdateHandler;

		private int? DesiredBrightness = null;
		private int currentBrightness = 25;

		public BrightnessAdapter(Config portconfig)
		{
			config = portconfig;
			port = new SerialPort(config.Port, config.Bauderate);
			port.DataReceived += Port_DataReceived;

			while (!Connect()) { Thread.Sleep(500); }
			GetBrightness();

			port.Open();
		}

		internal void Stop()
		{
			port.DataReceived -= Port_DataReceived;
			port.DiscardInBuffer();
			port.Close();
			socket.Close();
		}

		private bool Connect()
		{
			if (!socket.Connected)
			{
				try
				{
					socket.Connect("127.0.0.1", 3636);
					socket.Receive(new byte[1000]);
				}
				catch { return false; }
			}
			return true;
		}
		long? LastUpdated = null;

		public int CurrentBrightness { get => currentBrightness; private set => currentBrightness = value; }

		private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			var data = port.ReadLine();
			if (int.TryParse(data, out int value))
			{
				if (value > 1024) value = 1024;
				else if (value < 0) value = 0;

				value = ConvertAnalogValue(value);
				DesiredBrightness = value;
#if (DIRECT_UPDATE)

				if (Math.Abs(value - CurrentBrightness) >= 10)
				{
					long now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

					if (LastUpdated != null && (now - LastUpdated.Value) < 500) // value isn't stable for 500ms
					{
						LastUpdated = now;
					}
					else
					{
						CurrentBrightness = value;
						SetBrightNess(value);
					}
				}
#endif
			}
		}

		int Map(int value, int minIn, int maxIn, int minOut, int maxOut)
		{
			return (value - minIn) * (maxOut - minOut) / (maxIn - minIn) + minOut;
		}


		private int ConvertAnalogValue(int value)
		{
			const int minAnalog = 100; 
			const int maxAnalog = 900;
			const int maxBrightness = 100;
			const int minBrightness = 20;

			int mapped = Map(value, minAnalog, maxAnalog, maxBrightness, minBrightness); // minOut and maxOut are in reversed order because less impedence => more light
			if (mapped > 100) mapped = 100;
			else if (mapped < 20) mapped = 20;
			return mapped;
		}

		private void GetBrightness()
		{

			if (!Connect()) return;
			byte[] buffer = new byte[100];

			socket.Send("getbrightness\n");
			Thread.Sleep(10);
			socket.Receive(buffer);

			string serializedBrightness = Encoding.ASCII.GetString(buffer);
			if (int.TryParse(serializedBrightness.Split(':')[1], out int brightness))
			{
				CurrentBrightness = brightness;
			}
		}

		private void UpdateBrightness()
		{
			if (DesiredBrightness == null || DesiredBrightness.Value == CurrentBrightness) return;

			int Delta = (DesiredBrightness.Value - CurrentBrightness) / 2;
			if (Delta == 0)
				Delta = DesiredBrightness.Value - CurrentBrightness;

			CurrentBrightness += Delta;

			SetBrightNess(CurrentBrightness);
		}

		private void SetBrightNess(int value)
		{
			BrightnessUpdateHandler?.Invoke(this,value);

			byte[] buffer = new byte[1000];
			if (!Connect()) return;

			socket.Send("lock\n");
			Thread.Sleep(10);

			socket.Send($"setbrightness:{value}\n");
			Thread.Sleep(10);

			socket.Send("unlock\n");
			Thread.Sleep(10);
			socket.Receive(buffer);
		}

		internal void Loop()
		{
			while (true)
			{
				Thread.Sleep(1);
#if (!DIRECT_UPDATE)
				UpdateBrightness();
#endif
			}
		}
	}
}
