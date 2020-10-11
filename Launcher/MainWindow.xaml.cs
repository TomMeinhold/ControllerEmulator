using LauncherAPI;
using LauncherAPI.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly FileInfo VersionFile = new FileInfo("version.json");

        private static readonly FileInfo ApplicationFile = new FileInfo("bin/ControllerEmulator.exe");

        private static readonly DirectoryInfo BinDirectory =
            new DirectoryInfo(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName) + "/bin");

        private readonly EventWaitHandle interactionWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Task task = new Task(this.TaskVoid);
            task.Start();
        }

        private void TaskVoid()
        {
            ApplicationVersion currentVersion = ApplicationVersion.GetStoredVersion(VersionFile);
            if (!currentVersion.Installed)
            {
                this.InteractionButton.Dispatcher.Invoke(() =>
                {
                    this.ButtonText.Text = "Install";
                    this.InteractionButton.Visibility = Visibility.Visible;
                }, DispatcherPriority.Render);

                this.interactionWaitHandle.WaitOne();
            }

            ApplicationVersion version = ApplicationVersion.GetLatestVersion();
            if (currentVersion.UpdateRequired(version))
            {
                DownloadService downloadService = new DownloadService(version, this.ProgressBar,
                    this.DownloadSpeedTextBlock, BinDirectory);
                downloadService.DownloadFileCompleted += this.DownloadService_DownloadFileCompleted;
                downloadService.Download();
                version.StoreVersion(VersionFile);
            }
            else
            {
                this.DownloadService_DownloadFileCompleted(null, null);
            }
        }

        private void DownloadService_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Process.Start(ApplicationFile.FullName);
            this.Dispatcher.Invoke(this.Close);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(this.Close);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void InteractionButton_Click(object sender, RoutedEventArgs e)
        {
            this.InteractionButton.Visibility = Visibility.Hidden;
            this.interactionWaitHandle.Set();
        }
    }
}