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
			new BrightnessAdapter(SolvePort()).Loop();
		}

		private static Config SolvePort()
		{
			var config = Config.Load();
			if (config != null)
				return config;
			using (var form = new Form1())
			{
				if (form.ShowDialog() == DialogResult.OK)
				{
					form.Config.Save();
					return form.Config;
				}
			}
			Environment.Exit(0);
			return null;
		}
	}
}
