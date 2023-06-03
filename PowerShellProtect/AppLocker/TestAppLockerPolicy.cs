using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Security.ApplicationId.PolicyManagement;
using Microsoft.Security.ApplicationId.PolicyManagement.PolicyEngine;
using Microsoft.Security.ApplicationId.PolicyManagement.PolicyModel;

namespace PowerShellProtect.AppLocker
{
    public class TestAppLockerPolicy
    {

        public static AppLockerPolicyDecision ProcessFile(string filePath) {

            IAppIdPolicyHandler appIdPolicyHandler = (IAppIdPolicyHandler) new AppIdPolicyHandlerClass();

            SecurityIdentifier m_userSid = WindowsIdentity.GetCurrent().User;
            string m_xmlPolicyFilePath = appIdPolicyHandler.GetEffectivePolicy();

            AppLockerPolicy m_policy = AppLockerPolicy.Load(m_xmlPolicyFilePath);

            try {
                FileManager.VerifyFileTypeSupported(filePath);
                AppLockerPolicyDecision sendToPipeline = PolicyManager.IsFileAllowed(m_policy, filePath, m_userSid);
                return sendToPipeline;               
            } catch {
                throw new Exception("Error processing file");
            }

        }
    }
}