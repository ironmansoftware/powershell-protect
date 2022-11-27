using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace PowerShellProtect.Cmdlets
{
    [Cmdlet("New", "PSPConfiguration")]
    public class NewConfigurationCommand : PSCmdlet
    {
        [Parameter]
        public Engine.Configuration.Action[] Action { get; set; }

        [Parameter]
        public Rule[] Rule { get; set; }

        [Parameter]
        public SwitchParameter DisableBuiltInActions { get; set; }

        [Parameter]
        public string[] DisabledBuiltInConditions { get; set; } = new string[0];

        protected override void EndProcessing()
        {
            var configuration = new Configuration
            {
                Actions = Action?.ToList(),
                Rules = Rule?.ToList(),
                BuiltIn = new BuiltIn
                {
                    DisabledConditions = DisabledBuiltInConditions,
                    Actions = Action?.Select(m => new ActionRef { Name = m.Name }).ToList(),
                    Enabled = !DisableBuiltInActions.IsPresent
                }
            };

            WriteObject(configuration);
        }
    }
}
