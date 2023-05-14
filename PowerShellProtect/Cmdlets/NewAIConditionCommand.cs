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
        [Alias("Condition")]        
        public AIResultCondition AIResultCondition { get; set; }

        [Parameter(Mandatory = true)]
        [Alias("Temp")]
        [ValidateRange(-1, 1)]
        public double AITemperature { get; set; }    

        [Parameter(Mandatory = true)]
        [Alias("Model")]
        [ValidateRange(-1, 1)]
        public string AIModel { get; set; }  

        [Parameter(Mandatory = true)]
        [Alias("Rate")]
        [ValidateRange(0, 1)]
        public decimal AIRating { get; set; }    

        [Parameter(Mandatory = true)]
        [Alias("API")]
        public string APIKey { get; set; }   

        [Parameter(Mandatory = false)]
        [Alias("Bypass")]
        public SwitchParameter ContinueOnError { get; set; } 

        protected override void EndProcessing()
        {
            var condition = new Condition
            {
                Property = Property,
                AISettings = new aISettings
                {
                    resultCondition = AIResultCondition,
                    Temperature = AITemperature,
                    Rating = AIRating,
                    APIKey = APIKey,
                    ContinueOnError = ContinueOnError,
                    Model = AIModel
                }

            };

            WriteObject(condition);
        }
    }
}
