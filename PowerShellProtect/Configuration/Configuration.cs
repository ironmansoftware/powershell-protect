using System.Collections.Generic;
using System.Linq;

namespace Engine.Configuration
{
    public class Configuration
    {
        public List<Rule> Rules { get; set; } = new List<Rule>();
        public List<Action> Actions { get; set; } = new List<Action>();
        public BuiltIn BuiltIn { get; set; } = new BuiltIn();
    }

    public class BuiltIn
    {
        public bool Enabled { get; set; }
        public List<ActionRef> Actions { get; set; } = new List<ActionRef>();
        public string[] DisabledConditions { get; set; } = new string[0];
    }

    public class Rule
    {
        public string Name { get; set; }
        public bool AnyCondition { get; set; }
        public List<Condition> Conditions { get; set; } = new List<Condition>();
        public List<ActionRef> Actions { get; set; } = new List<ActionRef>();
    }

    public class Condition
    {
        public string Property { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }

    public class AICondition
    {
        public string Property { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }

    public class ActionRef
    {
        public string Name { get; set; }
    }

    public class Action
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<Setting> Settings { get; set; } = new List<Setting>();

        public string GetSetting(string name)
        {
            return Settings.FirstOrDefault(m => m.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase))?.Value;
        }
    }

    public class Setting
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

}
