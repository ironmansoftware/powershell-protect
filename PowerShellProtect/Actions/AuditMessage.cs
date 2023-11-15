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
            if (!String.IsNullOrEmpty(Script)) other.Script = String.Copy(Script);
            if (!String.IsNullOrEmpty(ContentPath)) other.ContentPath = String.Copy(ContentPath);
            if (!String.IsNullOrEmpty(ApplicationName)) other.ApplicationName = String.Copy(ApplicationName);
            if (!String.IsNullOrEmpty(UserName)) other.UserName = String.Copy(UserName);
            if (!String.IsNullOrEmpty(ComputerName)) other.ComputerName = String.Copy(ComputerName);
            if (!String.IsNullOrEmpty(DomainName)) other.DomainName = String.Copy(DomainName);
            if (!String.IsNullOrEmpty(Rule)) other.Rule = String.Copy(Rule);
            return other;
        }
    }
}
