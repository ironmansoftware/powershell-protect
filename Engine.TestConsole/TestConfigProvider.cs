using Engine.Configuration;

namespace Engine.TestConsole
{
    public class TestConfigProvider : IConfigProvider
    {
        private Configuration.Configuration _config;
        public TestConfigProvider(Configuration.Configuration config)
        {
            config = _config;
        }

        public int Precendence => 0;

        public Configuration.Configuration GetConfiguration()
        {
            return _config;
        }
    }
}
