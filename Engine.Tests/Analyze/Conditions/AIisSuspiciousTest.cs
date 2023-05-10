using Engine;
using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using OpenAI_API;
using System.Xml.Serialization;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace PowerShellProtect.Analyze.Conditions.Tests
{
    public class AIisSuspiciousConditionTests
    {
        [Fact]
        public void AnalyzeAsync_ReturnsTrue_WhenOpenAIResponseRatingIsGreaterThanOrEqualToConditionAIRating()
        {
            // Arrange
            var context = new ScriptContext();
            var condition = new Condition
            {
                APIKey = "your-api-key",
                AIRating = 0.5m
            };
            var aiIsSuspiciousCondition = new AIisSuspiciousCondition();

            // Act
            var result = aiIsSuspiciousCondition.Analyze(context, condition);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AnalyzeAsync_ReturnsFalse_WhenOpenAIResponseIsNull()
        {
            // Arrange
            var context = new ScriptContext();
            var condition = new Condition
            {
                APIKey = "your-api-key",
                AIRating = 0.5m
            };
            var aiIsSuspiciousCondition = new AIisSuspiciousCondition();

            // Act
            var result = aiIsSuspiciousCondition.Analyze(context, condition);

            // Assert
            Assert.False(result);
        }
    }
}
