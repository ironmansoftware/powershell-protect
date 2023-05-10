using Engine.Configuration;

namespace Engine.Analyze.Conditions
{
    internal abstract class BoolCondition : ICondition
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract bool GetValue(ScriptContext context);


        public bool Analyze(ScriptContext context, Condition condition)
        {
            var value = GetValue(context);

            var cValue = bool.Parse(condition.Value);

            return value == cValue;
        }
    }
}
