using Engine;
using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using OpenAI_API;
using System.IO;
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

            OpenAIResponse aIResponse = null;
            var api = new OpenAI_API.OpenAIAPI(condition.APIKey);
            var serializer = new JsonSerializer();

            StringBuilder UserMessage = new StringBuilder()
                .AppendLine(Engine.Configuration.OpenAIConfiguration.chatMessagePowerShellBestPractice)
                .AppendLine("The PowerShell Script:")
                .AppendLine(context.Script);

            var chat = api.Chat.CreateConversation();
            chat.AppendSystemMessage(Engine.Configuration.OpenAIConfiguration.chatRolePowerShellBestPractice);
            chat.AppendUserInput(UserMessage.ToString());           
            
            try
            {
                string response = await chat.GetResponseFromChatbotAsync();
                aIResponse = JsonConvert.DeserializeObject<OpenAIResponse>(response);
            }
            catch(Exception ex)
            {
                if (!condition.ContinueOnError) return true;
            }
            
            if (aIResponse != null && aIResponse.rating >= condition.AIRating) return false;
            return true;  
            
        }
    }
}