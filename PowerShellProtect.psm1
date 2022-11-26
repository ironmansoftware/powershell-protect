function Install-PowerShellProtect {
    if (-not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) {
        Write-Warning "PowerShell Protect installation requires admin privileges."
        return
    }

    $OSVersion = [Version](Get-CimInstance -ClassName Win32_OperatingSystem).Version
    if ($OSVersion -lt [Version]::new(10, 0)) {
        Write-Warning "PowerShell Protect is supported on Windows 10 and Windows Server 2016 or later."
        return
    }

    & "$Env:windir\system32\regsvr32.exe" /s "$PSScriptRoot\x64\AmsiProvider.dll"
    & "$Env:windir\syswow64\regsvr32.exe" /s "$PSScriptRoot\x86\AmsiProvider.dll"

    if (-not (Test-Path "$Env:ProgramData\PowerShellProtect\config.xml")) {
        Set-PSPConfiguration -ConfigurationFilePath "$PSScriptRoot\config.xml" -FileSystem
    }

    Write-Host -ForegroundColor Green -Object "PowerShell Protect installed successfully and watching for known exploits! You can read more about configuring PowerShell Protect by visiting the documentation: https://docs.poshtools.com/powershell-pro-tools-documentation/powershell-protect"
}

function Uninstall-PowerShellProtect {
    param([Switch]$Force)

    & "$Env:windir\system32\regsvr32.exe" /s /u "$PSScriptRoot\x64\AmsiProvider.dll"
    & "$Env:windir\syswow64\regsvr32.exe" /s /u "$PSScriptRoot\x86\AmsiProvider.dll"

    if ($Force) {
        Restart-Computer
    }
    else {
        Write-Warning "You will need to restart your machine to ensure PowerShell Protect has been unloaded. You will not be able to uninstall this module with Uninstall-Module until the machine has been restarted."
    }
}