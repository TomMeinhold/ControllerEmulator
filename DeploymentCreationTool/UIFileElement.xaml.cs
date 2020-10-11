using System;
using System.IO;
using System.Windows;

namespace DeploymentCreationTool
{
    /// <summary>
    /// Interaction logic für UIFileElement.xaml
    /// </summary>
    public partial class UIFileElement
    {
        private readonly FileElementManager manager;

        private readonly FileInfo fileInfo;

        public UIFileElement(FileElementManager manager, FileInfo fileInfo)
        {
            this.manager = manager;
            this.fileInfo = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));
            this.InitializeComponent();
            this.FilePath.Text = this.fileInfo.FullName;
        }

        public FileInfo GetFileInfo() => this.fileInfo;

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.manager.Remove(this);
        }
    }
}