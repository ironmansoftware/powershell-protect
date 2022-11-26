using System.Collections.Generic;
using System.Linq;

namespace Engine.Configuration
{
    public class Config
    {
        private IEnumerable<IConfigProvider> _configProviders;

        public Config()
        {            
            var programData = new XmlConfigProvider(@"%ProgramData%\PowerShellProtect\config.xml");

            _configProviders = new List<IConfigProvider> {
                programData,
                new RegistryConfigProvider(),
                new RegistryFileConfigProvider()
            };
        }

        public Config(IEnumerable<IConfigProvider> providers)
        {
            _configProviders = providers;
        }

        internal void SetProvider(IConfigProvider configProvider)
        {
            _configProviders = new[] { configProvider };
        }

        public Configuration GetConfiguration()
        {
            foreach(var provider in _configProviders.OrderBy(m => m.Precendence))
            {
                var config = provider.GetConfiguration();
                if (config != null) return config;
            }

            throw new System.Exception("Failed to load configuration");
        }
    }
}
