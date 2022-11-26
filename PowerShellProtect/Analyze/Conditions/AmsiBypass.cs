using Engine;
using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerShellProtect.Analyze.Conditions
{
    internal class AmsiBypass : ICondition
    {
        public string Name => "amsiBypass";

        public string Description => "An AMSI bypass is used to circumvent malware scanning for scripts run in PowerShell. Learn more: https://blog.ironmansoftware.com/protect-amsi-bypass/";

        public bool Analyze(ScriptContext context, Condition condition)
        {
            if (context?.Strings == null) return false;
            if (
                context.Strings.Any(m => m.Equals("amsi.dll", StringComparison.OrdinalIgnoreCase))
            ) return true;

            return false;
        }
    }
}
