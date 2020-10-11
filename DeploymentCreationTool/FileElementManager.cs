using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Windows.Controls;

namespace DeploymentCreationTool
{
    public class FileElementManager
    {
        private readonly StackPanel panel;

        private readonly List<UIFileElement> elements = new List<UIFileElement>();

        private readonly bool rewrite;

        public FileElementManager(StackPanel panel)
        {
            this.panel = panel;
            Settings.Default.ZipFiles ??= new StringCollection();

            foreach (var path in Settings.Default.ZipFiles)
            {
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    this.Add(file);
                }
            }

            this.rewrite = true;
        }

        public void Add(FileInfo fileInfo)
        {
            var element = new UIFileElement(this, fileInfo);
            this.elements.Add(element);
            this.panel.Children.Add(element);
            if (!this.rewrite)
            {
                return;
            }

            Settings.Default.ZipFiles.Clear();
            Settings.Default.ZipFiles.AddRange(this.UIFileElementToString());
            Settings.Default.Save();
        }

        public void Remove(UIFileElement element)
        {
            this.elements.Remove(element);
            this.panel.Children.Remove(element);
            if (!this.rewrite)
            {
                return;
            }

            Settings.Default.ZipFiles.Clear();
            Settings.Default.ZipFiles.AddRange(this.UIFileElementToString());
            Settings.Default.Save();
        }

        public FileInfo ZipFiles()
        {
            FileInfo fileInfo = new FileInfo(Path.GetTempPath() + "/" + Guid.NewGuid() + ".zip");
            DirectoryInfo temp = GetTemporaryDirectory();

            foreach (var element in this.elements)
            {
                element.GetFileInfo().CopyTo(temp.FullName + "/" + element.GetFileInfo().Name + element.GetFileInfo().Extension);
            }

            ZipFile.CreateFromDirectory(temp.FullName, fileInfo.FullName);
            return fileInfo;
        }

        public static DirectoryInfo GetTemporaryDirectory()
        {
            string tempDirectory = Path.GetTempPath() + "/" + Guid.NewGuid();
            Directory.CreateDirectory(tempDirectory);
            return new DirectoryInfo(tempDirectory);
        }

        private string[] UIFileElementToString()
        {
            return this.elements.ConvertAll(x => x.GetFileInfo().FullName).ToArray();
        }
    }
}