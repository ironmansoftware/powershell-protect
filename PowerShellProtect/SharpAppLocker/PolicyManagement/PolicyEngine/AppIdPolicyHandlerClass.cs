using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Security.ApplicationId.PolicyManagement.PolicyEngine
{
  [Guid("F1ED7D4C-F863-4DE6-A1CA-7253EFDEE1F3")]
  [ClassInterface(ClassInterfaceType.None)]
  [TypeLibType(TypeLibTypeFlags.FCanCreate)]
  [ComImport]
  public class AppIdPolicyHandlerClass : IAppIdPolicyHandler, AppIdPolicyHandler
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern AppIdPolicyHandlerClass();

    [DispId(1)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SetPolicy([MarshalAs(UnmanagedType.BStr), In] string bstrLdapPath, [MarshalAs(UnmanagedType.BStr), In] string bstrXmlPolicy);

    [DispId(2)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.BStr)]
    public virtual extern string GetPolicy([MarshalAs(UnmanagedType.BStr), In] string bstrLdapPath);

    [DispId(3)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.BStr)]
    public virtual extern string GetEffectivePolicy();

    [DispId(4)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int IsFileAllowed(
      [MarshalAs(UnmanagedType.BStr), In] string bstrXmlPolicy,
      [MarshalAs(UnmanagedType.BStr), In] string bstrFilePath,
      [MarshalAs(UnmanagedType.BStr), In] string bstrUserSid,
      out Guid pguidResponsibleRuleId);

    [DispId(5)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int IsPackageAllowed(
      [MarshalAs(UnmanagedType.BStr), In] string bstrXmlPolicy,
      [MarshalAs(UnmanagedType.BStr), In] string bstrPublisherName,
      [MarshalAs(UnmanagedType.BStr), In] string bstrPackageName,
      [In] ulong ullPackageVersion,
      [MarshalAs(UnmanagedType.BStr), In] string bstrUserSid,
      out Guid pguidResponsibleRuleId);
  }
}
