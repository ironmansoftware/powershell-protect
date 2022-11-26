using System;
using System.Diagnostics;

namespace Engine
{
    public static class Log
    {
        public static Action<string> Logger;

        public static void LogMessage(string message, EventLogEntryType type, int id)
        {
            if (Logger != null)
            {
                Logger(message);
            }

            try
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "PowerShellProtect";
                    eventLog.WriteEntry(message, type, id, 1);
                }
            }
            catch { }
        }

        public static void LogError(string message, int id)
        {
            LogMessage(message, EventLogEntryType.Error, id);
        }

        public static void LogError(string message)
        {
            LogMessage(message, EventLogEntryType.Error, 101);
        }

        public static void LogInformation(string message)
        {
            LogMessage(message, EventLogEntryType.Information, 102);
        }
    }
}
