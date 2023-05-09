using Engine;
using Engine.Configuration;
using System;
using System.Linq;

namespace PowerShellProtect.Analyze.Conditions
{
    internal class LoggingBypass : ICondition
    {
        public string Name => "loggingBypass";

        public string Description => "A logging bypass disables PowerShell module and script block logging. Learn more: https://blog.ironmansoftware.com/protect-logging-bypass/";

        public bool AnalyzeAsync(ScriptContext context, Condition condition)
        {
            if (context?.Script == null) return false;
            if (context?.Commands == null) return false;
            if (context?.Members == null) return false;
            if (context?.Strings == null) return false;

            // Module logging bypass
            if (context.Script.ToLower().Contains("cmdletinfo]::new(")) return true;
            if (context.Commands.Any(m => m.Equals("New-Object", StringComparison.OrdinalIgnoreCase)) && context.Script.ToLower().Contains("cmdletinfo")) return true;
            if (context.Script.ToLower().Contains("LogPipelineExecutionDetails".ToLower())) return true;

            // Script logging bypass
            if (context.Members.Any(m => m.Equals("GetProperty", StringComparison.OrdinalIgnoreCase)) &&
                context.Members.Any(m => m.Equals("SetValue", StringComparison.OrdinalIgnoreCase)) &&
                context.Strings.Any(m => m.Equals("HasLogged", StringComparison.OrdinalIgnoreCase))) return true;

            return false;
        }
    }
}
