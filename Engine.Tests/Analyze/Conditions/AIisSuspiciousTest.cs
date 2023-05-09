using Engine;
using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using OpenAI_API;
using System.Xml.Serialization;
using System.IO;
using Xunit;

namespace PowerShellProtect.Analyze.Conditions.Tests
{
    public class AIObfuscationConditionTests
    {
        [Fact]
        public void Analyze_ReturnsTrue_WhenOpenAIResponseRatingIsGreaterThanOrEqualToConditionAIRating()
        {
            // Arrange
            var context = new ScriptContext();
            var condition = new Condition
            {
                APIKey = "test",
                AIRating = 0.5m,
                ContinueOnError = false
            };
            var aiResponse = new OpenAIResponse { rating = 0.6m };
            var chatbotResponse = Serialize(aiResponse);

            var apiMock = new Mock<OpenAI_API.OpenAI_API>("test");
            var chatMock = new Mock<Chat>();
            chatMock.Setup(c => c.GetResponseFromChatbotAsync()).ReturnsAsync(chatbotResponse);
            apiMock.Setup(a => a.Chat.CreateConverstation()).Returns(chatMock.Object);

            var sut = new AIObfuscationCondition { Api = apiMock.Object };

            // Act
            var result = sut.Analyze(context, condition);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Analyze_ReturnsFalse_WhenOpenAIResponseRatingIsLessThanConditionAIRating()
        {
            // Arrange
            var context = new ScriptContext();
            var condition = new Condition
            {
                APIKey = "test",
                AIRating = 0.5m,
                ContinueOnError = false
            };
            var aiResponse = new OpenAIResponse { rating = 0.4m };
            var chatbotResponse = Serialize(aiResponse);

            var apiMock = new Mock<OpenAI_API.OpenAI_API>("test");
            var chatMock = new Mock<Chat>();
            chatMock.Setup(c => c.GetResponseFromChatbotAsync()).ReturnsAsync(chatbotResponse);
            apiMock.Setup(a => a.Chat.CreateConverstation()).Returns(chatMock.Object);

            var sut = new AIObfuscationCondition { Api = apiMock.Object };

            // Act
            var result = sut.Analyze(context, condition);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Analyze_ReturnsFalse_WhenApiThrowsExceptionAndConditionContinueOnErrorIsFalse()
        {
            // Arrange
            var context = new ScriptContext();
            var condition = new Condition
            {
                APIKey = "test",
                AIRating = 0.5m,
                ContinueOnError = false
            };
            var exception = new Exception("Test exception");

            var apiMock = new Mock<OpenAI_API.OpenAI_API>("test");
            var chatMock = new Mock<Chat>();
            chatMock.Setup(c => c.GetResponseFromChatbotAsync()).ThrowsAsync(exception);
            apiMock.Setup(a => a.Chat.CreateConverstation()).Returns(chatMock.Object);

            var sut = new AIObfuscationCondition { Api = apiMock.Object };

            // Act
            var result = sut.Analyze(context, condition);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Analyze_ReturnsTrue_WhenApiThrowsExceptionAndConditionContinueOnErrorIsTrue()
        {
            // Arrange
            var context = new ScriptContext();
            var condition = new Condition
            {
                APIKey = "test",
                AIRating = 0.5m,
                ContinueOnError = true
            };
            var exception = new Exception("Test exception");

            var apiMock = new Mock<OpenAI_API.OpenAI_API>("test");
            var chatMock = new Mock<Chat>();
            chatMock.Setup(c => c.GetResponseFromChatbotAsync()).ThrowsAsync(exception);
            apiMock.Setup(a => a.Chat.CreateConverstation()).Returns(chatMock.Object);

            var sut = new AIObfuscationCondition { Api = apiMock.Object };

            // Act
            var result = sut.Analyze(context, condition);

            // Assert
            Assert.True(result);
        }

        private string Serialize(OpenAIResponse response)
        {
            var serializer = new XmlSerializer(typeof(OpenAIResponse));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, response);
                return writer.ToString();
            }
        }
    }
}
