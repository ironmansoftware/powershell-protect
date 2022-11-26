using Engine.Analyze.Conditions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Engine.Tests.Analyze.Conditions
{
    public class CommandConditionTest
    {
        [Fact]
        public void ShouldReturnTrue()
        {
            var condition = new CommandCondition();

            var result = condition.Analyze(new ScriptContext
            {
                Script = "Invoke-WebRequest"
            }, new Configuration.Condition { 
                Property = "Command",
                Operator = "equals",
                Value = "Invoke-WebRequest"
            });

            Assert.True(result);
        }
    }
}
