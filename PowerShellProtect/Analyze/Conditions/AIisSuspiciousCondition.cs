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

          // Initialize variables
          OpenAIResponse aIResponse = null; // Response object from OpenAI
          var api = new OpenAI_API.OpenAIAPI(condition.APIKey); // API object to access OpenAI
          var serializer = new JsonSerializer(); // JSON serializer
          
          // Build user message to send to chatbot
          StringBuilder UserMessage = new StringBuilder()
              .AppendLine(Engine.Configuration.OpenAIConfiguration.chatMessagePowerShellSecurity)
              .AppendLine("The PowerShell Script:")
              .AppendLine(context.Script);
          
          // Set temperature and start conversation with chatbot
          api.Chat.DefaultChatRequestArgs.Temperature = condition.AITemperature;
          var chat = api.Chat.CreateConversation(api.Chat.DefaultChatRequestArgs);
          
          // Add system message and user input to chat log
          chat.AppendSystemMessage(Engine.Configuration.OpenAIConfiguration.chatRolePowerShellSecurity);
          chat.AppendUserInput(UserMessage.ToString());
          
          try
          {
              // Get response from chatbot and deserialize it into the response object
              string response = await chat.GetResponseFromChatbotAsync();

              // Validate the response to ensure that it's JSON
              aIResponse = JsonConvert.DeserializeObject<OpenAIResponse>(response);
          }
          catch(Exception ex)
          {
              // If there is an error and "ContinueOnError" is false, return true to indicate failure
              if (!condition.ContinueOnError) return true;
          }
          
          // If the response rating is greater than or equal to the required rating, return false to indicate success
          if (aIResponse != null && aIResponse.rating >= condition.AIRating) return false;
          
          // Otherwise, return true to indicate failure
          return true;  
                      
        }

    }
}