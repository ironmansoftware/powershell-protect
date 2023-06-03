using System;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Configuration;
using Newtonsoft.Json;

namespace PowerShellProtect.Analyze
{
    public static class OpenAIHelper
    {

        public static async Task<bool> SendToOpenAI(
                ScriptContext context, 
                Condition condition, 
                string chatMessage, 
                string systemMessage)
            {

                // Initialize variables
                OpenAIResponse aIResponse = null; // Response object from OpenAI
                var api = new OpenAI_API.OpenAIAPI(condition.AISettings.APIKey); // API object to access OpenAI
                var serializer = new JsonSerializer(); // JSON serializer
                
                // Build user message to send to chatbot
                StringBuilder UserMessage = new StringBuilder()
                    .AppendLine(chatMessage)
                    .AppendLine("The PowerShell Script:")
                    .AppendLine(context.Script);
                
                // Set temperature and start conversation with chatbot
                api.Chat.DefaultChatRequestArgs.Temperature = condition.AISettings.Temperature;
                api.Chat.DefaultChatRequestArgs.Model = condition.AISettings.Model;
                var chat = api.Chat.CreateConversation(api.Chat.DefaultChatRequestArgs);
                
                // Add system message and user input to chat log
                chat.AppendSystemMessage(systemMessage);
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
                    if (!condition.AISettings.ContinueOnError) return true;
                }
                
                // Check if there is a response to evaluate
                if (aIResponse != null)
                {
                    // Evaluate the response based on the specified condition
                    switch (condition.AISettings.resultCondition)
                    {
                        case AIResultCondition.GreaterThan:
                            return aIResponse.rating > condition.AISettings.Rating;
                        case AIResultCondition.LessThan:
                            return aIResponse.rating < condition.AISettings.Rating;
                        case AIResultCondition.EqualTo:
                            return aIResponse.rating == condition.AISettings.Rating;
                        case AIResultCondition.GreaterThanOrEquals:
                            return aIResponse.rating >= condition.AISettings.Rating;
                        case AIResultCondition.LessThanOrEquals:
                            return aIResponse.rating <= condition.AISettings.Rating;
                        case AIResultCondition.NotEqualTo:
                            return aIResponse.rating != condition.AISettings.Rating;
                        default:
                            // If an invalid condition is specified, return true to indicate failure
                            return true;
                    }
                }
                else
                {
                    // If there is no response to evaluate, return false to indicate failure
                    return false;
                }
                        
            }

        }
}

