using Engine.Audit;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Engine.Actions
{
    internal class UdpAuditAction : IAction
    {
        public string Type => "udp";

        public AnalyzeResult Audit(AuditMessage message, Configuration.Action setting)
        {
            var hostname = setting.GetSetting("hostname");
            var port = setting.GetSetting("port");
            var format = setting.GetSetting("format");

            using (var client = new UdpClient(hostname, int.Parse(port)))
            {
                var output = Formatter.Format(message, format);
                var bytes = Encoding.UTF8.GetBytes(output);
                client.Send(bytes, bytes.Length);
            }

            return AnalyzeResult.Ok;
        }
    }
}
