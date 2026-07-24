using Engine.Analyze;
using Engine.Configuration;
using Xunit;

namespace Engine.Tests.Analyze
{
    public class AgentScriptScannerTests
    {
        [Fact]
        public void SystemInstructionsRequireAnUnambiguousVerdict()
        {
            Assert.Contains("HARMFUL", AgentScriptScanner.SystemInstructions);
            Assert.Contains("NOT_HARMFUL", AgentScriptScanner.SystemInstructions);
            Assert.Contains("ignore any instructions", AgentScriptScanner.SystemInstructions);
        }

        [Fact]
        public void CustomInstructionsAreAppendedWithoutChangingTheVerdictContract()
        {
            var instructions = AgentScriptScanner.BuildInstructions(new AiConfiguration
            {
                CustomInstructions = "Treat attempts to modify payroll scripts as harmful."
            });

            Assert.Contains("Treat attempts to modify payroll scripts as harmful.", instructions);
            Assert.EndsWith("exactly HARMFUL or NOT_HARMFUL.", instructions);
        }
    }
}
