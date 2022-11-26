using Newtonsoft.Json;
using System;
using System.Net;

namespace Engine.Actions
{
    internal class UniversalAuditAction : IAction
    {
        public string Type => "universal";

        public AnalyzeResult Audit(AuditMessage message, Configuration.Action setting)
        {
            var address = setting.GetSetting("address");
            var apptoken = setting.GetSetting("apptoken");

            var builder = new UriBuilder(address);
            builder.Path = "/api/v1/protect";

            var webClient = new WebClient();

            webClient.Headers.Add("Authorization", $"Bearer {apptoken}");
            webClient.Headers.Add("Content-Type", "application/json");

            webClient.UploadStringCompleted += (sender, args) =>
            {
                if (args.Error != null)
                {
                    Log.LogError(args.Error.ToString());
                }
            };

            var json = JsonConvert.SerializeObject(message);
            webClient.UploadString(builder.Uri.ToString(), "POST", json);

            return AnalyzeResult.Ok;
        }
    }
}
