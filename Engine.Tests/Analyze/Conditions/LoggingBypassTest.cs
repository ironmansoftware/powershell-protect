using PowerShellProtect.Analyze.Conditions;
using Xunit;

namespace Engine.Tests.Analyze.Conditions
{
    public class LoggingBypassTest
    {
        [Fact]
        public void ShouldFindModuleLoggingBypassVariant1()
        {
            var condition = new LoggingBypass();
            Assert.True(condition.Analyze(new ScriptContext
            {
                Script = @"$module = Get-Module Microsoft.PowerShell.Utility
                $module.LogPipelineExecutionDetails = $false"
            }, null));
        }

        [Fact]
        public void ShouldFindModuleLoggingBypassVariant2()
        {
            var condition = new LoggingBypass();
            Assert.True(condition.Analyze(new ScriptContext
            {
                Script = @"$Cmd = [System.Management.Automation.CmdletInfo]::new('Write-Host', [Microsoft.PowerShell.Commands.WriteHostCommand])
                        & $Cmd 'abcd'"
            }, null));
        }

        [Fact]
        public void ShouldFindScriptBlockLoggingBypass()
        {
            var condition = new LoggingBypass();
            Assert.True(condition.Analyze(new ScriptContext
            {
                Script = @"$Script = { 'Log me' }
                            [ScriptBlock].getproperty('HasLogged',@('nonpublic','instance')).setvalue($script, $true)
                            $Script.Invoke()"
            }, null));
        }
    }
}
