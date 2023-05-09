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
        [ValidateSet("isBestPractice", "isSuspicious")]
        public string Property { get; set; }

        [Parameter(Mandatory = true)]
        [Alias("Temp")]
        [ValidateRange(-1, 1)]
        public string AITemperature { get; set; }    

        [Parameter(Mandatory = true)]
        [Alias("Rate")]
        [ValidateRange(-1, 1)]
        public string AIRating { get; set; }    

        [Parameter(Mandatory = true)]
        [Alias("API")]
        public SecureString APIKey { get; set; }   

        [Parameter(Mandatory = false)]
        [Alias("Bypass")]
        public SwitchParameter ContinueOnError { get; set; } 

        protected override void EndProcessing()
        {
            var condition = new Condition
            {
                Property = Property,
                AITemperature = AITemperature,
                AIRating = AIRating,
                APIKey = SecureStringExtensions.ToBase64(APIKey),
                ContinueOnError = ContinueOnError
            };

            WriteObject(condition);
        }
    }
}
