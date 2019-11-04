using SeleniumNodeRunner.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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

            LoadSettings();
            LoadLocalIPAddress();

            CtxMenuNotifyIcon = new ContextMenu();
            CtxMenuNotifyIcon.MenuItems.Add("Open", (s, ev) =>
            {
                this.Show();
                WindowState = FormWindowState.Normal;
            });
            CtxMenuNotifyIcon.MenuItems.Add("Exit", (s, ev) => Application.Exit());

            seleniumServer = new SeleniumServer(
                txtBox_ChromeDriver,
                txtBox_seleniumjar,
                txtBox_hubaddress,
                comboBox1,
                checkBox1
            );
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            seleniumServer.Stop(button1.Text);
        }

        private void LoadSettings()
        {
            txtBox_ChromeDriver.Text = Properties.Settings.Default.ChromeDriver;
            txtBox_seleniumjar.Text = Properties.Settings.Default.SeleniumJar;
            txtBox_hubaddress.Text = Properties.Settings.Default.HubAddress;

            checkBox1.Checked = Properties.Settings.Default.RunAsHub;
            checkBox2.Checked = Properties.Settings.Default.AutoRun;
        }

        private bool SaveSettings()
        {

            if (txtBox_ChromeDriver.Text == "" || txtBox_seleniumjar.Text == "" || txtBox_hubaddress.Text == "")
            {
                MessageBox.Show("Please set all the configurations",
                    "Info",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                txtBox_ChromeDriver.Focus();
                return false;
            }

            Properties.Settings.Default.ChromeDriver = txtBox_ChromeDriver.Text;
            Properties.Settings.Default.SeleniumJar = txtBox_seleniumjar.Text;
            Properties.Settings.Default.HubAddress = txtBox_hubaddress.Text;

            Properties.Settings.Default.RunAsHub = checkBox1.Checked;
            Properties.Settings.Default.AutoRun = checkBox2.Checked;

            Properties.Settings.Default.Save();

            return true;
        }

        private void LoadLocalIPAddress()
        {
            List<string> items = new List<string>();

            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress addr in localIPs)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    items.Add(addr.ToString());
                }
            }

            comboBox1.DataSource = items;
        }

        private void InputFormsToggle()
        {
            if (InputFormToggle == true)
            {
                txtBox_ChromeDriver.Enabled = false;
                txtBox_hubaddress.Enabled = false;
                txtBox_seleniumjar.Enabled = false;

                comboBox1.Enabled = false;

                checkBox1.Enabled = false;
                //checkBox2.Enabled = false;

                InputFormToggle = false;

            }
            else
            {
                txtBox_ChromeDriver.Enabled = true;
                txtBox_hubaddress.Enabled = true;
                txtBox_seleniumjar.Enabled = true;

                comboBox1.Enabled = true;

                checkBox1.Enabled = true;
                //checkBox2.Enabled = true;

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

            if (btnStartStop.Text == "Start")
            {

                if (SaveSettings())
                {
                    tabControl1.SelectTab(1);

                    seleniumServer.Run((output) =>
                    {
                        if (output != null)
                        {
                            textBox1.Invoke((Action)delegate
                            {
                                textBox1.AppendText(output);
                                textBox1.AppendText(Environment.NewLine);
                            }
                        );
                        }
                    });

                    btnStartStop.Text = "Stop";
                    toolStripStatusLabel1.Text = "✅ Online";
                    toolStripStatusLabel1.ForeColor = Color.Green;

                    InputFormsToggle();
                    toggle_enabled_hubAddress();

                }
            }
            else if (btnStartStop.Text == "Stop")
            {
                seleniumServer.Stop();
                btnStartStop.Text = "Start";
                toolStripStatusLabel1.Text = "🔴 Offline";
                toolStripStatusLabel1.ForeColor = Color.Red;

                textBox1.AppendText(Environment.NewLine);
                textBox1.AppendText("==================STOPPED===================");
                textBox1.AppendText(Environment.NewLine);
                textBox1.AppendText(Environment.NewLine);

                InputFormsToggle();
                toggle_enabled_hubAddress();

            }
            else { }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://chromedriver.chromium.org/downloads");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://selenium-release.storage.googleapis.com/index.html");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox1_sender = (CheckBox)sender;

            if (checkbox1_sender.Checked)
            {
                txtBox_hubaddress.Enabled = false;
            }
            else
            {
                txtBox_hubaddress.Enabled = true;
            }
        }

        private void toggle_enabled_hubAddress()
        {
            if (checkBox1.Checked)
            {
                txtBox_hubaddress.Enabled = false;
                this.Text = "Server - Selenium Node Runner";
            }
            else
            {
                txtBox_hubaddress.Enabled = true;
                this.Text = "Client - Selenium Node Runner";
            }
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
