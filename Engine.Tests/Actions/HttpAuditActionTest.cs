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

    public class HttpAuditActionTest
    {
        private string result;

        [Fact]
        public void ShouldSendMessage()
        {
            var server = new TcpListener(IPAddress.Loopback, 0);
            server.Start();
            var port = ((IPEndPoint)server.LocalEndpoint).Port;
            server.Stop();

            var listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{port}/");
            listener.Start();

            var task = Task.Run(() =>
            {
                var context = listener.GetContext();

                using (var sr = new StreamReader(context.Request.InputStream))
                {
                    result = sr.ReadToEnd();
                }

                context.Response.OutputStream.Write(new byte[0], 0, 0);
                context.Response.OutputStream.Close();

                listener.Stop();
            });

            var action = new HttpAuditAction();
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
                        Value = $"http://localhost:{port}"
                    },
                    new Configuration.Setting
                    {
                        Name = "Format",
                        Value = "{rule} {applicationname}"
                    }
                }
            });

            Thread.Sleep(1000);

            server.Stop();

            Assert.Equal("Hello World", result);
        }
    }
}
