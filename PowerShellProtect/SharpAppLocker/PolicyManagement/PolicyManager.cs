
using Microsoft.Security.ApplicationId.PolicyManagement.PolicyEngine;
using Microsoft.Security.ApplicationId.PolicyManagement.PolicyModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Microsoft.Security.ApplicationId.PolicyManagement
{
  public static class PolicyManager
  {
    private static readonly string LdapPrefix = "ldap";

    public static void SetPolicy(AppLockerPolicy policy, string ldapPath)
    {
      if (string.IsNullOrEmpty(ldapPath))
        PolicyManager.SetLocalPolicy(policy);
      else
        PolicyManager.SetDomainPolicy(policy, ldapPath);
    }

    public static AppLockerPolicy GetPolicy(string ldapPath) => !string.IsNullOrEmpty(ldapPath) ? PolicyManager.GetDomainPolicy(ldapPath) : PolicyManager.GetLocalPolicy();

    public static void SetLocalPolicy(AppLockerPolicy policy)
    {
      if (policy == null)
        throw new ArgumentNullException(nameof (policy));
      IAppIdPolicyHandler appIdPolicyHandler = (IAppIdPolicyHandler) new AppIdPolicyHandlerClass();
      string xml = policy.ToXml();
      try
      {
        appIdPolicyHandler.SetPolicy(string.Empty, xml);
      }
      catch (COMException ex)
      {
        throw new SetLocalPolicyException((Exception) ex);
      }
    }

    public static void SetDomainPolicy(AppLockerPolicy policy, string ldapPath)
    {
      if (policy == null)
        throw new ArgumentNullException(nameof (policy));
      if (ldapPath == null)
        throw new ArgumentNullException(nameof (ldapPath));
      IAppIdPolicyHandler appIdPolicyHandler = (IAppIdPolicyHandler) new AppIdPolicyHandlerClass();
      string str = PolicyManager.FormatLdapPath(ldapPath);
      string xml = policy.ToXml();
      try
      {
        appIdPolicyHandler.SetPolicy(str, xml);
      }
      catch (COMException ex)
      {
        throw new SetDomainPolicyException(str, (Exception) ex);
      }
    }

    public static AppLockerPolicy GetLocalPolicy()
    {
      IAppIdPolicyHandler appIdPolicyHandler = (IAppIdPolicyHandler) new AppIdPolicyHandlerClass();
      string policy;
      try
      {
        policy = appIdPolicyHandler.GetPolicy(string.Empty);
      }
      catch (COMException ex)
      {
        throw new GetLocalPolicyException((Exception) ex);
      }
      return AppLockerPolicy.FromXml(policy);
    }

    public static AppLockerPolicy GetDomainPolicy(string ldapPath)
    {
      if (ldapPath == null)
        throw new ArgumentNullException(nameof (ldapPath));
      IAppIdPolicyHandler appIdPolicyHandler = (IAppIdPolicyHandler) new AppIdPolicyHandlerClass();
      string str = PolicyManager.FormatLdapPath(ldapPath);
      string policy;
      try
      {
        policy = appIdPolicyHandler.GetPolicy(str);
      }
      catch (COMException ex)
      {
        throw new GetDomainPolicyException(str, (Exception) ex);
      }
      return AppLockerPolicy.FromXml(policy);
    }

    public static AppLockerPolicy GetEffectivePolicy()
    {
      IAppIdPolicyHandler appIdPolicyHandler = (IAppIdPolicyHandler) new AppIdPolicyHandlerClass();
      string effectivePolicy;
      try
      {
        effectivePolicy = appIdPolicyHandler.GetEffectivePolicy();
      }
      catch (COMException ex)
      {
        throw new GetEffectivePolicyException((Exception) ex);
      }
      return AppLockerPolicy.FromXml(effectivePolicy);
    }

    public static AppLockerPolicy MergePolicies(ICollection<AppLockerPolicy> policies)
    {
      if (policies == null)
        throw new ArgumentNullException(nameof (policies));
      AppLockerPolicy appLockerPolicy = new AppLockerPolicy();
      foreach (AppLockerPolicy policy in (IEnumerable<AppLockerPolicy>) policies)
        appLockerPolicy.Merge(policy);
      return appLockerPolicy;
    }

    public static AppLockerPolicyDecision IsFileAllowed(
      AppLockerPolicy policy,
      string filePath,
      SecurityIdentifier userSid)
    {
      if (policy == null)
        throw new ArgumentNullException(nameof (policy));
      if (filePath == null)
        throw new ArgumentNullException(nameof (filePath));
      if (userSid == (SecurityIdentifier) null)
        throw new ArgumentNullException(nameof (userSid));
      IAppIdPolicyHandler appIdPolicyHandler = (IAppIdPolicyHandler) new AppIdPolicyHandlerClass();
      Guid pguidResponsibleRuleId;
      bool allowed;
      try
      {
        allowed = appIdPolicyHandler.IsFileAllowed(policy.ToXml(), filePath, userSid.Value, out pguidResponsibleRuleId) != 0;
      }
      catch (COMException ex)
      {
        throw new TestFileAllowedException(filePath, (Exception) ex);
      }
      Id ruleId = new Id(pguidResponsibleRuleId);
      string ruleName = PolicyManager.GetRuleName(policy, ruleId);
      return new AppLockerPolicyDecision(filePath, allowed, ruleName);
    }

    public static AppLockerPolicyDecision IsPackageAllowed(
      AppLockerPolicy policy,
      FileInformation fileInformation,
      SecurityIdentifier userSid)
    {
      if (policy == null)
        throw new ArgumentNullException(nameof (policy));
      if (fileInformation == (FileInformation) null)
        throw new ArgumentNullException(nameof (fileInformation));
      if (userSid == (SecurityIdentifier) null)
        throw new ArgumentNullException(nameof (userSid));
      IAppIdPolicyHandler appIdPolicyHandler = (IAppIdPolicyHandler) new AppIdPolicyHandlerClass();
      Guid pguidResponsibleRuleId;
      bool allowed;
      try
      {
        allowed = appIdPolicyHandler.IsPackageAllowed(policy.ToXml(), fileInformation.Publisher.PublisherName, fileInformation.Publisher.ProductName, fileInformation.Publisher.BinaryVersion.VersionNumber, userSid.Value, out pguidResponsibleRuleId) != 0;
      }
      catch (COMException ex)
      {
        throw new TestFileAllowedException(fileInformation.Path.Path, (Exception) ex);
      }
      Id ruleId = new Id(pguidResponsibleRuleId);
      string ruleName = PolicyManager.GetRuleName(policy, ruleId);
      return new AppLockerPolicyDecision(fileInformation.Path.Path, allowed, ruleName);
    }

    private static string GetRuleName(AppLockerPolicy policy, Id ruleId)
    {
      string ruleName = string.Empty;
      foreach (RuleCollection ruleCollection in (IEnumerable<RuleCollection>) policy.RuleCollections)
      {
        if (ruleCollection.Has(ruleId))
        {
          ruleName = ruleCollection.Get(ruleId).Name;
          break;
        }
      }
      return ruleName;
    }

    public static AppLockerPolicy GeneratePolicy(
      RuleGenerationSettings settings,
      ICollection<FileInformation> files)
    {
      ICollection<FileInformation> ignoredFiles = (ICollection<FileInformation>) null;
      return PolicyManager.GeneratePolicy(settings, files, ServiceEnforcementMode.NotConfigured, SystemAppAllowMode.NotEnabled, out ignoredFiles);
    }

    public static AppLockerPolicy GeneratePolicy(
      RuleGenerationSettings settings,
      ICollection<FileInformation> files,
      ServiceEnforcementMode serviceEnforcementMode,
      SystemAppAllowMode allowWindowsMode,
      out ICollection<FileInformation> ignoredFiles)
    {
      if (settings == null)
        throw new ArgumentNullException(nameof (settings));
      Dictionary<AppLockerFileType, List<FileInformation>> dictionary = files != null ? PolicyManager.SortFiles(files) : throw new ArgumentNullException(nameof (files));
      AppLockerPolicy policyToUpdate = new AppLockerPolicy();
      List<FileInformation> ignoredFilesToUpdate = new List<FileInformation>();
      foreach (AppLockerFileType key in dictionary.Keys)
        PolicyManager.CreateRulesForSpecificFiles(key, (ICollection<FileInformation>) dictionary[key], settings, policyToUpdate, serviceEnforcementMode, allowWindowsMode, ignoredFilesToUpdate);
      if (files.Count == 0)
      {
        foreach (AppLockerFileType fileType in new List<AppLockerFileType>()
        {
          AppLockerFileType.Exe,
          AppLockerFileType.Dll
        })
          PolicyManager.CreateEmptyCollection(fileType, policyToUpdate, serviceEnforcementMode, allowWindowsMode);
      }
      ignoredFiles = (ICollection<FileInformation>) ignoredFilesToUpdate;
      return policyToUpdate;
    }

    private static RuleCollection CreateEmptyCollection(
      AppLockerFileType fileType,
      AppLockerPolicy policyToUpdate,
      ServiceEnforcementMode serviceEnforcementMode,
      SystemAppAllowMode allowWindowsMode)
    {
      string fileRuleCollection = FileManager.GetFileRuleCollection(fileType);
      RuleCollection ruleCollection = policyToUpdate.CreateRuleCollection(fileRuleCollection);
      if (fileType == AppLockerFileType.Exe || fileType == AppLockerFileType.Dll)
      {
        ruleCollection.ServiceEnforcementMode = serviceEnforcementMode;
        ruleCollection.SystemAppAllowMode = allowWindowsMode;
      }
      return ruleCollection;
    }

    private static void CreateRulesForSpecificFiles(
      AppLockerFileType fileType,
      ICollection<FileInformation> files,
      RuleGenerationSettings settings,
      AppLockerPolicy policyToUpdate,
      ServiceEnforcementMode serviceEnforcementMode,
      SystemAppAllowMode allowWindowsMode,
      List<FileInformation> ignoredFilesToUpdate)
    {
      RuleGenerationResult rules = RuleManager.GenerateRules(settings, files);
      ignoredFilesToUpdate.AddRange((IEnumerable<FileInformation>) rules.IgnoredFiles);
      if (rules.Rules.Count == 0)
        return;
      RuleCollection emptyCollection = PolicyManager.CreateEmptyCollection(fileType, policyToUpdate, serviceEnforcementMode, allowWindowsMode);
      foreach (AppLockerRule rule in (IEnumerable<AppLockerRule>) rules.Rules)
        emptyCollection.Add(rule);
    }

    private static Dictionary<AppLockerFileType, List<FileInformation>> SortFiles(
      ICollection<FileInformation> files)
    {
      Dictionary<AppLockerFileType, List<FileInformation>> dictionary = new Dictionary<AppLockerFileType, List<FileInformation>>();
      foreach (FileInformation file in (IEnumerable<FileInformation>) files)
      {
        AppLockerFileType fileType = FileManager.GetFileType(file);
        List<FileInformation> fileInformationList;
        if (dictionary.ContainsKey(fileType))
        {
          fileInformationList = dictionary[fileType];
        }
        else
        {
          fileInformationList = new List<FileInformation>();
          dictionary.Add(fileType, fileInformationList);
        }
        fileInformationList.Add(file);
      }
      return dictionary;
    }

    private static string FormatLdapPath(string ldapPath)
    {
      string empty = string.Empty;
      return !ldapPath.StartsWith(PolicyManager.LdapPrefix, StringComparison.OrdinalIgnoreCase) ? ldapPath : PolicyManager.LdapPrefix.ToUpper(CultureInfo.InvariantCulture) + ldapPath.Substring(PolicyManager.LdapPrefix.Length);
    }
  }
}
