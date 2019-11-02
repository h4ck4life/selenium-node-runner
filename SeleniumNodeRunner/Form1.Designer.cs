namespace SeleniumNodeRunner
{
	partial class Form1
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.linkLabel2 = new System.Windows.Forms.LinkLabel();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtBox_hubaddress = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtBox_seleniumjar = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtBox_ChromeDriver = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.statusStrip1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
			this.notifyIcon1.Text = "Selenium Node Runner";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 464);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(666, 22);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Red;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(62, 17);
			this.toolStripStatusLabel1.Text = "🔴 Offline";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.linkLabel2);
			this.groupBox1.Controls.Add(this.linkLabel1);
			this.groupBox1.Controls.Add(this.groupBox3);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtBox_hubaddress);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtBox_seleniumjar);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.txtBox_ChromeDriver);
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(642, 188);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Configurations";
			// 
			// linkLabel2
			// 
			this.linkLabel2.AutoSize = true;
			this.linkLabel2.Location = new System.Drawing.Point(358, 78);
			this.linkLabel2.Name = "linkLabel2";
			this.linkLabel2.Size = new System.Drawing.Size(55, 13);
			this.linkLabel2.TabIndex = 10;
			this.linkLabel2.TabStop = true;
			this.linkLabel2.Text = "Download";
			this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(358, 29);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(55, 13);
			this.linkLabel1.TabIndex = 9;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Download";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.checkBox2);
			this.groupBox3.Controls.Add(this.checkBox1);
			this.groupBox3.Location = new System.Drawing.Point(432, 30);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(188, 72);
			this.groupBox3.TabIndex = 8;
			this.groupBox3.TabStop = false;
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new System.Drawing.Point(35, 42);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(71, 17);
			this.checkBox2.TabIndex = 8;
			this.checkBox2.Text = "Auto start";
			this.checkBox2.UseVisualStyleBackColor = true;
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.checkBox1.Location = new System.Drawing.Point(35, 19);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(129, 17);
			this.checkBox1.TabIndex = 7;
			this.checkBox1.Text = "Run as Selenium Hub";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 126);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(149, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Selenium Hub HTTP Address:";
			// 
			// txtBox_hubaddress
			// 
			this.txtBox_hubaddress.Location = new System.Drawing.Point(19, 142);
			this.txtBox_hubaddress.Name = "txtBox_hubaddress";
			this.txtBox_hubaddress.Size = new System.Drawing.Size(391, 20);
			this.txtBox_hubaddress.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 78);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(200, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "selenium-server-standalone-x.x.x.jar path:";
			// 
			// txtBox_seleniumjar
			// 
			this.txtBox_seleniumjar.Location = new System.Drawing.Point(19, 94);
			this.txtBox_seleniumjar.Name = "txtBox_seleniumjar";
			this.txtBox_seleniumjar.Size = new System.Drawing.Size(391, 20);
			this.txtBox_seleniumjar.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(118, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "ChromeDriver.exe path:";
			// 
			// txtBox_ChromeDriver
			// 
			this.txtBox_ChromeDriver.AllowDrop = true;
			this.txtBox_ChromeDriver.Location = new System.Drawing.Point(19, 46);
			this.txtBox_ChromeDriver.Name = "txtBox_ChromeDriver";
			this.txtBox_ChromeDriver.Size = new System.Drawing.Size(391, 20);
			this.txtBox_ChromeDriver.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.Location = new System.Drawing.Point(432, 118);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(188, 54);
			this.button1.TabIndex = 0;
			this.button1.Text = "Start";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.ForeColor = System.Drawing.SystemColors.AppWorkspace;
			this.label4.Location = new System.Drawing.Point(580, 469);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(65, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "Made by Alif";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.textBox1);
			this.groupBox2.Location = new System.Drawing.Point(12, 206);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(642, 244);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Output";
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.SystemColors.Control;
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.ForeColor = System.Drawing.SystemColors.WindowText;
			this.textBox1.Location = new System.Drawing.Point(6, 19);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(630, 222);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "java console output here";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(666, 486);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.statusStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "Selenium Node Runner";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.Resize += new System.EventHandler(this.Form1_Resize);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtBox_ChromeDriver;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtBox_seleniumjar;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtBox_hubaddress;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.LinkLabel linkLabel2;
	}
}

