using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Security;
using PowerShellProtect.Analyze;
using System.Text;

namespace PowerShellProtect.Cmdlets
{
    [Cmdlet("New", "PSPAICondition")]
    public class NewAIConditionCommand : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        [ValidateSet("isBestPractice", "isSuspicious", "isObfuscated")]
        public string Property { get; set; }

        [Parameter(Mandatory = true)]
        [Alias("Rate")]
        [ValidateRange(0, 1)]
        public decimal Rating { get; set; } 

        [Parameter(Mandatory = true)]
        [Alias("Result")]
        public AIResultCondition resultCondition { get; set; } 

        [Parameter(Mandatory = true)]
        public aISettings AIConfiguration { get; set; }

        protected override void EndProcessing()
        {
            var condition = new Condition
            {
                Property = Property,
                AISettings = AIConfiguration
            };

            // Append the Rating and the Result Condition to the AIConfiguration
            condition.AISettings.Rating = Rating;
            condition.AISettings.resultCondition = resultCondition;

            WriteObject(condition);
        }
    }
}
