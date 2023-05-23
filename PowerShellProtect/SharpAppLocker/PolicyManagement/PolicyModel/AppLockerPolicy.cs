
using Microsoft.Security.ApplicationId.PolicyManagement.Xml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Security.ApplicationId.PolicyManagement.PolicyModel
{
  public sealed class AppLockerPolicy : PolicyElement
  {
    private double m_version;
    private Dictionary<string, RuleCollection> m_ruleCollections = new Dictionary<string, RuleCollection>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private List<Plugin> m_plugins = new List<Plugin>();
    private static readonly double CurrentVersion = 1.0;
    private static readonly string SecurityDirectory = "Security";
    private static readonly string AppIdDirectory = "ApplicationId";
    private static readonly string PolicyManagementDirectory = "PolicyManagement";
    private static readonly string PolicySchemaFileName = "AppIdPolicy.xsd";

    public AppLockerPolicy() => this.Version = AppLockerPolicy.CurrentVersion;

    public AppLockerPolicy(double version) => this.Version = version;

    private AppLockerPolicy(AppLockerPolicy other)
    {
      this.Version = other.Version;
      foreach (KeyValuePair<string, RuleCollection> ruleCollection in other.m_ruleCollections)
        this.m_ruleCollections.Add(ruleCollection.Key, (RuleCollection) ruleCollection.Value.Clone());
    }

    protected override object CloneObject() => (object) new AppLockerPolicy(this);

    public double Version
    {
      get => this.m_version;
      set => this.m_version = value;
    }

    public ICollection<RuleCollection> RuleCollections => (ICollection<RuleCollection>) this.m_ruleCollections.Values;

    public ICollection<string> RuleCollectionTypes
    {
      get
      {
        List<string> ruleCollectionTypes = new List<string>();
        foreach (string key in this.m_ruleCollections.Keys)
          ruleCollectionTypes.Add(key);
        return (ICollection<string>) ruleCollectionTypes;
      }
    }

    private List<Plugin> Plugins
    {
      get => this.m_plugins;
      set => this.m_plugins = value;
    }

    public RuleCollection CreateRuleCollection(string type)
    {
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      RuleCollection ruleCollection = !this.HasRuleCollection(type) ? new RuleCollection(type) : throw new RuleCollectionAlreadyExistsException();
      this.m_ruleCollections.Add(ruleCollection.RuleCollectionType, ruleCollection);
      return ruleCollection;
    }

    public void DeleteRuleCollections() => this.m_ruleCollections.Clear();

    public void DeleteRuleCollection(string type)
    {
      if (!this.HasRuleCollection(type))
        throw new RuleCollectionDoesNotExistException();
      this.m_ruleCollections.Remove(type);
    }

    public RuleCollection GetRuleCollection(string type) => this.HasRuleCollection(type) ? this.FindRuleCollection(type) : throw new RuleCollectionDoesNotExistException();

    public bool HasRuleCollection(string type) => this.FindRuleCollection(type) != null;

    private RuleCollection FindRuleCollection(string type)
    {
      RuleCollection ruleCollection = (RuleCollection) null;
      if (this.m_ruleCollections.ContainsKey(type))
        ruleCollection = this.m_ruleCollections[type];
      return ruleCollection;
    }

    public void Merge(AppLockerPolicy newPolicy)
    {
      if (newPolicy == null)
        return;
      if (this.Version != newPolicy.Version)
        throw new PolicyMergingException(Resources.PolicyVersionsDoNotMatch);
      foreach (RuleCollection ruleCollection1 in (IEnumerable<RuleCollection>) newPolicy.RuleCollections)
      {
        string ruleCollectionType = ruleCollection1.RuleCollectionType;
        RuleCollection ruleCollection2;
        if (this.HasRuleCollection(ruleCollectionType))
        {
          ruleCollection2 = this.GetRuleCollection(ruleCollectionType);
          if (ruleCollection2.Count > 0)
          {
            if (ruleCollection1.ServiceEnforcementMode != ruleCollection2.ServiceEnforcementMode)
              ruleCollection2.ServiceEnforcementMode = ServiceEnforcementMode.Enabled;
            if (ruleCollection1.SystemAppAllowMode == SystemAppAllowMode.Enabled)
              ruleCollection2.SystemAppAllowMode = SystemAppAllowMode.Enabled;
          }
        }
        else
        {
          ruleCollection2 = this.CreateRuleCollection(ruleCollectionType);
          ruleCollection2.EnforcementMode = ruleCollection1.EnforcementMode;
          ruleCollection2.ServiceEnforcementMode = ruleCollection1.ServiceEnforcementMode;
        }
        foreach (AppLockerRule appLockerRule in ruleCollection1)
        {
          if (!ruleCollection2.Has(appLockerRule.Id))
            ruleCollection2.Add((AppLockerRule) appLockerRule.Clone());
        }
      }
    }

    public void Store(string xmlFilePath)
    {
      string xml = this.ToXml();
      Document document = new Document();
      document.LoadXml(xml);
      document.Save(xmlFilePath);
    }

    public static AppLockerPolicy Load(string xmlFilePath)
    {
      string end;
      using (StreamReader streamReader = File.OpenText(xmlFilePath))
        end = streamReader.ReadToEnd();
      AppLockerPolicy.ValidatePolicy(end);
      return AppLockerPolicy.FromXml(end);
    }

    public override string ToString() => this.ToXml();

    public string ToXml() => this.Serialize();

    public static AppLockerPolicy FromXml(string xml)
    {
      AppLockerPolicy.ValidatePolicy(xml);
      return XmlSerializer.Deserialize<AppLockerPolicy>(xml);
    }

    private static void ValidatePolicy(string xml)
    {
      string policySchemaFilePath = AppLockerPolicy.GetPolicySchemaFilePath();
      SchemaValidationResult validationResult = Document.Validate(xml, policySchemaFilePath, true);
      if (!validationResult.XmlValid)
        throw new InvalidXmlPolicyException(validationResult.ErrorMessage);
    }

    private static string GetPolicySchemaFilePath() => Environment.GetEnvironmentVariable("SystemRoot") + "\\" + AppLockerPolicy.SecurityDirectory + "\\" + AppLockerPolicy.AppIdDirectory + "\\" + AppLockerPolicy.PolicyManagementDirectory + "\\" + AppLockerPolicy.PolicySchemaFileName;

    internal override void Serialize(Microsoft.Security.ApplicationId.PolicyManagement.Xml.Node xmlNode)
    {
      xmlNode.CreateAttribute("Version", this.Version);
      foreach (KeyValuePair<string, RuleCollection> ruleCollection1 in this.m_ruleCollections)
      {
        RuleCollection ruleCollection2 = ruleCollection1.Value;
        Microsoft.Security.ApplicationId.PolicyManagement.Xml.Node child = xmlNode.CreateChild(ruleCollection2.GetXmlName());
        ruleCollection1.Value.Serialize(child);
      }
      if (this.Plugins.Count <= 0)
        return;
      Microsoft.Security.ApplicationId.PolicyManagement.Xml.Node child1 = xmlNode.CreateChild("PolicyExtensions").CreateChild("ThresholdExtensions").CreateChild("Plugins");
      foreach (XmlSerializer plugin in this.Plugins)
        plugin.Serialize(child1.CreateChild("Plugin"));
    }

    internal override void Deserialize(Microsoft.Security.ApplicationId.PolicyManagement.Xml.Node xmlNode)
    {
      this.Version = xmlNode.ParseDecimalNumberAttribute("Version");
      if (this.Version != AppLockerPolicy.CurrentVersion)
        throw new InvalidXmlPolicyException(Resources.UnsupportedPolicyVersion);
      foreach (Microsoft.Security.ApplicationId.PolicyManagement.Xml.Node specificChild in xmlNode.GetSpecificChilds("RuleCollection"))
        this.CreateRuleCollection(specificChild.ParseStringAttribute("Type")).Deserialize(specificChild);
      if (!xmlNode.HasSpecificChild("PolicyExtensions"))
        return;
      Microsoft.Security.ApplicationId.PolicyManagement.Xml.Node specificChild1 = xmlNode.GetSpecificChild("PolicyExtensions");
      if (!specificChild1.HasSpecificChild("ThresholdExtensions"))
        return;
      Microsoft.Security.ApplicationId.PolicyManagement.Xml.Node specificChild2 = specificChild1.GetSpecificChild("ThresholdExtensions");
      if (!specificChild2.HasSpecificChild("Plugins"))
        return;
      Microsoft.Security.ApplicationId.PolicyManagement.Xml.Node specificChild3 = specificChild2.GetSpecificChild("Plugins");
      List<Plugin> pluginList = new List<Plugin>();
      foreach (Microsoft.Security.ApplicationId.PolicyManagement.Xml.Node specificChild4 in specificChild3.GetSpecificChilds("Plugin"))
      {
        Plugin plugin = new Plugin();
        plugin.Deserialize(specificChild4);
        pluginList.Add(plugin);
      }
      this.Plugins = pluginList;
    }

    internal override string GetXmlName() => nameof (AppLockerPolicy);
  }
}
