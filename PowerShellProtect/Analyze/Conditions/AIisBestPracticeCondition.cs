using Engine;
using Engine.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PowerShellProtect.Analyze.Conditions
{
    public class AIisBestPracticeCondition : ICondition
    {
        public string Name => "isBestPractice";

        public string Description => "AI security best practices must be followed by scripts that are executed.";

        public bool Analyze(ScriptContext context, Condition condition)
        {
            return this.AnalyzeAsync(context, condition).Result;
        }

        public async Task<bool> AnalyzeAsync(ScriptContext context, Condition condition)
        {

            return (await OpenAIHelper.SendToOpenAI(
                context, 
                condition, 
                Engine.Configuration.OpenAIConfiguration.chatMessagePowerShellBestPractice,
                Engine.Configuration.OpenAIConfiguration.chatRolePowerShellBestPractice)
            );                       
        }
    }
}