using Engine;
using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PowerShellProtect.Analyze.Conditions
{
    internal class PersistentWmi : ICondition
    {
        public string Name => "PersistentWmi";

        public string Description => "There was an attempt to create a persistent WMI event subscription.";

        public bool Analyze(ScriptContext context, Condition condition)
        {
            var script = context.Script.ToLower();

            return script.Contains("set-wmiinstance") && script.Contains(@"root\subscription") && script.Contains("commandlineeventconsumer");
        }
    }
}
