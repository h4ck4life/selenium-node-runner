using SeleniumNodeRunner.Service;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SeleniumNodeRunner
{
	public partial class Form1 : Form
	{

		SeleniumServer seleniumServer;
		private uint fPreviousExecutionState;
		bool InputFormToggle = true;
		ContextMenu CtxMenuNotifyIcon;

		public Form1()
		{
			InitializeComponent();
			seleniumServer = new SeleniumServer(
				txtBox_ChromeDriver.Text,
				txtBox_seleniumjar.Text,
				txtBox_hubaddress.Text
			);

			// Set new state to prevent system sleep
			fPreviousExecutionState = NativeMethods.SetThreadExecutionState(
				NativeMethods.ES_CONTINUOUS | NativeMethods.ES_SYSTEM_REQUIRED);
			if (fPreviousExecutionState == 0)
			{
				Console.WriteLine("SetThreadExecutionState failed. Do something here...");
				Close();
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			//SeleniumServer.Run(this.textBox1, this);
			textBox1.Text = "hahahaa";

			CtxMenuNotifyIcon = new ContextMenu();
			CtxMenuNotifyIcon.MenuItems.Add("Open", (s, ev) => {
				this.Show();
				WindowState = FormWindowState.Normal;
			});
			CtxMenuNotifyIcon.MenuItems.Add("Exit", (s, ev) => Application.Exit());
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			seleniumServer.Stop(button1.Text);
		}

		private void InputFormsToggle()
		{
			if(InputFormToggle == true)
			{
				txtBox_ChromeDriver.Enabled = false;
				txtBox_hubaddress.Enabled = false;
				txtBox_seleniumjar.Enabled = false;

				checkBox1.Enabled = false;
				checkBox2.Enabled = false;

				InputFormToggle = false;

			} else
			{
				txtBox_ChromeDriver.Enabled = true;
				txtBox_hubaddress.Enabled = true;
				txtBox_seleniumjar.Enabled = true;

				checkBox1.Enabled = true;
				checkBox2.Enabled = true;

				InputFormToggle = true;
			}
			
		}

		private void Form1_Resize(object sender, EventArgs e)
		{
			if (FormWindowState.Minimized == this.WindowState)
			{
				notifyIcon1.Visible = true;
				notifyIcon1.ContextMenu = CtxMenuNotifyIcon;
				notifyIcon1.ShowBalloonTip(500, "Selenium Node Runner", "Selenium Node Runner in background", ToolTipIcon.Info);
				this.Hide();
			}

			else if (FormWindowState.Normal == this.WindowState)
			{
				notifyIcon1.Visible = false;
			}
		}

		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.Show();
			WindowState = FormWindowState.Normal;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Button btnStartStop = (Button)sender;

			if(btnStartStop.Text == "Start")
			{
				seleniumServer.Run((output) => {
					textBox1.Invoke((Action)delegate
						{
							textBox1.AppendText(output);
						}
					);
				});

				btnStartStop.Text = "Stop";
				toolStripStatusLabel1.Text = "✅ Online";
				toolStripStatusLabel1.ForeColor = Color.Green;

				InputFormsToggle();
			} 
			else if (btnStartStop.Text == "Stop")
			{
				seleniumServer.Stop();
				btnStartStop.Text = "Start";
				toolStripStatusLabel1.Text = "🔴 Offline";
				toolStripStatusLabel1.ForeColor = Color.Red;

				InputFormsToggle();

			} else { }
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://chromedriver.chromium.org/downloads");
		}

		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://selenium-release.storage.googleapis.com/index.html");
		}
	}

	internal static class NativeMethods
	{
		// Import SetThreadExecutionState Win32 API and necessary flags
		[DllImport("kernel32.dll")]
		public static extern uint SetThreadExecutionState(uint esFlags);
		public const uint ES_CONTINUOUS = 0x80000000;
		public const uint ES_SYSTEM_REQUIRED = 0x00000001;
	}
}
