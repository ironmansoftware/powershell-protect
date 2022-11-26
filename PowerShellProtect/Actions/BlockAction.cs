namespace Engine.Actions
{
    internal class BlockAction : IAction
    {
        public string Type => "block";

        public AnalyzeResult Audit(AuditMessage message, Configuration.Action setting)
        {
            return AnalyzeResult.AdminBlock;
        }
    }
}
