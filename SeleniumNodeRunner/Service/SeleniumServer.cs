using System;
using System.ComponentModel;
using System.Diagnostics;

namespace SeleniumNodeRunner.Service
{
	class SeleniumServer
	{
		private Process process;
		private BackgroundWorker worker;
		private String chromeDrivePath;
		private String seleniumServerPath;
		private String seleniumHubAddress;

		public SeleniumServer(
			String chromeDrivePath,
			String seleniumServerPath,
			String seleniumHubAddress
		)
		{
			this.chromeDrivePath = chromeDrivePath;
			this.seleniumServerPath = seleniumServerPath;
			this.seleniumHubAddress = seleniumHubAddress;
		}

		public void Run(Action<String> callback)
		{
			process = new Process();
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;

			process.StartInfo.Arguments = "\"-Dwebdriver.chrome.driver=C:\\Users\\mohd_alif_abdul_aziz\\Downloads\\chromedriver_win32\\chromedriver.exe\" -jar C:\\Users\\mohd_alif_abdul_aziz\\Downloads\\selenium-server-standalone-3.14.0.jar -role webdriver -hub http://10.108.5.83:4444/grid/register -host 10.93.144.98";

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
