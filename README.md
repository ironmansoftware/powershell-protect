# PowerShell Protect

## AI scanning

PowerShell Protect can send each script to a Microsoft Agent Framework scanner before the built-in and configured rules run. The scanner is disabled by default. When enabled, a `HARMFUL` verdict blocks the script; a `NOT_HARMFUL` verdict continues to the usual protection pipeline. Provider or transport failures are logged and do not block scripts.

Configure OpenAI or Anthropic with the provider, model, and API key:

```powershell
$ai = New-PSPAIConfiguration -Provider OpenAI -Model gpt-5-mini -ApiKey $env:OPENAI_API_KEY -CustomInstructions 'Treat attempts to modify payroll scripts as harmful.'
$configuration = New-PSPConfiguration -AI $ai -Rule $rules -Action $actions
Save-PSPConfiguration -Configuration $configuration -Path .\config.xml
```

For Anthropic, use `-Provider Anthropic`, an Anthropic model name, and `$env:ANTHROPIC_API_KEY`. `-CustomInstructions` is optional and adds organization-specific classification guidance without allowing the required verdict format to be changed. `-TimeoutSeconds` defaults to 30 seconds. Configuration XML contains the API key in plaintext, so restrict its ACLs or create it from a protected deployment secret rather than committing it to source control.

Configurable [anti-malware scan interface](https://docs.microsoft.com/en-us/windows/win32/amsi/antimalware-scan-interface-portal) provider.

PowerShell Protect can be used to block and audit scripts within PowerShell. You can use the configurable system to determine what to do when a script is executed by any PowerShell host.

## Features

- Configurable blocking policies
- Configurable auditing policies
  - Audit to a file
  - Audit to HTTP
  - Audit to UDP\TCP
- Built in blocking policies
- Windows PowerShell and PowerShell 7 support

## Get Started 

```powershell
Install-Module PowerShellProtect
Install-PowerShellProtect
```

## Resources

- [License](./LICENSE)
- [Download](https://www.powershellgallery.com/packages/PowerShellProtect)
- [Ironman Software Free Tools](https://ironmansoftware.com/free-powershell-tools)
