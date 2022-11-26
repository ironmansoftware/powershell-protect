using Engine.Configuration;
using System.IO;
using System.Management.Automation;
using System.Text;
using System.Xml.Serialization;

namespace PowerShellProtect.Cmdlets
{
    [Cmdlet(VerbsCommon.Set, "PSPConfiguration")]
    public class SetConfigurationCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, ParameterSetName = "RegistryPath")]
        [Parameter(Mandatory = true, ParameterSetName = "FileSystemPath")]
        public string ConfigurationFilePath { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "RegistryPath")]
        [Parameter(Mandatory = true, ParameterSetName = "RegistryConfig")]
        public SwitchParameter Registry { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "FileSystemPath")]
        [Parameter(Mandatory = true, ParameterSetName = "FileSystemConfig")]
        public SwitchParameter FileSystem { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "FileSystemConfig", ValueFromPipeline = true)]
        [Parameter(Mandatory = true, ParameterSetName = "RegistryConfig", ValueFromPipeline = true)]
        public Configuration Configuration { get; set; }

        protected override void ProcessRecord()
        {
            if (ConfigurationFilePath != null)
            {
                var path = GetUnresolvedProviderPathFromPSPath(ConfigurationFilePath);
                if (!File.Exists(path))
                {
                    throw new System.Exception("Configuration file does not exist.");
                }

                var contents = File.ReadAllText(path);

                if (ParameterSetName == "RegistryPath")
                {
                    var provider = new RegistryConfigProvider();
                    provider.SetConfiguration(contents);
                }

                if (ParameterSetName == "FileSystemPath")
                {
                    var provider = new XmlConfigProvider(@"%ProgramData%\PowerShellProtect\config.xml");
                    provider.SetConfiguration(contents);
                }
            }
            else
            {
                var xmlSerializer = new XmlSerializer(typeof(Configuration));

                string contents;
                using(var memoryStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(memoryStream, Configuration);
                    memoryStream.Position = 0;

                    contents = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
                
                if (ParameterSetName == "RegistryConfig")
                {
                    var provider = new RegistryConfigProvider();
                    provider.SetConfiguration(contents);
                }

                if (ParameterSetName == "FileSystemConfig")
                {
                    var provider = new XmlConfigProvider(@"%ProgramData%\PowerShellProtect\config.xml");
                    provider.SetConfiguration(contents);
                }
            }
        }
    }
}
