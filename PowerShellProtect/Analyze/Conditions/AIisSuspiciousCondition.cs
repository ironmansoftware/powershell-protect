using Engine;
using Engine.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PowerShellProtect.Analyze.Conditions
{
    public class AIisSuspiciousCondition : ICondition
    {
        public string Name => "isSuspicious";

        public string Description => "The AI considers it suspicious.";

        public bool Analyze(ScriptContext context, Condition condition)
        {
            return this.AnalyzeAsync(context, condition).Result;
        }

         public async Task<bool> AnalyzeAsync(ScriptContext context, Condition condition)
        {

            return (await OpenAIHelper.SendToOpenAI(
                context, 
                condition, 
                Engine.Configuration.OpenAIConfiguration.chatMessagePowerShellSecurity,
                Engine.Configuration.OpenAIConfiguration.chatRolePowerShellSecurity)
            ); 
                      
        }
    }
}