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
    public class AIisSuspiciousCondition : ICondition
    {
        public string Name => "isSuspicious";

        public string Description => "The AI considers it suspicious.";

        public bool Analyze(ScriptContext context, Condition condition)
        {
            return this.AnalyzeAsync(context, condition).Result;
        }

        private async Task<bool> AnalyzeAsync(ScriptContext context, Condition condition)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(OpenAIResponse));

            var api = new OpenAI_API.OpenAIAPI(condition.APIKey);

            StringBuilder UserMessage = new StringBuilder()
                .AppendLine(Engine.Configuration.OpenAIConfiguration.chatMessagePowerShellSecurity)
                .AppendLine(context.Script.ToString());

            var chat = api.Chat.CreateConversation();
            chat.AppendSystemMessage(Engine.Configuration.OpenAIConfiguration.chatRolePowerShellSecurity);
            chat.AppendUserInput(UserMessage.ToString());

            OpenAIResponse xmlResponse = null;
            StringReader reader = null;
            
            try
            {
                string response = await chat.GetResponseFromChatbotAsync();
                reader = new StringReader(response);
                xmlResponse = (OpenAIResponse)serializer.Deserialize(reader);
            }
            catch
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