namespace Engine.Analyze.Conditions
{
    /// <summary>
    /// Matches the language mode of the PowerShell runspace that AMSI is scanning.
    /// </summary>
    internal class LanguageModeCondition : StringCondition
    {
        public override string Name => "languagemode";

        public override string Description => "Matches the language mode of the PowerShell runspace that submitted the script.";

        public override string GetValue(ScriptContext context)
        {
            return context.LanguageMode ?? string.Empty;
        }
    }
}
