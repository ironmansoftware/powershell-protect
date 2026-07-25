using System.Collections.Generic;
using System.Linq;
using Engine.Configuration;
using PowerShellProtect.Analyze.Conditions;

namespace Engine
{
    internal static class BuiltInConditionCatalog
    {
        internal static IEnumerable<ICondition> Create()
        {
            return new ICondition[]
            {
                new AmsiBypass(),
                new LoggingBypass(),
                new DisableDefender(),
                new PowerSploit(),
                new AssemblyLoad(),
                new ReflectionEmit(),
                new MarshalClass(),
                new PersistentWmi(),
                new BloudHound(),
                new Kerberoasting(),
                new InvokeExpression(),
                new Log4J()
            };
        }

        internal static IEnumerable<BuiltInRule> GetRules()
        {
            return Create().Select(condition => new BuiltInRule
            {
                Name = condition.Name,
                Description = condition.Description
            });
        }
    }
}
