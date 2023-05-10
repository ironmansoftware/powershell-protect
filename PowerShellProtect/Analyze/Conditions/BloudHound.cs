using Engine;
using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PowerShellProtect.Analyze.Conditions
{
    internal class BloudHound : ICondition
    {
        public string Name => "BloudHound";

        public string Description => "There was an attempt to run the BloudHound injestor. Learn more about the injestor: https://github.com/BloodHoundAD/SharpHound3";

        public bool Analyze(ScriptContext context, Condition condition)
        {
            var script = context.Script.ToLower();

            return script.Contains("assemblyloader") && script.Contains("invokesharphound");
        }
    }
}
