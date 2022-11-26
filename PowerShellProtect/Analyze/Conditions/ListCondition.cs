using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Engine.Analyze.Conditions
{
    internal abstract class ListCondition : ICondition
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract List<string> GetValue(ScriptContext context);

        public bool Analyze(ScriptContext context, Condition condition)
        {
            var value = GetValue(context);

            switch (condition.Operator.ToLower())
            {
                case "startswith":
                    return value.Any(x => x.StartsWith(condition.Value, StringComparison.OrdinalIgnoreCase));
                case "notstartswith":
                    return !value.Any(x => x.StartsWith(condition.Value, StringComparison.OrdinalIgnoreCase));
                case "endswith":
                    return value.Any(x => x.EndsWith(condition.Value, StringComparison.OrdinalIgnoreCase));
                case "notendswith":
                    return !value.Any(x => x.EndsWith(condition.Value, StringComparison.OrdinalIgnoreCase));
                case "equals":
                    return value.Any(x => x.Equals(condition.Value, StringComparison.OrdinalIgnoreCase));
                case "notequals":
                    return !value.Any(x => x.Equals(condition.Value, StringComparison.OrdinalIgnoreCase));
                case "contains":
                    return value.Any(x => x.ToLower().Contains(condition.Value.ToLower()));
                case "notcontains":
                    return !value.Any(x => x.ToLower().Contains(condition.Value.ToLower()));
                case "matches":
                    return value.Any(x => new Regex(condition.Value).IsMatch(x));
            }

            throw new Exception("Unknown operator: " + condition.Operator);
        }
    }
}
