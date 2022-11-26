using PowerShellProtect.Analyze.Conditions;
using Xunit;

namespace Engine.Tests.Analyze.Conditions
{
    public class InvokeExpressionTest
    {
        [Fact]
        public void ShouldFlagWhenThereWasAnAttemptToInvokeExpression()
        {
            Assert.True(new InvokeExpression().Analyze(new ScriptContext
            {
                Script = "Invoke-Expression 'Get-Help'"
            }, null));
        }

        [Fact]
        public void ShouldFlagWhenFqdnCommandName()
        {
            Assert.True(new InvokeExpression().Analyze(new ScriptContext
            {
                Script = "Module\\Invoke-Expression 'Get-Help'"
            }, null));
        }

        [Fact]
        public void ShouldFlagWhenDefaultAlias()
        {
            Assert.True(new InvokeExpression().Analyze(new ScriptContext
            {
                Script = "iex 'Get-Help'"
            }, null));
        }


        [Fact]
        public void ShouldFlagWhenSetAlias()
        {
            Assert.True(new InvokeExpression().Analyze(new ScriptContext
            {
                Script = "Set-Alias Invoke-Expression"
            }, null));
        }
    }
}
