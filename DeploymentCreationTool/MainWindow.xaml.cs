using LauncherAPI.Extensions;
using LauncherAPI.Json;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DeploymentCreationTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public partial class MainWindow
    {
        private readonly bool ignoreUpdate;

        private readonly JsonSerializerOptions options =
            new JsonSerializerOptions { WriteIndented = true, AllowTrailingCommas = true };

        private ApplicationVersion applicationVersion = new ApplicationVersion();

        private readonly FileElementManager manager;

        public MainWindow()
        {
            this.ignoreUpdate = true;
            this.InitializeComponent();
            this.manager = new FileElementManager(this.FilePanel);
            this.JsonFilePath.Text = Settings.Default.JsonFilePath;
            this.ZipFilePath.Text = Settings.Default.ZipFilePath;
            this.AutoImportButton.IsChecked = Settings.Default.AutoImport;
            this.ignoreUpdate = false;
            if (Settings.Default.AutoImport)
            {
                this.ImportButton_Click(null, null);
            }
            else
            {
                this.DisplayJson();
            }
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            foreach (var path in (string[])e.Data.GetData(DataFormats.FileDrop, false) ?? Array.Empty<string>())
            {
                FileInfo fileInfo = new FileInfo(path);
                if (fileInfo.Exists)
                {
                    this.manager.Add(fileInfo);
                }
            }
        }

        private void UrlTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.applicationVersion.Url = new Uri(this.UrlTextBox.Text);
            this.DisplayJson();
        }

        private void VersionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.applicationVersion.Version = this.VersionTextBox.Text;
            this.DisplayJson();
        }

        private void DisplayJson()
        {
            if (!this.ignoreUpdate)
            {
                this.JsonOutput.Text = JsonSerializer.Serialize(this.applicationVersion, this.options);
            }
        }

        private void ExportFilePath_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog { Filter = "Json files (*.json)|*.json", FilterIndex = 0 };

            if (!dialog.ShowDialog() ?? false)
            {
                return;
            }

            this.JsonFilePath.Text = Settings.Default.JsonFilePath = dialog.FileName;
            Settings.Default.Save();
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            this.ExportProgressBar.Value = 1;
            this.ExportProgressBar.Foreground = Brushes.Yellow;
            FileInfo tmpZip = this.manager.ZipFiles();
            tmpZip.CopyTo(Settings.Default.ZipFilePath, true);
            this.applicationVersion.Hash = Encoding.UTF8.GetString(new FileInfo(Settings.Default.ZipFilePath).CreateHash());
            this.DisplayJson();

            FileInfo fileInfo = new FileInfo(Settings.Default.JsonFilePath);
            Stream stream = fileInfo.Open(FileMode.Create);
            byte[] buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this.applicationVersion, this.options));
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Close();
            this.ExportProgressBar.Foreground = Brushes.Green;
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.Default.JsonFilePath))
            {
                return;
            }

            FileInfo fileInfo = new FileInfo(Settings.Default.JsonFilePath);
            if (!fileInfo.Exists)
            {
                return;
            }

            Stream stream = fileInfo.Open(FileMode.Open);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Close();
            this.applicationVersion = JsonSerializer.Deserialize<ApplicationVersion>(Encoding.UTF8.GetString(buffer));
            this.VersionTextBox.Text = this.applicationVersion.Version;
            this.UrlTextBox.Text = this.applicationVersion.Url.OriginalString;
            this.DisplayJson();
        }

        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog { Multiselect = true, CheckFileExists = true };

            if (!dialog.ShowDialog() ?? false)
            {
                return;
            }

            foreach (var file in dialog.FileNames)
            {
                if (!string.IsNullOrEmpty(file))
                {
                    this.manager.Add(new FileInfo(file));
                }
            }
        }

        private void ZipFilePath_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog { Filter = "Zip files (*.zip)|*.zip", FilterIndex = 0 };
            if (!dialog.ShowDialog() ?? false)
            {
                return;
            }

            this.ZipFilePath.Text = Settings.Default.ZipFilePath = dialog.FileName;
            Settings.Default.Save();
        }

        private void AutoImportButton_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.AutoImport = !Settings.Default.AutoImport;
            Settings.Default.Save();
        }
    }
}