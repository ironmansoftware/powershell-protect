using Engine.Configuration;
using PowerShellProtect.Analyze;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace PowerShellProtect.Cmdlets
{
    [Cmdlet("New", "PSPCondition")]
    public class NewConditionCommand : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        [ValidateSet("admin", "domaincontroller", "computername", "domain", "ApplicationName", "command", "script", "contentpath", "variable", "member", "string", "applicationHash", "assembly", "assemblyHash")]
        public string Property { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "NotEquals")]
        [Alias("Neq")]
        public SwitchParameter NotEquals { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Eq")]
        [Alias("Eq")]
        public new SwitchParameter Equals { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "StartsWith")]
        public SwitchParameter StartsWith { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "EndsWith")]
        public SwitchParameter EndsWith { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "NotStartsWith")]
        public SwitchParameter NotStartsWith { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "NotEndsWith")]
        public SwitchParameter NotEndsWith { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "NotContains")]
        public SwitchParameter NotContains { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Contains")]
        public SwitchParameter Contains { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Matches")]
        public SwitchParameter Matches { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "AI")]
        [Parameter(Mandatory = false)]
        [Alias("Result")]
        public AIResultCondition resultCondition { get; set; } 

        [Parameter(Mandatory = true, ParameterSetName = "AI")]
        [Parameter(Mandatory = false)]
        public aISettings AIConfiguration { get; set; }

        [Parameter(Mandatory = true)]
        public string Value { get; set; }

        protected override void EndProcessing()
        {
            var condition = new Condition
            {
                Property = Property,
                Value = Value
            };

            if (AIConfiguration != null)
            {
                condition.AISettings = AIConfiguration;
            }

            if (Equals)
            {
                condition.Operator = "equals";
            }

            if (StartsWith)
            {
                condition.Operator = "startswith";
            }

            if (EndsWith)
            {
                condition.Operator = "endswith";
            }

            if (Contains)
            {
                condition.Operator = "contains";
            }

            if (NotContains)
            {
                condition.Operator = "notcontains";
            }

            if (NotEquals)
            {
                condition.Operator = "notequals";
            }

            if (NotStartsWith)
            {
                condition.Operator = "notstartswith";
            }

            if (NotEndsWith)
            {
                condition.Operator = "notendswith";
            }

            WriteObject(condition);
        }
    }
}
