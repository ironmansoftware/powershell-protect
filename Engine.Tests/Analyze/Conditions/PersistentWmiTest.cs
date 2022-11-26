using PowerShellProtect.Analyze.Conditions;
using Xunit;

namespace Engine.Tests.Analyze.Conditions
{
    public class PersistentWmiTest
    {
        [Fact]
        public void ShouldDetect()
        {
            Assert.True(new PersistentWmi().Analyze(new ScriptContext
            {
                Script = Resource1.PersistentWmi
            }, null));
        }
    }
}
