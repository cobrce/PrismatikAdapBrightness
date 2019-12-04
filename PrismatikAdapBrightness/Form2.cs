using PrismatikAdapBrightness.Correctors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PrismatikAdapBrightness
{
	public partial class Form2 : Form
	{
		NotifyIcon notifyIcon;
		private Config config;
		private BrightnessAdapter adapter;

		internal Form2(Config config)
		{
			InitializeComponent();

			components = new System.ComponentModel.Container();
			notifyIcon = new NotifyIcon(this.components);
			notifyIcon.Icon = this.Icon;
			notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;

			notifyIcon.Text = "Prismatik Brightness Adapter";

			this.config = config;
			DisplayCfg();
			CreateAdapter();
		}

		private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			Show();
			WindowState = FormWindowState.Normal;
			notifyIcon.Visible = false;
		}

		private void CreateAdapter()
		{
			try
			{
				adapter = new BrightnessAdapter(config);
				SetText(txtBrightness, adapter.CurrentBrightness.ToString());
				UpdateNotifyIcon(adapter.CurrentBrightness);
				adapter.BrightnessUpdateHandler += Adapter_BrightnessUpdateHandler;
			}
			catch (TimeoutException)
			{
				MessageBox.Show("Cannot connect to Prismatik : LightPack API Server\n" +
					"Make sure it's enabled and using port 3636");
			}
		}

		private void DisplayCfg()
		{
			SetText(txtPort, config.Port);
			SetText(txtBaude, config.Bauderate.ToString());
		}

		private void Adapter_BrightnessUpdateHandler(object sender, int e)
		{
			SetText(txtBrightness, e.ToString());
			UpdateNotifyIcon(e);
		}

		private void UpdateNotifyIcon(int e)
		{
			notifyIcon.Text = $"Prismatik Brightness Adapter\nCurrent brightness : {e}";
		}

		private void SetText(TextBox textbox, string text)
		{
			if (textbox.InvokeRequired)
				textbox.Invoke(new Action<TextBox, string>(SetText), new object[] { textbox, text });
			else
				textbox.Text = text;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			adapter.Stop();
			if (Form1.GetConfig(config) is Config cfg)
				config = cfg;
			DisplayCfg();
			CreateAdapter();

		}

		private void Form2_Resize(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Minimized)
			{
				Hide();
				notifyIcon.Visible = true;
			}
		}
	}
}
