using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

namespace DeploymentCreationTool
{
    public sealed class PortableSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        private const string RootNodeName = "settings";

        private const string LocalSettingsNodeName = "localSettings";

        private const string GlobalSettingsNodeName = "globalSettings";

        private const string ClassName = "PortableSettingsProvider";

        private XmlDocument xmlDocument;

        private string FilePath => Path.Combine(
            Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName) ?? string.Empty,
            $"{this.ApplicationName}.settings");

        private XmlNode LocalSettingsNode
        {
            get
            {
                XmlNode settingsNode = this.GetSettingsNode(LocalSettingsNodeName);
                XmlNode machineNode = settingsNode.SelectSingleNode(Environment.MachineName.ToUpperInvariant());

                if (machineNode != null)
                {
                    return machineNode;
                }

                machineNode = this.RootDocument.CreateElement(Environment.MachineName.ToUpperInvariant());
                settingsNode.AppendChild(machineNode);

                return machineNode;
            }
        }

        private XmlNode GlobalSettingsNode => this.GetSettingsNode(GlobalSettingsNodeName);

        private XmlNode RootNode => this.RootDocument.SelectSingleNode(RootNodeName);

        private XmlDocument RootDocument
        {
            get
            {
                if (this.xmlDocument != null)
                {
                    return this.xmlDocument;
                }

                try
                {
                    this.xmlDocument = new XmlDocument();
                    this.xmlDocument.Load(this.FilePath);
                }
#pragma warning disable CA1031 //
                catch (Exception)
#pragma warning restore CA1031 //
                {
                    // ignored
                }

                if (this.xmlDocument?.SelectSingleNode(RootNodeName) != null)
                {
                    return this.xmlDocument;
                }

                this.xmlDocument = GetBlankXmlDocument();

                return this.xmlDocument;
            }
        }

        public override string ApplicationName
        {
            get => Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule?.FileName);
            set { }
        }

        public override string Name => ClassName;

        public void Reset(SettingsContext context)
        {
            this.LocalSettingsNode.RemoveAll();
            this.GlobalSettingsNode.RemoveAll();

            this.xmlDocument.Save(this.FilePath);
        }

        public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
        {
            // do nothing
            return new SettingsPropertyValue(property);
        }

        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        {
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(this.Name, config);
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (SettingsPropertyValue propertyValue in collection)
            {
                this.SetValue(propertyValue);
            }

            try
            {
                this.RootDocument.Save(this.FilePath);
            }
            catch (XmlException)
            {
                /*
                 * If this is a portable application and the device has been
                 * removed then this will fail, so don't do anything. It's
                 * probably better for the application to stop saving settings
                 * rather than just crashing outright. Probably.
                 */
            }
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context,
            SettingsPropertyCollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();

            foreach (SettingsProperty property in collection)
            {
                values.Add(new SettingsPropertyValue(property) { SerializedValue = this.GetValue(property) });
            }

            return values;
        }

        private void SetValue(SettingsPropertyValue propertyValue)
        {
            XmlNode targetNode = IsGlobal(propertyValue.Property)
                ? this.GlobalSettingsNode
                : this.LocalSettingsNode;

            XmlNode settingNode = targetNode.SelectSingleNode($"setting[@name='{propertyValue.Name}']");

            if (settingNode != null)
            {
                settingNode.InnerText = propertyValue.SerializedValue.ToString();
            }
            else
            {
                settingNode = this.RootDocument.CreateElement("setting");

                XmlAttribute nameAttribute = this.RootDocument.CreateAttribute("name");
                nameAttribute.Value = propertyValue.Name;

                settingNode.Attributes.Append(nameAttribute);
                settingNode.InnerText = propertyValue.SerializedValue.ToString();

                targetNode.AppendChild(settingNode);
            }
        }

        private string GetValue(SettingsProperty property)
        {
            XmlNode targetNode = IsGlobal(property) ? this.GlobalSettingsNode : this.LocalSettingsNode;
            XmlNode settingNode = targetNode.SelectSingleNode($"setting[@name='{property.Name}']");

            if (settingNode == null)
            {
                return property.DefaultValue != null ? property.DefaultValue.ToString() : string.Empty;
            }

            return settingNode.InnerText;
        }

        private static bool IsGlobal(SettingsProperty property)
        {
            return property.Attributes.Cast<DictionaryEntry>()
                .Any(attribute => (Attribute)attribute.Value is SettingsManageabilityAttribute);
        }

        private XmlNode GetSettingsNode(string name)
        {
            XmlNode settingsNode = this.RootNode.SelectSingleNode(name);

            if (settingsNode != null)
            {
                return settingsNode;
            }

            settingsNode = this.RootDocument.CreateElement(name);
            this.RootNode.AppendChild(settingsNode);

            return settingsNode;
        }

        public static XmlDocument GetBlankXmlDocument()
        {
            XmlDocument blankXmlDocument = new XmlDocument();
            blankXmlDocument.AppendChild(blankXmlDocument.CreateXmlDeclaration("1.0", "utf-8", string.Empty));
            blankXmlDocument.AppendChild(blankXmlDocument.CreateElement(RootNodeName));

            return blankXmlDocument;
        }
    }
}