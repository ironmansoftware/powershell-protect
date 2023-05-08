using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Security.SecureString;
using System.SecureStringExtensions;
using System.Text;

namespace PowerShellProtect.Cmdlets
{
    [Cmdlet("New", "PSPAICondition")]
    public class NewAIConditionCommand : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        [ValidateSet("isBestPractice", "isSuspicious", "isSafe")]
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

        [Parameter(Mandatory)]
        [Alias("Bypass")]
        [ValidateRange(-1, 1)]
        public SwitchParameter ContinueOnError { get; set; } 

        protected override void EndProcessing()
        {
            var condition = new AICondition
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
