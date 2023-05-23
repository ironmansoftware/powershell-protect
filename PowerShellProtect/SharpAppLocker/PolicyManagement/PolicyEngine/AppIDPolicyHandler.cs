using System.Runtime.InteropServices;

namespace Microsoft.Security.ApplicationId.PolicyManagement.PolicyEngine
{
  [CoClass(typeof (AppIdPolicyHandlerClass))]
  [Guid("B6FEA19E-32DD-4367-B5B7-2F5DA140E87D")]
  [ComImport]
  public interface AppIdPolicyHandler : IAppIdPolicyHandler
  {
  }
}
