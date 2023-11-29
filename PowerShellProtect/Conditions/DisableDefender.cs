using Engine;
using Engine.Configuration;
using System.Linq;

namespace PowerShellProtect.Analyze.Conditions
{
    internal class DisableDefender : ICondition
    {
        public string Name => "DisableDefender";

        public string Description => "There was an attempt made to disable Windows Defender.";

        public bool Analyze(ScriptContext context, Condition condition)
        {
            if (context?.Script == null) return false;
            if (context?.Commands == null) return false;
            return context.Commands.Any(m => m.Equals("Set-MpPreference", System.StringComparison.OrdinalIgnoreCase)) && context.Script.ToLower().Contains("DisableRea".ToLower());
        }
    }
}
