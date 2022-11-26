using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Engine.Tests
{
    public class ConfigTests
    {
        [Fact()]
        public void ShouldLoadConfig()
        {
            var p = new XmlConfigProvider("%ProgramData%\\PowerShellProtect\\config.xml");
            p.GetConfiguration();
        }
    }
}
