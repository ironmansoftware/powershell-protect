using Engine.Configuration;
using System.IO;
using System.Management.Automation;
using System.Text;
using System.Xml.Serialization;

namespace PowerShellProtect.Cmdlets
{
    [Cmdlet("Save", "PSPConfiguration")]
    public class SaveConfigurationCommand : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Path { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public Configuration Configuration { get; set; }

        protected override void ProcessRecord()
        {
            var path = GetUnresolvedProviderPathFromPSPath(Path);
            var xmlSerializer = new XmlSerializer(typeof(Configuration));

            using (var memoryStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                xmlSerializer.Serialize(memoryStream, Configuration);
            }
        }
    }
}
