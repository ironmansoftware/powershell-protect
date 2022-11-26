using PowerShellProtect.Analyze.Conditions;
using Xunit;

namespace Engine.Tests.Analyze.Conditions
{
    public class DisableDefenderTest
    {
        [Fact]
        public void ShouldFlagWhenThereWasAnAttemptToDisableDefender()
        {
            Assert.True(new DisableDefender().Analyze(new ScriptContext { 
                Script = "Set-MpPreference -DisableRealtime $true"
            }, null));
        }

        [Fact]
        public void ShouldNotThrowException()
        {
            new DisableDefender().Analyze(new ScriptContext
            {
                Script = "& { Set-StrictMode -Version 1; $this.Exception.InnerException.PSMessageDetails }"
            }, null);
        }
    }
}
