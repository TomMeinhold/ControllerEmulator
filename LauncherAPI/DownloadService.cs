using LauncherAPI.Extensions;
using LauncherAPI.Humanize;
using LauncherAPI.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Windows.Controls;
using System.Windows.Threading;

namespace LauncherAPI
{
    public class DownloadService
    {
        private readonly DirectoryInfo dest;

        private readonly ProgressBar progressBar;

        private readonly Stopwatch stopwatch = new Stopwatch();

        private readonly FileInfo tempFile;

        private readonly TextBlock textBlock;

        private readonly ApplicationVersion version;

        private long lastBytes;

        private DateTime lastUpdate;

        public DownloadService(ApplicationVersion version, ProgressBar progressBar, TextBlock textBlock,
            DirectoryInfo dest)
        {
            this.version = version;
            this.progressBar = progressBar;
            this.textBlock = textBlock;
            this.dest = dest;
            this.tempFile = new FileInfo(Path.GetTempFileName()) { Attributes = FileAttributes.Temporary };
        }

        public event EventHandler<AsyncCompletedEventArgs> DownloadFileCompleted;

        public void Download()
        {
            this.progressBar.Dispatcher.Invoke(() => this.progressBar.Maximum = 100, DispatcherPriority.Render);
            using WebClient client = new WebClient();
            client.DownloadFileCompleted += this.Client_DownloadFileCompleted;
            client.DownloadProgressChanged += this.Client_DownloadProgressChanged;
            this.stopwatch.Start();
            client.DownloadFileAsync(this.version.Url, this.tempFile.FullName);
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.progressBar.Dispatcher.Invoke(() => this.progressBar.Value = e.ProgressPercentage,
                DispatcherPriority.Render);

            if (this.stopwatch.Elapsed.Seconds < 0.2)
            {
                return;
            }

            this.stopwatch.Restart();
            if (this.lastBytes == 0)
            {
                this.lastUpdate = DateTime.Now;
                this.lastBytes = e.BytesReceived;
                return;
            }

            this.CalculateDownloadSpeed(e);
        }

        private void CalculateDownloadSpeed(DownloadProgressChangedEventArgs e)
        {
            DateTime now = DateTime.Now;
            TimeSpan timeSpan = now - this.lastUpdate;
            long bytesChange = e.BytesReceived - this.lastBytes;
            double bytesPerSecond = bytesChange / timeSpan.TotalSeconds;
            TimeSpan downloadTime = new TimeSpan(0, 0, (int)(e.TotalBytesToReceive / bytesPerSecond));

            this.lastBytes = e.BytesReceived;
            this.lastUpdate = now;

            this.textBlock.Dispatcher.Invoke(() => this.textBlock.Text = $"{bytesPerSecond.SpeedSuffix()} ({downloadTime:d\\.hh\\:mm\\:ss} remaining.) ");
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (Encoding.UTF8.GetString(this.tempFile.CreateHash()) != this.version.Hash)
            {
                // Retry download because file is corrupted.
                this.Download();
            }
            else
            {
                // Extract zip file.
                this.textBlock.Dispatcher.Invoke(() => this.textBlock.Text = "Extracting ..");
                ZipFile.ExtractToDirectory(this.tempFile.FullName, this.dest.FullName, true);
                this.tempFile.Delete();

                // Invoke DownloadFileCompleted.
                this.DownloadFileCompleted?.Invoke(this, new AsyncCompletedEventArgs(null, false, null));
            }
        }
    }
}