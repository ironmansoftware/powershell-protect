using Engine.Analyze.Conditions;
using Xunit;

namespace Engine.Tests.Analyze.Conditions
{
    public class LanguageModeTests
    {
        [Fact]
        public void ShouldMatchConstrainedLanguageMode()
        {
            var condition = new LanguageModeCondition();

            var result = condition.Analyze(new ScriptContext { LanguageMode = "ConstrainedLanguage" }, new Configuration.Condition
            {
                Property = "languagemode",
                Operator = "equals",
                Value = "ConstrainedLanguage"
            });

            Assert.True(result);
        }

        [Fact]
        public void ShouldMatchAFullLanguageViolation()
        {
            var condition = new LanguageModeCondition();

            var result = condition.Analyze(new ScriptContext { LanguageMode = "FullLanguage" }, new Configuration.Condition
            {
                Property = "languagemode",
                Operator = "notequals",
                Value = "ConstrainedLanguage"
            });

            Assert.True(result);
        }
    }
}
