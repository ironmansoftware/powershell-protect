using Engine;
using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using OpenAI_API;
using System.Xml.Serialization;
using System.IO;
using System.Threading.Tasks;

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

            XmlSerializer serializer = new XmlSerializer(typeof(OpenAIResponse));

            var api = new OpenAI_API.OpenAIAPI(condition.APIKey);

            StringBuilder UserMessage = new StringBuilder()
                .AppendLine(Engine.Configuration.OpenAIConfiguration.chatMessagePowerShellBestPractice)
                .AppendLine(context.Script);

            var chat = api.Chat.CreateConversation();
            chat.AppendSystemMessage(Engine.Configuration.OpenAIConfiguration.chatRolePowerShellBestPractice);
            chat.AppendUserInput(UserMessage.ToString());

            OpenAIResponse xmlResponse = null;
            StringReader reader = null;
            
            try
            {
                string response = await chat.GetResponseFromChatbotAsync();
                reader = new StringReader(response);
                
                xmlResponse = (OpenAIResponse)serializer.Deserialize(reader);
            }
            catch(Exception ex)
            {
                if (!condition.ContinueOnError) return false;
            }
            finally
            {
                reader?.Close();
            }
            
            if (xmlResponse != null && xmlResponse.rating >= condition.AIRating) return true;
            return false;  
            
        }
    }
}