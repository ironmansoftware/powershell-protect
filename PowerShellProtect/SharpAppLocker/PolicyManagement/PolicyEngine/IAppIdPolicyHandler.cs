using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Security.ApplicationId.PolicyManagement.PolicyEngine
{
  [Guid("B6FEA19E-32DD-4367-B5B7-2F5DA140E87D")]
  [TypeLibType(TypeLibTypeFlags.FDual | TypeLibTypeFlags.FNonExtensible | TypeLibTypeFlags.FDispatchable)]
  [ComImport]
  public interface IAppIdPolicyHandler
  {
    [DispId(1)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetPolicy([MarshalAs(UnmanagedType.BStr), In] string bstrLdapPath, [MarshalAs(UnmanagedType.BStr), In] string bstrXmlPolicy);

    [DispId(2)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.BStr)]
    string GetPolicy([MarshalAs(UnmanagedType.BStr), In] string bstrLdapPath);

    [DispId(3)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.BStr)]
    string GetEffectivePolicy();

    [DispId(4)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int IsFileAllowed(
      [MarshalAs(UnmanagedType.BStr), In] string bstrXmlPolicy,
      [MarshalAs(UnmanagedType.BStr), In] string bstrFilePath,
      [MarshalAs(UnmanagedType.BStr), In] string bstrUserSid,
      out Guid pguidResponsibleRuleId);

    [DispId(5)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int IsPackageAllowed(
      [MarshalAs(UnmanagedType.BStr), In] string bstrXmlPolicy,
      [MarshalAs(UnmanagedType.BStr), In] string bstrPublisherName,
      [MarshalAs(UnmanagedType.BStr), In] string bstrPackageName,
      [In] ulong ullPackageVersion,
      [MarshalAs(UnmanagedType.BStr), In] string bstrUserSid,
      out Guid pguidResponsibleRuleId);
  }
}
