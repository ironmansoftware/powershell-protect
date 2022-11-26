using System;

namespace Engine.Analyze.Conditions
{
    internal class DomainCondition : StringCondition
    {
        public override string Name => "domain";

        public override string Description => throw new NotImplementedException();

        public override string GetValue(ScriptContext context)
        {
            return Environment.UserDomainName;
        }
    }
}
