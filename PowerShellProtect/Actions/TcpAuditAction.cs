using Engine.Audit;
using System.IO;
using System.Net.Sockets;

namespace Engine.Actions
{
    internal class TcpAuditAction : IAction
    {
        public string Type => "tcp";

        public AnalyzeResult Audit(AuditMessage message, Configuration.Action setting)
        {
            var hostname = setting.GetSetting("hostname");
            var port = setting.GetSetting("port");
            var format = setting.GetSetting("format");

            using (var tcpClient = new TcpClient(hostname, int.Parse(port)))
            {
                using (var streamWriter = new StreamWriter(tcpClient.GetStream()))
                {
                    streamWriter.WriteLine(Formatter.Format(message, format));
                }
            }

            return AnalyzeResult.Ok;
        }
    }
}
