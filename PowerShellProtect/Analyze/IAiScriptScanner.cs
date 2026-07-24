using Engine.Configuration;

namespace Engine.Analyze
{
    internal interface IAiScriptScanner
    {
        AiScanResult Scan(ScriptContext scriptContext, AiConfiguration configuration);
    }

    internal enum AiScanResult
    {
        NotHarmful,
        Harmful
    }
}
