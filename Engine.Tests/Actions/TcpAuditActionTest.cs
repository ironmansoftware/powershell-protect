using Engine.Actions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Engine.Tests.Actions
{
    public class TcpAuditActionTest
    {
        private string result;

        [Fact]
        public void ShouldSendTcpMessage()
        {
            var server = new TcpListener(IPAddress.Loopback, 0);
            server.Start();
            var port = ((IPEndPoint)server.LocalEndpoint).Port;

            var task = Task.Run(() =>
            {
                var client = server.AcceptTcpClient();

                NetworkStream stream = client.GetStream();

                int i;
                Byte[] bytes = new Byte[256];
                String data = null;

                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                    result = data;
                }

                client.Close();
            });

            var action = new TcpAuditAction();
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
                        Name = "HostName",
                        Value = "localhost"
                    },
                    new Configuration.Setting
                    {
                        Name = "Port",
                        Value = port.ToString()
                    },
                    new Configuration.Setting
                    {
                        Name = "Format",
                        Value = "{rule} {applicationname}"
                    }
                }
            });

            Thread.Sleep(100);

            server.Stop();

            Assert.Equal("Hello World\r\n", result);
        }
    }
}
