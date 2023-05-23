namespace Microsoft.Security.ApplicationId.PolicyManagement
{
  public class AppLockerPolicyDecision
  {
    private string m_filePath = string.Empty;
    private PolicyDecision m_policyDecision;
    private string m_matchingRule = string.Empty;

    internal AppLockerPolicyDecision(string filePath, bool allowed, string matchingRule)
    {
      this.m_filePath = filePath;
      this.m_matchingRule = matchingRule;
      if (allowed)
      {
        if (string.IsNullOrEmpty(matchingRule))
          this.m_policyDecision = PolicyDecision.AllowedByDefault;
        else
          this.m_policyDecision = PolicyDecision.Allowed;
      }
      else if (string.IsNullOrEmpty(matchingRule))
        this.m_policyDecision = PolicyDecision.DeniedByDefault;
      else
        this.m_policyDecision = PolicyDecision.Denied;
    }

    public string FilePath => this.m_filePath;

    public PolicyDecision PolicyDecision => this.m_policyDecision;

    public string MatchingRule => this.m_matchingRule;
  }
}
