using Microsoft.Win32;
using SeleniumNodeRunner.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AutoUpdaterDotNET;

namespace SeleniumNodeRunner
{
    public partial class Form1 : Form
    {
        SeleniumServer seleniumServer;
        bool InputFormToggle = true;
        ContextMenu CtxMenuNotifyIcon;

        public Form1()
        {
            InitializeComponent();
        }

        private void checkForUpdate()
        {
            AutoUpdater.DownloadPath = Environment.CurrentDirectory;
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.Start("https://raw.githubusercontent.com/h4ck4life/selenium-node-runner/master/SeleniumNodeRunner/Assets/AutoUpdaterTest.xml");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NativeMethods.PreventSleep();
            LoadSettings();
            LoadLocalIPAddress();
            DisplayAppVersion();

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
                checkBox1,
                numericUpDown1,
                numericUpDown2
            );

            if (checkBox2.Checked)
            {
                RunTheSelenium(button1);
            }

            try
            {
                checkForUpdate();
            }
            catch (Exception) { }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            DialogResult result1 = MessageBox.Show("This will stop running tests. Are you sure want to exit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result1 == DialogResult.Yes)
            {
                seleniumServer.Stop(button1.Text);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void LoadSettings()
        {
            txtBox_ChromeDriver.Text = Properties.Settings.Default.ChromeDriver;
            txtBox_seleniumjar.Text = Properties.Settings.Default.SeleniumJar;
            txtBox_hubaddress.Text = Properties.Settings.Default.HubAddress;

            checkBox1.Checked = Properties.Settings.Default.RunAsHub;
            checkBox2.Checked = Properties.Settings.Default.AutoRun;

            numericUpDown1.Value = decimal.Parse(Properties.Settings.Default.MaxSession);
            numericUpDown2.Value = decimal.Parse(Properties.Settings.Default.MaxInstances);
        }

        private void DisplayAppVersion()
        {
            label8.Text = "Version " + Application.ProductVersion;
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

            Properties.Settings.Default.MaxInstances = numericUpDown1.Value.ToString();
            Properties.Settings.Default.MaxSession = numericUpDown2.Value.ToString();

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

                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = false;
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

                numericUpDown1.Enabled = true;
                numericUpDown2.Enabled = true;
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

        private void RunTheSelenium(Button btnStartStop)
        {
            tabControl1.SelectTab(2);

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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button btnStartStop = (Button)sender;

            if (btnStartStop.Text == "Start")
            {
                if (SaveSettings())
                {
                    RunTheSelenium(btnStartStop);
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

            toggle_enabled_hubAddress();
        }

        private void RegisterInStartup(bool isChecked)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (isChecked)
            {
                registryKey.SetValue("ApplicationName", Application.ExecutablePath);
            }
            else
            {
                registryKey.DeleteValue("ApplicationName");
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

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/SeleniumHQ/selenium/wiki/Grid2#optional-parameters");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox autoStart = (CheckBox)sender;
            RegisterInStartup(autoStart.Checked);
        }
    }

    internal static class NativeMethods
    {
        public static void PreventSleep()
        {
            SetThreadExecutionState(ExecutionState.EsContinuous | ExecutionState.EsSystemRequired);
        }

        public static void AllowSleep()
        {
            SetThreadExecutionState(ExecutionState.EsContinuous);
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);

        [FlagsAttribute]
        private enum ExecutionState : uint
        {
            EsAwaymodeRequired = 0x00000040,
            EsContinuous = 0x80000000,
            EsDisplayRequired = 0x00000002,
            EsSystemRequired = 0x00000001
        }
    }
}
