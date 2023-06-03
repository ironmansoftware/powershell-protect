﻿using System.Collections.Generic;
using System.Linq;
using System.Security;
using PowerShellProtect.Analyze;

namespace Engine.Configuration
{
    
    public class Configuration
    {
        public List<Rule> Rules { get; set; } = new List<Rule>();
        public List<Action> Actions { get; set; } = new List<Action>();
        public BuiltIn BuiltIn { get; set; } = new BuiltIn();
    }

    public class OpenAIConfiguration {
        public static readonly string chatRolePowerShellSecurity        = "You are a code analyzer. You are analyzing a PowerShell script for security vulnerabilities. All responses are in the JSON format.";
        public static readonly string chatRolePowerShellBestPractice    = "You are a code analyzer. You are analyzing a PowerShell script for best practices. All responses are in the JSON format.";
        public static readonly string chatRolePowerShellObfuscate       = "You are a code analyzer. You are analyzing a PowerShell script for obfuscation. All responses are in the JSON format.";

        public static readonly string chatMessagePowerShellSecurity     = "Just the json, no explanation or usage. Analyze the following PowerShell Script for security risks. Simplify your response as a Boolean called 'result' and include a rating (scoring from bad to good) between 0 and 1.";
        public static readonly string chatMessagePowerShellBestPractice = "Just the json, no explanation or usage. Analyze the following PowerShell Script for best practices. Simplify your response as a Boolean called 'result' and include a rating (scoring from bad to good) between 0 and 1.";

        public static readonly string chatMessagePowerShellObfuscation = "Just the json, no explanation or usage. Analyze the following PowerShell Script for a level of obfuscation. Simplify your response as a Boolean called 'result' and include a rating (scoring from bad to good) between 0 and 1.";

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
        public aISettings AISettings { get; set; }

    }

    public class aISettings
    {
        public AIResultCondition resultCondition { get; set; }
        public double Temperature { get; set; }
        public decimal Rating { get; set; }
        public string APIKey { get; set; }
        public bool ContinueOnError { get; set; }
        public string Model { get; set; }
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
