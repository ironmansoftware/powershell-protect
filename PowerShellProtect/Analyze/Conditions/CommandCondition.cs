using System;
using System.Collections.Generic;

namespace Engine.Analyze.Conditions
{
    internal class CommandCondition : ListCondition
    {
        public override string Name => "command";

        public override string Description => throw new NotImplementedException();

        public override List<string> GetValue(ScriptContext context)
        {
            if (context?.Commands == null) return new List<string>();
            return context.Commands;
        }
    }
}
