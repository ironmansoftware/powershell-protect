using Engine;
using Engine.Configuration;
using System;
using System.Linq;

namespace PowerShellProtect.Analyze.Conditions
{
    internal class InvokeExpression : ICondition
    {
        public string Name => "InvokeExpression";

        public string Description => "Invoke-Expression is often used to execute malicious payloads downloaded from the internet.";

        public bool AnalyzeAsync(ScriptContext context, Condition condition)
        {
            if (context?.Commands == null) return false;

            var commands = new[] { "invoke-expression", "iex" };

            if (context.Commands.Any(m => commands.Any(x => x.ToLower().Contains("invoke-expression") || x.ToLower().Equals("iex"))))
            {
                return true;
            }

            if (context.Commands.Any(m => m.Equals("Set-Alias", StringComparison.OrdinalIgnoreCase) && context.Script.ToLower().Contains("invoke-expression")))
            {
                return true;
            }

            return false;
        }
    }
}
