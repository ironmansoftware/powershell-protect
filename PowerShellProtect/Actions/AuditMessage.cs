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

        public AuditMessage DeepCopy()
        {
            AuditMessage other = (AuditMessage)this.MemberwiseClone();
            other.Script = String.Copy(Script);
            other.ContentPath = String.Copy(ContentPath);
            other.ApplicationName = String.Copy(ApplicationName);
            other.UserName = String.Copy(UserName);
            other.ComputerName = String.Copy(ComputerName);
            other.DomainName = String.Copy(DomainName);
            other.Rule = String.Copy(Rule);
            return other;
        }
    }
}
