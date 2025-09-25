using System;
using System.Diagnostics;

namespace Engine.Audit
{
    internal class EventLogAuditAction : IAction
    {
        public string Type => "eventlog";

        public AnalyzeResult Audit(AuditMessage message, Configuration.Action setting)
        {
            int id = 0;

            var entryType = setting.GetSetting("EntryType");
            var format = setting.GetSetting("Format");
            int maxSize = int.Parse(setting.GetSetting("MaxSize"));

            if (!Enum.TryParse(entryType, out EventLogEntryType type))
            {
                type = EventLogEntryType.Information;
            }

            // determine the event ID according to the type
            switch (type)
            {
                case EventLogEntryType.Information:
                    id = 101;
                    break;
                case EventLogEntryType.Warning:
                    id = 103;
                    break;
                case EventLogEntryType.Error:
                    id = 102;
                    break;
            }

            // Cut off the script buffer if it is too big
            AuditMessage copiedMessage = message.DeepCopy();
            if (maxSize > 0 && copiedMessage.Script.Length > maxSize) {
                copiedMessage.Script = copiedMessage.Script.Substring(0, maxSize);
            }

            var output = Formatter.Format(copiedMessage, format);
            Log.LogMessage(output, type, id);

            return AnalyzeResult.Ok;
        }
    }
}
