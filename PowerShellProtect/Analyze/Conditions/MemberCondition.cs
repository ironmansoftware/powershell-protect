using System;
using System.Collections.Generic;

namespace Engine.Analyze.Conditions
{
    internal class MemberCondition : ListCondition
    {
        public override string Name => "member";

        public override string Description => throw new NotImplementedException();

        public override List<string> GetValue(ScriptContext context)
        {
            if (context?.Members == null) return new List<string>();
            return context.Members;
        }
    }
}
