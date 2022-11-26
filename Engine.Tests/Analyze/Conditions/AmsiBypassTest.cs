using PowerShellProtect.Analyze.Conditions;
using System.Management.Automation.Runspaces;
using Xunit;

namespace Engine.Tests.Analyze.Conditions
{
    public class AmsiBypassTest
    {
        [Fact]
        public void ShouldDetectAmsiBypass()
        {
            Runspace.DefaultRunspace = RunspaceFactory.CreateRunspace();
            Runspace.DefaultRunspace.Open();

            var amsiBypass = new AmsiBypass();
            Assert.True(amsiBypass.Analyze(new ScriptContext
            {
                Script = Resource1.AmsiBypass
            }, null));
        }

        [Fact]
        public void ShouldNotThrowNull()
        {
            Runspace.DefaultRunspace = RunspaceFactory.CreateRunspace();
            Runspace.DefaultRunspace.Open();

            var amsiBypass = new AmsiBypass();
            Assert.False(amsiBypass.Analyze(new ScriptContext
            {
                Script = "Invoke-Mimikatz"
            }, null));
        }
    }
}
