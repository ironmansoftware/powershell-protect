using Engine;
using Engine.Configuration;
using System.Management.Automation;
using System.Linq;

namespace PowerShellProtect.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "PSPConfiguration")]
    public class GetConfigurationCommand : PSCmdlet
    {
        protected override void BeginProcessing()
        {
            var config = new Config();
            var configuration = config.GetConfiguration();

            configuration.BuiltInRules = BuiltInConditionCatalog.GetRules().ToList();

            WriteObject(configuration);
        }
    }
}
