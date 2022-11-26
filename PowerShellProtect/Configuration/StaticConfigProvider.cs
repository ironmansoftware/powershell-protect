using Engine.Configuration;

namespace PowerShellProtect
{
    public class StaticConfigProvider : IConfigProvider
    {
        public int Precendence => -1;

        private readonly Engine.Configuration.Configuration configuration;

        public StaticConfigProvider(Engine.Configuration.Configuration configuration)
        {
            this.configuration = configuration;
        }

        public Engine.Configuration.Configuration GetConfiguration()
        {
            return configuration;
        }
    }
}
