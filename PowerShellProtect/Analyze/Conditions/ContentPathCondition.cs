using System;

namespace Engine.Analyze.Conditions
{
    internal class ContentPathCondition : StringCondition
    {
        public override string Name => "contentpath";

        public override string Description => throw new NotImplementedException();

        public override string GetValue(ScriptContext context)
        {
            return context.ContentName;
        }
    }
}
