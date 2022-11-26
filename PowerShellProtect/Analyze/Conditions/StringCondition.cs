using Engine.Configuration;
using System;
using System.Text.RegularExpressions;

namespace Engine.Analyze.Conditions
{
    internal abstract class StringCondition : ICondition
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract string GetValue(ScriptContext context);

        public bool Analyze(ScriptContext context, Condition condition)
        {
            var value = GetValue(context);

            switch(condition.Operator.ToLower())
            {
                case "startswith":
                    return value.StartsWith(condition.Value, StringComparison.OrdinalIgnoreCase);
                case "notstartswith":
                    return !value.StartsWith(condition.Value, StringComparison.OrdinalIgnoreCase);
                case "endswith":
                    return value.EndsWith(condition.Value, StringComparison.OrdinalIgnoreCase);
                case "notendswith":
                    return !value.EndsWith(condition.Value, StringComparison.OrdinalIgnoreCase);
                case "equals":
                    return value.Equals(condition.Value, StringComparison.OrdinalIgnoreCase);
                case "notequals":
                    return !value.Equals(condition.Value, StringComparison.OrdinalIgnoreCase);
                case "contains":
                    return value.ToLower().Contains(condition.Value.ToLower());
                case "notcontains":
                    return !value.ToLower().Contains(condition.Value.ToLower());
                case "matches":
                    return new Regex(condition.Value).IsMatch(value);
            }

            throw new Exception("Unknown operator: " + condition.Operator);
        }
    }
}
