using Microsoft.Win32;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Engine.Configuration
{
    public class RegistryFileConfigProvider : IConfigProvider
    {
        public int Precendence => 2;

        public Configuration GetConfiguration()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Ironman Software\PowerShell Protect"))
            {
                if (key == null) return null;

                var config = key.GetValue("ConfigurationFile").ToString();

                config = Environment.ExpandEnvironmentVariables(config);

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
                using (var fileStream = new FileStream(config, FileMode.Open))
                {
                    return (Configuration)xmlSerializer.Deserialize(fileStream);
                }
            }
        }
    }
}
