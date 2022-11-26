using Engine.Audit;
using System.Net;

namespace Engine.Actions
{
    internal class HttpAuditAction : IAction
    {
        public string Type => "http";

        public AnalyzeResult Audit(AuditMessage message, Configuration.Action setting)
        {
            var address = setting.GetSetting("address");
            var format = setting.GetSetting("format");
            var headers = setting.GetSetting("headers");

            var webClient = new WebClient();

            if (headers != null)
            {
                foreach(var header in headers.Split(';'))
                {
                    var kvp = header.Split('=');
                    webClient.Headers.Add(kvp[0], kvp[1]);
                }
            }

            webClient.UploadStringCompleted += (sender, args) => {
                if (args.Error != null)
                {
                    Log.LogError(args.Error.ToString());
                }
            };
            webClient.UploadStringAsync(new System.Uri(address), Formatter.Format(message, format));

            return AnalyzeResult.Ok;
        }
    }
}
