using Engine;
using Engine.Analyze.Conditions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PowerShellProtect.Analyze.Conditions
{
    internal class AssemblyCondition : ListCondition
    {
        public override string Name => "Assembly";

        public override string Description => string.Empty;

        public override List<string> GetValue(ScriptContext context)
        {
            try
            {
                var modules = new List<string>();
                foreach (ProcessModule item in Process.GetCurrentProcess().Modules)
                {
                    modules.Add(item.FileName);
                }
                return modules;
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
            }

            return new List<string>();
        }
    }
}
