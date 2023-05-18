using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace PowerShellProtect.Cmdlets
{
    [Cmdlet("New", "PSPAIConfiguration")]
    public class NewAIConfiguration : PSCmdlet {

        [Parameter(Mandatory = true)]
        [Alias("Temp")]
        [ValidateRange(-1, 1)]
        public double AITemperature { get; set; }    

        [Parameter(Mandatory = true)]
        [Alias("Model")]
        [ValidateSet("gpt-4", "gpt-4-0314", "gpt-4-32k", "gpt-4-32k-0314","gpt-3.5-turbo","gpt-3.5-turbo-0301","text-davinci-003")]
        public string AIModel { get; set; }  
  
        [Parameter(Mandatory = true)]
        [Alias("API")]
        public string APIKey { get; set; }   

        [Parameter(Mandatory = false)]
        [Alias("Bypass")]
        public SwitchParameter ContinueOnError { get; set; } 


        protected override void EndProcessing()
        {
            var condition = new aISettings
            {
                Temperature = AITemperature,
                APIKey = APIKey,
                ContinueOnError = ContinueOnError,
                Model = AIModel
            };

            WriteObject(condition);
        }        

    }
}