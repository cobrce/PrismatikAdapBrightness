﻿namespace PrismatikAdapBrightness
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.radioSm = new System.Windows.Forms.RadioButton();
			this.radioTh = new System.Windows.Forms.RadioButton();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(11, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Port";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(11, 47);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(77, 20);
			this.label2.TabIndex = 1;
			this.label2.Text = "Bauderate";
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(94, 10);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(259, 28);
			this.comboBox1.TabIndex = 2;
			this.comboBox1.DropDown += new System.EventHandler(this.comboBox1_DropDown);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(94, 44);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(151, 27);
			this.textBox1.TabIndex = 3;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(359, 11);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(81, 27);
			this.button1.TabIndex = 4;
			this.button1.Text = "Save";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// radioSm
			// 
			this.radioSm.AutoSize = true;
			this.radioSm.Location = new System.Drawing.Point(358, 47);
			this.radioSm.Name = "radioSm";
			this.radioSm.Size = new System.Drawing.Size(82, 24);
			this.radioSm.TabIndex = 3;
			this.radioSm.Text = "Smooth";
			this.radioSm.UseVisualStyleBackColor = true;
			// 
			// radioTh
			// 
			this.radioTh.AutoSize = true;
			this.radioTh.Location = new System.Drawing.Point(257, 47);
			this.radioTh.Name = "radioTh";
			this.radioTh.Size = new System.Drawing.Size(95, 24);
			this.radioTh.TabIndex = 3;
			this.radioTh.Text = "Threshold";
			this.radioTh.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(443, 81);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.radioSm);
			this.Controls.Add(this.radioTh);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "PrismatikAdaptBrightness by COB - Config";
			this.Load += new System.EventHandler(this.Form1_Load);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.RadioButton radioSm;
		private System.Windows.Forms.RadioButton radioTh;
	}
}

