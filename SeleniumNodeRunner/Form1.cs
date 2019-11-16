using Microsoft.Win32;
using SeleniumNodeRunner.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace SeleniumNodeRunner
{
    public partial class Form1 : Form
    {
        SeleniumServer seleniumServer;
        bool InputFormToggle = true;
        ContextMenu CtxMenuNotifyIcon;
        Timer GridHubStatusTimer = new Timer();

        public Form1()
        {
            InitializeComponent();
        }

        private void checkForUpdate()
        {

        }

        private bool checkIPFamily(string hubAddress, string clientAddress)
        {
            if (checkBox1.Checked == false)
            {
                var pattern = @"\d+";
                Regex rgx = new Regex(pattern);

                var hubAddressMatches = rgx.Matches(hubAddress);
                var clientAddressMatches = rgx.Matches(clientAddress);

                if (hubAddressMatches.Count > 0 && clientAddressMatches.Count > 0)
                {
                    if (hubAddressMatches[0].ToString().Equals(clientAddressMatches[0].ToString()))
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        private async Task<bool> checkIfThereIsActiveTestsAreRunningAsync(String URL)
        {
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(URL),
                    Timeout = TimeSpan.FromSeconds(15)
                };
                var response = await client.GetAsync("grid/api/hub");
                if (response.IsSuccessStatusCode)
                {
                    GridHub gridHub = await response.Content.ReadAsAsync<GridHub>();
                    if (gridHub.SlotCounts.Free != gridHub.SlotCounts.Total && gridHub.NewSessionRequestCount == 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void GridHubCheckTestTimer(bool toggleSwitch)
        {
            GridHubStatusTimer.Tick += new EventHandler(TimerEventProcessor);
            GridHubStatusTimer.Interval = 20000;
            if (toggleSwitch)
            {
                GridHubStatusTimer.Start();
            }
            else
            {
                GridHubStatusTimer.Stop();
            }
        }

        private void TimerEventProcessor(object sender, EventArgs e)
        {
            Task<bool> isTestRunning = Task.Run(() =>
            {
                Task<bool> task = checkIfThereIsActiveTestsAreRunningAsync(txtBox_hubaddress.Text);
                return task;
            });

            if(isTestRunning.Result)
            {
                toolStripStatusLabel1.Text = "⚡ Active tests are running..";
                //label12.Text = "Active tests running";
                //label12.BackColor = System.Drawing.Color.WhiteSmoke;
                //label12.ForeColor = Color.DarkGreen;
            }
            else
            {
                toolStripStatusLabel1.Text = "✔️ Online";
                //label12.Text = "No tests running";
                //label12.BackColor = System.Drawing.Color.WhiteSmoke;
                //label12.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NativeMethods.PreventSleep();
            LoadSettings();
            LoadLocalIPAddress();
            DisplayAppVersion();
            toggle_enabled_hubAddress();

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
                numericUpDown2,
                numericUpDown3,
                numericUpDown4,
                numericUpDown5
            );

            if (checkBox2.Checked)
            {
                if(checkIPFamily(txtBox_hubaddress.Text, comboBox1.SelectedValue.ToString()))
                {
                    WindowState = FormWindowState.Minimized;
                    RunTheSelenium(button1);
                }
                else
                {
                    notifyIcon1.Visible = true;
                    notifyIcon1.ContextMenu = CtxMenuNotifyIcon;
                    notifyIcon1.ShowBalloonTip(500, "Selenium Node Runner", "Not able to start the client. Please select same machine IP address range with Hub IP address - " + comboBox1.SelectedValue.ToString(), ToolTipIcon.Warning);
                }
            }

            GridHubCheckTestTimer(true);

            try
            {
                checkForUpdate();
            }
            catch (Exception) { }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (button1.Text == "Stop")
            {
                Task<bool> isTestRunning = Task.Run(() =>
                {
                    Task<bool> task = checkIfThereIsActiveTestsAreRunningAsync(txtBox_hubaddress.Text);
                    return task;
                });

                isTestRunning.Wait();

                if(isTestRunning.Result)
                {
                    DialogResult result1 = MessageBox.Show("There are active tests still running. Closing the application will interrupt the results.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result1 == DialogResult.Yes)
                    {
                        notifyIcon1.Dispose();
                        seleniumServer.Stop(button1.Text);
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    notifyIcon1.Dispose();
                    seleniumServer.Stop(button1.Text);
                }
            }
            else
            {
                notifyIcon1.Dispose();
                seleniumServer.Stop(button1.Text);
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
            if (txtBox_ChromeDriver.Text == "" || txtBox_seleniumjar.Text == "" || (checkBox1.Checked == false && txtBox_hubaddress.Text == ""))
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
                numericUpDown3.Enabled = false;
                numericUpDown4.Enabled = false;
                numericUpDown5.Enabled = false;
            }
            else
            {
                txtBox_ChromeDriver.Enabled = true;
                if (!checkBox1.Checked)
                {
                    txtBox_hubaddress.Enabled = true;
                }
                txtBox_seleniumjar.Enabled = true;

                comboBox1.Enabled = true;

                checkBox1.Enabled = true;
                //checkBox2.Enabled = true;

                InputFormToggle = true;

                numericUpDown1.Enabled = true;
                numericUpDown2.Enabled = true;
                numericUpDown3.Enabled = true;
                numericUpDown4.Enabled = true;
                numericUpDown5.Enabled = true;
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

        private void ShowOnlineStatus()
        {
            toolStripStatusLabel1.Text = "✔️ Online";
            toolStripStatusLabel1.ForeColor = Color.Green;
        }

        private void ShowOfflineStatus()
        {
            toolStripStatusLabel1.Text = "❌ Offline";
            toolStripStatusLabel1.ForeColor = Color.Red;
        }

        private void RunTheSelenium(Button btnStartStop)
        {
            int notificationCounter = 0;

            tabControl1.SelectTab(2);

            seleniumServer.Run((output) =>
            {
                if (output != null)
                {
                    textBox1.Invoke((Action)delegate
                    {
                        if (output.Contains("Selenium Grid hub is up and running") || output.Contains("registered"))
                        {
                            ShowOnlineStatus();
                        }
                        if (output.Contains("Couldn't register this node"))
                        {
                            ShowOfflineStatus();
                            if (notificationCounter == 0)
                            {
                                notifyIcon1.ShowBalloonTip(500, "Selenium Hub is down", output, ToolTipIcon.Warning);
                                notificationCounter = 30;
                            }
                            else
                            {
                                notificationCounter--;
                            }

                        }
                        if (output.Contains("Removing session") && checkBox3.Checked)
                        {
                            seleniumServer.Stop();
                            Application.Exit();
                        }
                        textBox1.AppendText(output);
                        textBox1.AppendText(Environment.NewLine);
                    }
                );
                }
            });

            btnStartStop.Text = "Stop";

            InputFormsToggle();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button btnStartStop = (Button)sender;

            if (btnStartStop.Text == "Start")
            {
                if (checkIPFamily(txtBox_hubaddress.Text, comboBox1.SelectedValue.ToString()))
                {
                    if (SaveSettings())
                    {
                        RunTheSelenium(btnStartStop);
                    }
                }
                else
                {
                    notifyIcon1.Visible = true;
                    notifyIcon1.ContextMenu = CtxMenuNotifyIcon;
                    notifyIcon1.ShowBalloonTip(500, "Selenium Node Runner", "Not able to start the client. Please select same machine IP address range with Hub IP address - " + comboBox1.SelectedValue.ToString(), ToolTipIcon.Warning);
                }
            }
            else if (btnStartStop.Text == "Stop")
            {
                if (!checkBox1.Checked)
                {
                    Task<bool> isTestRunning = Task.Run(() =>
                    {
                        Task<bool> task = checkIfThereIsActiveTestsAreRunningAsync(txtBox_hubaddress.Text);
                        return task;
                    });

                    isTestRunning.Wait();

                    if (isTestRunning.Result)
                    {
                        DialogResult result1 = MessageBox.Show("There are active tests still running. Stopping the client will interrupt the results.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        seleniumServer.Stop();

                        btnStartStop.Text = "Start";
                        ShowOfflineStatus();

                        textBox1.AppendText(Environment.NewLine);
                        textBox1.AppendText("==================STOPPED===================");
                        textBox1.AppendText(Environment.NewLine);
                        textBox1.AppendText(Environment.NewLine);

                        InputFormsToggle();
                        GridHubCheckTestTimer(false);
                    }
                }
                else
                {
                    seleniumServer.Stop();

                    btnStartStop.Text = "Start";
                    ShowOfflineStatus();

                    textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText("==================STOPPED===================");
                    textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText(Environment.NewLine);

                    InputFormsToggle();
                    GridHubCheckTestTimer(false);
                }
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

            Properties.Settings.Default.AutoRun = checkBox2.Checked;
            Properties.Settings.Default.Save();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/h4ck4life/selenium-node-runner/releases");
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            
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
