using System;
using System.Management;

namespace Engine.Analyze.Conditions
{
    internal class DomainControllerCondition : BoolCondition
    {
        public override string Name => "domaincontroller";

        public override string Description => throw new NotImplementedException();

        private bool? isDc;

        public override bool GetValue(ScriptContext context)
        {
            if (!isDc.HasValue)
            {
                isDc = false;
                try
                {
                    ManagementScope wmiScope = new ManagementScope(@".rootcimv2");
                    wmiScope.Connect();
                    ManagementObjectSearcher moSearcher = new ManagementObjectSearcher(wmiScope, new ObjectQuery("SELECT DomainRole FROM Win32_ComputerSystem"));
                    foreach (ManagementObject shareData in moSearcher.Get())
                    {
                        int domainRole = int.Parse(shareData["DomainRole"].ToString());
                        if (domainRole >= 4)
                        {
                            isDc = true;
                        }
                        break;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return isDc.Value;
        }
    }
}
