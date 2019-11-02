using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace SeleniumNodeRunner.Service
{
	class SeleniumServer
	{
		private Process process;
		private BackgroundWorker worker;
		private TextBox chromeDrivePath;
		private TextBox seleniumServerPath;
		private TextBox seleniumHubAddress;
		private ComboBox localIPAddress;

		public SeleniumServer(
			TextBox chromeDrivePath,
			TextBox seleniumServerPath,
			TextBox seleniumHubAddress,
			ComboBox localIPAddress
		)
		{
			this.chromeDrivePath = chromeDrivePath;
			this.seleniumServerPath = seleniumServerPath;
			this.seleniumHubAddress = seleniumHubAddress;
			this.localIPAddress = localIPAddress;
		}

		public void Run(Action<String> callback)
		{
			process = new Process();
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;

			process.StartInfo.Arguments = "\"-Dwebdriver.chrome.driver="+ this.chromeDrivePath.Text + "\" -jar " + this.seleniumServerPath.Text + " -role webdriver -hub http://" + this.seleniumHubAddress.Text + ":4444/grid/register -host "+ this.localIPAddress.SelectedItem;

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
			worker.CancelAsync();
			Process proc = Process.GetProcessById(process.Id, System.Environment.MachineName);
			proc.Kill();
		}

		public void Stop(String btnText)
		{
			if (btnText == "Stop")
			{
				worker.CancelAsync();
				Process proc = Process.GetProcessById(process.Id, System.Environment.MachineName);
				proc.Kill();
			}
		}
	}
}
