﻿using System;

namespace Engine.Analyze.Conditions
{
    internal class ScriptCondition : StringCondition
    {
        public override string Name => "script";

        public override string Description => throw new NotImplementedException();

        public override string GetValue(ScriptContext context)
        {
            if (context?.Script == null) return "";
            return context.Script;
        }
    }
}
