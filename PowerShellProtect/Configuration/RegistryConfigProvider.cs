using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Engine.Configuration
{
    public class RegistryConfigProvider : IConfigProvider
    {
        public int Precendence => 0;

        public Configuration GetConfiguration()
        {
            using(var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Ironman Software\PowerShell Protect"))
            {
                if (key == null) return null;

                var config = key.GetValue("Configuration").ToString();

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
                
                using (var stringReader = new StringReader(config))
                {
                    return (Configuration)xmlSerializer.Deserialize(stringReader);
                }
            }
        }

        public void SetConfiguration(string configurationXml)
        {
            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Ironman Software\PowerShell Protect"))
            {
                if (key == null) return;

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));

                using (var memoryStream = new MemoryStream())
                {
                    key.SetValue("Configuration", configurationXml);
                }
            }
        }
    }
}
