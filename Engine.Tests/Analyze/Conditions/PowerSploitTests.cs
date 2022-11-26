using PowerShellProtect.Analyze.Conditions;
using Xunit;

namespace Engine.Tests.Analyze.Conditions
{
    public class PowerSploitTests
    {
        [Fact]
        public void ShouldDetectInvokeMimiktaz()
        {
            Assert.True(new PowerSploit().Analyze(new ScriptContext
            {
                Script = "Invoke-Mimikatz"
            }, null));
        }
    }
}
