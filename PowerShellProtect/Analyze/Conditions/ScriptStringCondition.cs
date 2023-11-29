using System;
using System.Collections.Generic;

namespace Engine.Analyze.Conditions
{
    internal class ScriptStringCondition : ListCondition
    {
        public override string Name => "string";

        public override string Description => throw new NotImplementedException();

        public override List<string> GetValue(ScriptContext context)
        {
            if (context?.Strings == null) return new List<string>();
            return context.Strings;
        }
    }
}
