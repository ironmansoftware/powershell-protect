using Engine;
using System.Linq;
using Xunit;

namespace Engine.Tests.Analyze
{
    public class BuiltInConditionCatalogTests
    {
        [Fact]
        public void ShouldEnumerateAllBuiltInRules()
        {
            var rules = BuiltInConditionCatalog.GetRules().ToList();

            Assert.Equal(12, rules.Count);
            Assert.Contains(rules, rule => rule.Name == "amsiBypass");
            Assert.All(rules, rule => Assert.False(string.IsNullOrWhiteSpace(rule.Description)));
        }
    }
}
