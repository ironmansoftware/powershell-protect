using PowerShellProtect.Analyze.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Engine.Tests.Analyze.Conditions
{
    public class AssemblyLoadTests
    {
        [Theory]
        [InlineData("[Reflection.Assembly]::Load()")]
        [InlineData("[System.Reflection.Assembly]::Load()")]
        public void ShouldDetectAssemblyLoads(string script)
        {
            Assert.True(new AssemblyLoad().Analyze(new ScriptContext
            {
                Script = script
            }, null));
        }
    }
}
