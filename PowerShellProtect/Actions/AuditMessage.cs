using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class AuditMessage
    {
        public DateTime Timestamp => DateTime.UtcNow;
        public string Script { get; set; }
        public string ContentPath { get; set; }
        public string ApplicationName { get; set; }
        public string UserName { get; set; }
        public string ComputerName { get; set; }
        public bool Administrator { get; set; }
        public string DomainName { get; set; }
        public string Rule { get; set; }

        public override string ToString()
        {
            return ApplicationName;
        }
    }
}
