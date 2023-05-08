using Engine;
using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PowerShellProtect.Analyze.Conditions
{
    public class AIObfuscationCondition : ICondition
    {
        public string Name => "AIisSuspiciousCondition";

        public string Description => "The AI considers it suspicious.";

        public bool Analyze(ScriptContext context, Condition condition)
        {

            string xmlResponse;

            var api = new OpenAI_API.OpenAI_API(condition.APIKey.ToInsecureString());

            string UserMessage = new StringBuilder()
                .AppendLine(Engine.Configuration.OpenAIConfiguration.chatMessagePowerShellSecurity)
                .AppendLine(context.Script);

            var chat = api.Chat.CreateConverstation();
            chat.AppendSystemMessage(Engine.Configuration.OpenAIConfiguration.chatRolePowerShellSecurity);
            chat.AppendUserInput(UserMessage.ToString());
            string response = await chat.GetResponseFromChatbotAsync();

            xmlResponse = response;

            if (xmlResponse.rating >= condition.AIRating) return true;
            return false;

        }

    }
}