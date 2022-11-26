using Engine;
using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PowerShellProtect.Analyze.Conditions
{
    internal class Kerberoasting : ICondition
    {
        public string Name => "Kerberoasting";

        public string Description => "There was an attempt to run a Kerberoasting script. Learn more about Kerberoasting: https://github.com/EmpireProject/Empire/blob/master/data/module_source/credentials/Invoke-Kerberoast.ps1";

        public bool Analyze(ScriptContext context, Condition condition)
        {
            var script = context.Script.ToLower();

            return script.Contains("lastlogon") && script.Contains("lastlogontimestamp") && script.Contains("pwdlastset") && script.Contains("lastlogoff") && script.Contains("badpasswordtime") && script.Contains("invokemember") && script.Contains("reflection");
        }
    }
}
