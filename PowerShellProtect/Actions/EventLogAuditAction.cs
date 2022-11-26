using System;
using System.Diagnostics;

namespace Engine.Audit
{
    internal class EventLogAuditAction : IAction
    {
        public string Type => "eventlog";

        public AnalyzeResult Audit(AuditMessage message, Configuration.Action setting)
        {
            var entryType = setting.GetSetting("EntryType");
            var format = setting.GetSetting("Format");

            if (!Enum.TryParse(entryType, out EventLogEntryType type))
            {
                type = EventLogEntryType.Information;
            }

            var output = Formatter.Format(message, format);

            Log.LogMessage(output, type, 0);

            return AnalyzeResult.Ok;
        }
    }
}
