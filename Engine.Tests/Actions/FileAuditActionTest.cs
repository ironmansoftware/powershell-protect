using Engine.Audit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Engine.Tests.Actions
{
    public class FileAuditActionTest
    {
        [Fact]
        public void ShouldWriteToFile()
        {
            var tempFile = Path.GetTempFileName();

            var action = new FileAuditAction();
            action.Audit(new AuditMessage
            {
                Rule = "Hello",
                ApplicationName = "World"
            }, new Configuration.Action {
                Settings = new List<Configuration.Setting>
                {
                    new Configuration.Setting
                    {
                        Name = "Path",
                        Value = tempFile
                    },
                    new Configuration.Setting
                    {
                        Name = "Format",
                        Value = "{rule} {applicationname}"
                    }
                }
            });

            var text = File.ReadAllText(tempFile);

            Assert.Equal("Hello World\r\n", text);

            File.Delete(tempFile);
        }
    }
}
