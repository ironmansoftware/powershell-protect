using PowerShellProtect.Analyze.Conditions;
using Xunit;

namespace Engine.Tests.Analyze.Conditions
{
    public class ReflectionEmitTests
    {
        [Theory]
        [InlineData("$TypeBuilder.DefineField('Signature', [UInt32], 'Public') | Out-Null")]
        [InlineData(" $TypeBuilder = $ModuleBuilder.DefineType('IMAGE_OPTIONAL_HEADER32', $Attributes, [System.ValueType], 224)")]
        [InlineData(" $ModuleBuilder = $AssemblyBuilder.DefineDynamicModule('DynamicModule', $false)")]
        [InlineData("$AssemblyBuilder = $Domain.DefineDynamicAssembly($DynamicAssembly, [System.Reflection.Emit.AssemblyBuilderAccess]::Run)")]
        public void ShouldDetect(string script)
        {
            Assert.True(new ReflectionEmit().Analyze(new ScriptContext
            {
                Script = script
            }, null));
        }
    }
}
