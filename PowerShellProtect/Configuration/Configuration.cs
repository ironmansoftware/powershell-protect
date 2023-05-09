using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace Engine.Configuration
{
    
    public class Configuration
    {
        public List<Rule> Rules { get; set; } = new List<Rule>();
        public List<Action> Actions { get; set; } = new List<Action>();
        public BuiltIn BuiltIn { get; set; } = new BuiltIn();
    }

    public class OpenAIConfiguration {
        public string chatRolePowerShellSecurity        = "You are a code analyzer. You are analyzing a PowerShell script for security vulnerabilities.";
        public string chatRolePowerShellBestPractice    = "You are a code analyzer. You are analyzing a PowerShell script for best practices.";
        public string chatMessagePowerShellSecurity     = "Analyze the following PowerShell Script for security vulnerabilities. Simplify the response as a boolean called 'result', provide a rating between 0 and 1 and format the response as XML.";
        public string chatMessagePowerShellBestPractice = "Analyze the following PowerShell Script for best practices. Simplify the response as a boolean called 'result', provide a rating between 0 and 1 and format the response as XML.";
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
        public string AITemperature { get; set; }
        public string AIRating { get; set; }
        public SecureString APIKey { get; set; }
        public bool ContinueOnError { get; set; }
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
