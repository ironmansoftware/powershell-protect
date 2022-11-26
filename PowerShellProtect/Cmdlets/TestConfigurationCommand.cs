using Engine;
using Engine.Configuration;
using System.IO;
using System.Management.Automation;

namespace PowerShellProtect.Cmdlets
{
    [Alias("Test-SuspiciousScript")]
    [Cmdlet(VerbsDiagnostic.Test, "PSPConfiguration")]
    public class TestConfigurationCommand : PSCmdlet
    {
        [Parameter(ParameterSetName = "ScriptBlockPath")]
        [Parameter(ParameterSetName = "PathPath")]
        public string ConfigurationPath { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "PathConfig")]
        [Parameter(Mandatory = true, ParameterSetName = "ScriptBlockConfig")]
        public Configuration Configuration { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "ScriptBlockPath")]
        [Parameter(Mandatory = true, ParameterSetName = "ScriptBlockConfig")]
        public ScriptBlock ScriptBlock { get; set; }
        [Parameter(Mandatory = true, ParameterSetName = "PathPath")]
        [Parameter(Mandatory = true, ParameterSetName = "PathConfig")]
        public string FilePath { get; set; }

        protected override void BeginProcessing()
        {
            Log.Logger = (str) => WriteVerbose(str);

            var analyzer = new Analyzer();

            if (ParameterSetName == "PathPath" || ParameterSetName == "ScriptBlockPath")
            {
                if (ConfigurationPath != null)
                {
                    WriteVerbose("Configuration: " + ConfigurationPath);
                    analyzer._config.SetProvider(new XmlConfigProvider(ConfigurationPath));
                }
                else
                {
                    WriteVerbose("Using default configuration.");
                    analyzer._config.SetProvider(new StaticConfigProvider(new Configuration()));
                }
            }
            else
            {
                WriteVerbose("Using provided Configuration.");
                analyzer._config.SetProvider(new StaticConfigProvider(Configuration));
            }

            if (FilePath != null)
            {
                var path = GetUnresolvedProviderPathFromPSPath(FilePath);
                if (!File.Exists(FilePath))
                {
                    throw new System.Exception("File doesn't exist.");
                }

                ScriptBlock = ScriptBlock.Create(File.ReadAllText(path));
            }

            WriteObject(analyzer.Analyze(new ScriptContext
            {
                Script = ScriptBlock.ToString()
            }));
        }
    }
}
