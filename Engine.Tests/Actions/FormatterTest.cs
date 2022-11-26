using Engine.Audit;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Engine.Tests.Actions
{
    public class FormatterTest
    {
        private AuditMessage auditMessage; 

        public FormatterTest()
        {
            auditMessage = new AuditMessage
            {
                Administrator = true,
                ApplicationName = "app",
                ComputerName = "comp",
                ContentPath = "ps1",
                DomainName = "domain",
                Rule = "rule",
                Script = "script"
            };
        }

        [Theory()]
        [InlineData("Administrator", "True")]
        [InlineData("ApplicationName", "app")]
        [InlineData("computername", "comp")]
        [InlineData("Rule", "rule")]
        [InlineData("UserName", "")]
        public void ShouldFormatProperties(string property, string expectedValue)
        {
            Assert.Equal(expectedValue, Formatter.Format(auditMessage, $"{{{property}}}"));
        }
    }
}
