using Engine.Configuration;
using System.Management.Automation;

namespace PowerShellProtect.Cmdlets
{
    [Cmdlet("New", "PSPAIConfiguration")]
    public class NewAiConfigurationCommand : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        [ValidateSet("OpenAI", "Anthropic")]
        public string Provider { get; set; }

        [Parameter(Mandatory = true)]
        public string Model { get; set; }

        [Parameter(Mandatory = true)]
        public string ApiKey { get; set; }

        [Parameter]
        public string CustomInstructions { get; set; }

        [Parameter]
        [ValidateRange(1, 300)]
        public int TimeoutSeconds { get; set; } = 30;

        protected override void EndProcessing()
        {
            WriteObject(new AiConfiguration
            {
                Enabled = true,
                Provider = Provider,
                Model = Model,
                ApiKey = ApiKey,
                CustomInstructions = CustomInstructions,
                TimeoutSeconds = TimeoutSeconds
            });
        }
    }
}
