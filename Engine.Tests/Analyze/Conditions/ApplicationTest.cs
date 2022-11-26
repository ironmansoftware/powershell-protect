using Engine.Analyze.Conditions;
using Xunit;

namespace Engine.Tests.Analyze.Conditions
{
    public class ApplicationTest
    {
        [Fact]
        public void ShouldReturnAppName()
        {
            var appName = new ApplicationCondition();
            var result = appName.Analyze(new ScriptContext { ApplicationName = "appName" }, new Configuration.Condition
            {
                Property = "ApplicationName",
                Operator = "equals",
                Value = "appName"
            });

            Assert.True(result);
        }
    }
}
