using System;
using System.IO;

namespace Engine.Audit
{
    internal class FileAuditAction : IAction
    {
        public string Type => "file";

        public AnalyzeResult Audit(AuditMessage message, Configuration.Action setting)
        {
            var path = setting.GetSetting("Path");
            var format = setting.GetSetting("Format");

            path = Environment.ExpandEnvironmentVariables(path);

            var output = Formatter.Format(message, format);

            File.AppendAllText(path, output + Environment.NewLine);

            return AnalyzeResult.Ok;
        }
    }
}
