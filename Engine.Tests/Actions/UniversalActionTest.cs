using Engine.Actions;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Engine.Tests.Actions
{

    public class UniversalAuditActionTest
    {
        private string result;

        [Fact(Skip = "Wont work")]
        public void ShouldSendMessage()
        {
            var action = new UniversalAuditAction();
            action.Audit(new AuditMessage
            {
                Rule = "Hello",
                ApplicationName = "World"
            }, new Configuration.Action
            {
                Settings = new List<Configuration.Setting>
                {
                    new Configuration.Setting
                    {
                        Name = "address",
                        Value = $"http://localhost:5000"
                    },
                    new Configuration.Setting
                    {
                        Name = "apptoken",
                        Value = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9oYXNoIjoiYTc0ZjU4OTQtNjI5MS00Mzk1LWJhYmQtOWUzYWY0MzVjNzU2Iiwic3ViIjoiUG93ZXJTaGVsbFVuaXZlcnNhbCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluaXN0cmF0b3IiLCJuYmYiOjE2NTQwMTc4MjMsImV4cCI6MTY1NjYwOTc4MCwiaXNzIjoiSXJvbm1hblNvZnR3YXJlIiwiYXVkIjoiUG93ZXJTaGVsbFVuaXZlcnNhbCJ9.YVFQh47m0AuBuD2SSbWm7AR1aXFR3iusoAnJdXayaUo"
                    }
                }
            });
        }
    }
}
