using PowerShellProtect.Analyze.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Engine.Tests.Analyze.Conditions
{
    public class MarshalClassTest
    {
        [Fact]
        public void ShouldDetect()
        {
            Assert.True(new MarshalClass().Analyze(new ScriptContext
            {
                Script = "[System.Runtime.InteropServices.Marshal]::SizeOf()"
            }, null));
        }
    }
}
