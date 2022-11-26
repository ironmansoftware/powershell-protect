using Engine.Configuration;

namespace Engine
{
    internal interface ICondition
    {
        bool Analyze(ScriptContext context, Condition condition);

        string Name { get; }
        string Description { get; }
    }
}
