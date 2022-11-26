using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace PowerShellProtect.Cmdlets
{
    [Cmdlet("New", "PSPRule")]
    public class NewRuleCommand : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [Parameter()]
        public Condition[] Condition { get; set; }

        [Parameter()]
        public Engine.Configuration.Action[] Action { get; set; }
        [Parameter()]
        public SwitchParameter AnyCondition { get; set; }

        protected override void EndProcessing()
        {
            WriteObject(new Rule
            {
                Actions = Action?.Select(m => new ActionRef { Name = m.Name }).ToList(),
                Conditions = Condition?.ToList(),
                Name = Name,
                AnyCondition = AnyCondition
            });
        }
    }
}
