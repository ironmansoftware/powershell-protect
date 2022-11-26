using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Analyze.Conditions
{
    internal class VariableCondition : ListCondition
    {
        public override string Name => "variable";

        public override string Description => throw new NotImplementedException();

        public override List<string> GetValue(ScriptContext context)
        {
            return context.Variables;
        }
    }
}
