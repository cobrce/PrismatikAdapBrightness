using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrismatikAdapBrightness
{
	static class Program
	{


		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form2(SolvePort()));
		}

		private static Config SolvePort()
		{
			var config = Config.Load();
			if (config != null)
				return config;

			if(Form1.GetConfig(null) is Config cfg)
				return cfg;
			else
				Environment.Exit(0);
			return null;
		}

	}
}
