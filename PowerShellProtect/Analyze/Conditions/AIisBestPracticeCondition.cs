using Engine;
using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using OpenAI_API;
using System.Xml.Serialization;
using System.IO;

namespace PowerShellProtect.Analyze.Conditions
{
    public class AIisBestPracticeCondition : ICondition
    {
        public string Name => "AIisBestPractice";

        public string Description => "AI security best practices must be followed by scripts that are executed.";

        public bool Analyze(ScriptContext context, Condition condition)
        {

            string xmlResponse;

            var api = new OpenAI_API.OpenAI_API(condition.APIKey.ToInsecureString());

            string UserMessage = new StringBuilder()
                .AppendLine(Engine.Configuration.OpenAIConfiguration.chatMessagePowerShellBestPractice)
                .AppendLine(context.Script);

            var chat = api.Chat.CreateConverstation();
            chat.AppendSystemMessage(Engine.Configuration.OpenAIConfiguration.chatRolePowerShellBestPractice);
            chat.AppendUserInput(UserMessage.ToString());
            string response = await chat.GetResponseFromChatbotAsync();

            xmlResponse = response;

            if (xmlResponse.rating >= condition.AIRating) return true;
            return false;

        }
    }
}