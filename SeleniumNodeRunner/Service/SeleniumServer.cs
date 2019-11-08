using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace SeleniumNodeRunner.Service
{
    class SeleniumServer : IDisposable
    {
        private Process process;
        private BackgroundWorker worker;
        private TextBox chromeDrivePath;
        private TextBox seleniumServerPath;
        private TextBox seleniumHubAddress;
        private ComboBox localIPAddress;
        private CheckBox runAsHub;
        private NumericUpDown maxSession;
        private NumericUpDown maxInstances;
		private NumericUpDown timeout;
		private NumericUpDown browserTimeout;
		private NumericUpDown cleanUpCycle;

		public SeleniumServer(
            TextBox chromeDrivePath,
            TextBox seleniumServerPath,
            TextBox seleniumHubAddress,
            ComboBox localIPAddress,
            CheckBox runAsHub,
            NumericUpDown maxSession,
            NumericUpDown maxInstances,
			NumericUpDown browserTimeout,
			NumericUpDown timeout,
			NumericUpDown cleanUpCycle
        )
        {
            this.chromeDrivePath = chromeDrivePath;
            this.seleniumServerPath = seleniumServerPath;
            this.seleniumHubAddress = seleniumHubAddress;
            this.localIPAddress = localIPAddress;
            this.runAsHub = runAsHub;
            this.maxSession = maxSession;
            this.maxInstances = maxInstances;
			this.browserTimeout = browserTimeout;
			this.timeout = timeout;
			this.cleanUpCycle = cleanUpCycle;
		}

        public void Run(Action<string> callback)
        {
            process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.StartInfo.Arguments = "\"-Dwebdriver.chrome.driver=" + this.chromeDrivePath.Text + "\" -jar " + this.seleniumServerPath.Text + " -role " + (runAsHub.Checked ? "hub -timeout "+ timeout.Value.ToString() + " -browserTimeout "+ browserTimeout.Value.ToString() + " -cleanUpCycle "+ cleanUpCycle.Value.ToString() : "webdriver -browser browserName=chrome,maxInstances=" + maxInstances.Value.ToString() + " -maxSession " + maxSession.Value.ToString() + " -hub " + this.seleniumHubAddress.Text) + " -host " + this.localIPAddress.SelectedItem;

            process.StartInfo.CreateNoWindow = true;
            //process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.FileName = "java.exe";

            process.ErrorDataReceived += (s, ev) =>
            {
                callback(ev.Data);
            };
            process.OutputDataReceived += Process_OutputDataReceived;

            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += delegate
            {
                process.Start();

                process.BeginErrorReadLine();
                process.BeginOutputReadLine();

                process.WaitForExit();
            };

            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            //Console.WriteLine(e.Data);
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Do your thing o UI thread
        }

        public void Stop()
        {
            try
            {
                worker.CancelAsync();
                Process proc = Process.GetProcessById(process.Id, System.Environment.MachineName);
                proc.Kill();
            }
            catch (Exception)
            {
                //MessageBox.Show("Something wrong happened. " + e.Message);
            }
        }

        public void Stop(String btnText)
        {
            try
            {
                if (btnText == "Stop")
                {
                    worker.CancelAsync();
                    Process proc = Process.GetProcessById(process.Id, System.Environment.MachineName);
                    proc.Kill();
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("Something wrong happened. " + e.Message);
            }

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
