using Engine.Configuration;
using System.Management.Automation;

namespace PowerShellProtect.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "PSPConfiguration")]
    public class GetConfigurationCommand : PSCmdlet
    {
        protected override void BeginProcessing()
        {
            var config = new Config();

            WriteObject(config.GetConfiguration());
        }
    }
}
