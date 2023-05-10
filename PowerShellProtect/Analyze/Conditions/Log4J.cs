using Engine;
using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PowerShellProtect.Analyze.Conditions
{
    internal class Log4J : ICondition
    {
        public string Name => "Log4J";

        public string Description => "A string containing the CVE-2021-44228 Log4j exploit was found within the script.";

        public bool Analyze(ScriptContext context, Condition condition)
        {
            if (context?.Script == null) return false;

            var script = context.Script.ToLower();

            return script.Contains("${jndi");
        }
    }
}
