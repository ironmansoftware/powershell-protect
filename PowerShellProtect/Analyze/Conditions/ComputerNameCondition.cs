using Engine.Configuration;
using System;

namespace Engine.Analyze.Conditions
{
    internal class ComputerNameCondition : StringCondition
    {
        public override string Name => "computername";

        public override string Description => throw new NotImplementedException();

        public override string GetValue(ScriptContext context)
        {
            return Environment.MachineName;
        }
    }
}
