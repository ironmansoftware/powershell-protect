namespace Engine.Analyze.Conditions
{
    internal class ApplicationCondition : StringCondition
    {
        public override string Name => "ApplicationName";

        public override string Description => string.Empty;

        public override string GetValue(ScriptContext context)
        {
            return context.ApplicationName;
        }
    }
}
