namespace Engine
{
    internal interface IAction
    {
        string Type { get; }
        AnalyzeResult Audit(AuditMessage message, Configuration.Action setting);
    }
}
