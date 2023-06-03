using Engine;
using Engine.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PowerShellProtect.Analyze.Conditions
{
    public class AIisisObfuscatedCondition : ICondition
    {
        public string Name => "isObfuscated";

        public string Description => "Code should not be obfuscated.";

        public bool Analyze(ScriptContext context, Condition condition)
        {
            return this.AnalyzeAsync(context, condition).Result;
        }

        public async Task<bool> AnalyzeAsync(ScriptContext context, Condition condition)
        {

            return (await OpenAIHelper.SendToOpenAI(
                context, 
                condition, 
                Engine.Configuration.OpenAIConfiguration.chatMessagePowerShellObfuscation,
                Engine.Configuration.OpenAIConfiguration.chatRolePowerShellObfuscate)
            ); 
                      
        }
    }
}