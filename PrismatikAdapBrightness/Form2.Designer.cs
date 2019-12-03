namespace PrismatikAdapBrightness
{
	partial class Form2
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtBrightness = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.txtBaude = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Port";
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(54, 5);
			this.txtPort.Name = "txtPort";
			this.txtPort.ReadOnly = true;
			this.txtPort.Size = new System.Drawing.Size(74, 27);
			this.txtPort.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(129, 20);
			this.label2.TabIndex = 0;
			this.label2.Text = "Current Brightness";
			// 
			// txtBrightness
			// 
			this.txtBrightness.Location = new System.Drawing.Point(148, 38);
			this.txtBrightness.Name = "txtBrightness";
			this.txtBrightness.ReadOnly = true;
			this.txtBrightness.Size = new System.Drawing.Size(181, 27);
			this.txtBrightness.TabIndex = 1;
			this.txtBrightness.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(335, 6);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(100, 59);
			this.button1.TabIndex = 2;
			this.button1.Text = "Config";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// txtBaude
			// 
			this.txtBaude.Location = new System.Drawing.Point(209, 5);
			this.txtBaude.Name = "txtBaude";
			this.txtBaude.ReadOnly = true;
			this.txtBaude.Size = new System.Drawing.Size(120, 27);
			this.txtBaude.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(134, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(69, 20);
			this.label3.TabIndex = 0;
			this.label3.Text = "Baudrate";
			// 
			// Form2
			// 
			this.ClientSize = new System.Drawing.Size(443, 81);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtPort);
			this.Controls.Add(this.txtBrightness);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtBaude);
			this.Controls.Add(this.label3);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Form2";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "PrismatikAdaptBrightness by COB - Main";
			this.Resize += new System.EventHandler(this.Form2_Resize);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtBrightness;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox txtBaude;
		private System.Windows.Forms.Label label3;
	}
}