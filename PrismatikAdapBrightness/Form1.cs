using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrismatikAdapBrightness
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			DialogResult = DialogResult.Cancel;
		}

		internal Config Config { get; private set; }

		private void comboBox1_DropDown(object sender, EventArgs e)
		{
			comboBox1.Items.Clear();
			comboBox1.Items.AddRange(
				Serial.SerialTool.ComPorts()
				.Where((k) => !k.Value.ToLower().Contains("shared"))
				.Select((k) => $"{k.Key} - {k.Value}").ToArray());
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (comboBox1.Text != "" && textBox1.Text != "" && int.TryParse(textBox1.Text, out int bauderate))
			{
				var config = new Config()
				{
					Port = comboBox1.Text.Split('-', StringSplitOptions.RemoveEmptyEntries)[0],
					Bauderate = bauderate
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
	}
}
