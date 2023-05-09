﻿using Engine;
using Engine.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerShellProtect.Analyze.Conditions
{
    internal class PowerSploit : ICondition
    {
        public string Name => "PowerSploit";

        public string Description => "There was an attempt to run a PowerSploit command. Learn more about PowerSploit: https://github.com/PowerShellMafia/PowerSploit";

        public bool AnalyzeAsync(ScriptContext context, Condition condition)
        {
            var commands = new[]
            {
                "Add-NetUser",
                "Add-ObjectAcl",
                "Add-Persistence",
                "Add-ServiceDacl",
                "Convert-NameToSid",
                "Convert-NT4toCanonical",
                "Convert-SidToName",
                "Copy-ClonedFile",
                "Find-AVSignature",
                "Find-ComputerField",
                "Find-ForeignGroup",
                "Find-ForeignUser",
                "Find-GPOComputerAdmin",
                "Find-GPOLocation",
                "Find-InterestingFile",
                "Find-LocalAdminAccess",
                "Find-PathDLLHijack",
                "Find-ProcessDLLHijack",
                "Find-ManagedSecurityGroups",
                "Find-UserField",
                "Get-ADObject",
                "Get-ApplicationHost",
                "Get-CachedRDPConnection",
                "Get-ComputerDetails",
                "Get-ComputerProperty",
                "Get-CurrentUserTokenGroupSid",
                "Get-DFSshare",
                "Get-DomainPolicy",
                "Get-ExploitableSystem",
                "Get-GPPPassword",
                "Get-HttpStatus",
                "Get-Keystrokes",
                "Get-LastLoggedOn",
                "Get-ModifiablePath",
                "Get-ModifiableRegistryAutoRun",
                "Get-ModifiableScheduledTaskFile",
                "Get-ModifiableService",
                "Get-ModifiableServiceFile",
                "Get-NetComputer",
                "Get-NetDomain",
                "Get-NetDomainController",
                "Get-NetDomainTrust",
                "Get-NetFileServer",
                "Get-NetForest",
                "Get-NetForestCatalog",
                "Get-NetForestDomain",
                "Get-NetForestTrust",
                "Get-NetGPO",
                "Get-NetGPOGroup",
                "Get-NetGroup",
                "Get-NetGroupMember",
                "Get-NetLocalGroup",
                "Get-NetLoggedon",
                "Get-NetOU",
                "Get-NetProcess",
                "Get-NetRDPSession",
                "Get-NetSession",
                "Get-NetShare",
                "Get-NetSite",
                "Get-NetSubnet",
                "Get-NetUser",
                "Get-ObjectAcl",
                "Get-PathAcl",
                "Get-Proxy",
                "Get-RegistryAlwaysInstallElevated",
                "Get-RegistryAutoLogon",
                "Get-SecurityPackages",
                "Get-ServiceDetail",
                "Get-SiteListPassword",
                "Get-System",
                "Get-TimedScreenshot",
                "Get-UnattendedInstallFile",
                "Get-UnquotedService",
                "Get-UserEvent",
                "Get-UserProperty",
                "Get-VaultCredential",
                "Get-VolumeShadowCopy",
                "Get-Webconfig",
                "Install-ServiceBinary",
                "Install-SSP",
                "Invoke-ACLScanner",
                "Invoke-CheckLocalAdminAccess",
                "Invoke-CredentialInjection",
                "Invoke-DllInjection",
                "Invoke-EnumerateLocalAdmin",
                "Invoke-EventHunter",
                "Invoke-FileFinder",
                "Invoke-MapDomainTrust",
                "Invoke-Mimikatz",
                "Invoke-NinjaCopy",
                "Invoke-Portscan",
                "Invoke-PrivescAudit",
                "Invoke-ProcessHunter",
                "Invoke-ReflectivePEInjection",
                "Invoke-ReverseDnsLookup",
                "Invoke-ServiceAbuse",
                "Invoke-ShareFinder",
                "Invoke-Shellcode",
                "Invoke-TokenManipulation",
                "Invoke-UserHunter",
                "Invoke-WmiCommand",
                "Mount-VolumeShadowCopy",
                "New-ElevatedPersistenceOption",
                "New-UserPersistenceOption",
                "New-VolumeShadowCopy",
                "Out-CompressedDll",
                "Out-EncodedCommand",
                "Out-EncryptedScript",
                "Out-Minidump",
                "Remove-Comments",
                "Remove-VolumeShadowCopy",
                "Restore-ServiceBinary",
                "Set-ADObject",
                "Set-CriticalProcess",
                "Set-MacAttribute",
                "Set-MasterBootRecord",
                "Set-ServiceBinPath",
                "Test-ServiceDaclPermission",
                "Write-HijackDll",
                "Write-ServiceBinary",
                "Write-UserAddMSI"
            };

            return commands.Any(m => context.Script.ToLower().Contains(m.ToLower()));
        }
    }
}
