using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PrismatikAdapBrightness.Correctors.AbstractCorrector;

namespace PrismatikAdapBrightness
{
	public partial class Form1 : Form
	{
		internal static Config GetConfig(Config previous)
		{
			using (var form = new Form1(previous))
			{
				if (form.ShowDialog() == DialogResult.OK)
				{
					form.Config.Save();
					return form.Config;
				}
			}
			return null;
		}

		internal Form1(Config previous)
		{
			InitializeComponent();
			Config = previous;
			DialogResult = DialogResult.Cancel;

			switch (previous.CorrectorType)
			{
				case CorrectorTypeEnum.Smooth:
					radioSm.Checked = true;
					break;
				default:
					radioTh.Checked = true;
					break;
			}
		}

		internal Config Config { get; private set; }

		private void comboBox1_DropDown(object sender, EventArgs e)
		{
			UpdateComboBox(comboBox1.SelectedItem is PortAndDescription pnd ? pnd.Port : "");
		}

		internal struct PortAndDescription
		{
			public string Port;
			public string Description;
			public PortAndDescription(string port, string description)
			{
				Port = port;
				Description = description;
			}
			public override string ToString()
			{
				return $"{Port} - {Description}";
			}

		}

		private void UpdateComboBox(string selectedPort)
		{
			object selected = null;
			comboBox1.Items.Clear();
			comboBox1.Items.AddRange(
				Serial.SerialTool.ComPorts()
				.Where((k) => !k.Value.ToLower().Contains("shared"))
				.Select((k) =>
				{
					var pnd = new PortAndDescription(k.Key, k.Value);
					if (k.Key.ToLower().Trim() == selectedPort.ToLower().Trim())
						selected = pnd;
					return (object)pnd;
				}).ToArray());
			comboBox1.SelectedItem = selected;
		}



		private void button1_Click(object sender, EventArgs e)
		{
			if (comboBox1.SelectedItem is PortAndDescription pnd && textBox1.Text != "" && int.TryParse(textBox1.Text, out int bauderate))
			{
				var config = new Config()
				{
					Port = pnd.Port,
					Bauderate = bauderate,
					CorrectorType = radioSm.Checked? CorrectorTypeEnum.Smooth : CorrectorTypeEnum.Threshold,
				};

				if (config.CheckPort())
				{
					this.Config = config;
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
				else
				{
					MessageBox.Show("Can't open selected port");
				}

			}
			else
			{
				MessageBox.Show("Incorrect or incomplete entries");
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			if (Config != null)
			{
				UpdateComboBox(Config.Port);
				textBox1.Text = Config.Bauderate.ToString();
			}
		}
	}
}
