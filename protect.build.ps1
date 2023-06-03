$Output = "$PSScriptRoot\output"

task Clean {
    Remove-Item -Path $Output -Force -Recurse -ErrorAction SilentlyContinue
    Remove-Item -Path "$PSScriptRoot\publish" -Force -Recurse -ErrorAction SilentlyContinue
}

task Build {
    Push-Location $PSScriptRoot
    & "$PSScriptRoot\nuget.exe" restore

    $path = .\vswhere -version "[17.0,18.0)" -requires Microsoft.Component.MSBuild -find MSBuild\Current\Bin\MSBuild.exe | Select-Object -First 1
    & $path .\AmsiProvider.sln /p:Configuration=Release /p:Platform=x64 /p:OutputPath="$PSScriptRoot\Engine\bin\Release\net462\x64\"

    New-Item -Path "$Output\x64" -ItemType Directory
    Copy-Item "$PSScriptRoot\Engine\bin\Release\net462\x64\Engine.dll" "$Output\x64"
    Copy-Item "$PSScriptRoot\x64\Release\AmsiProvider.dll" "$Output\x64"
    Copy-Item "$PSScriptRoot\Engine\bin\Release\net462\*.dll" "$Output"

    & $path .\AmsiProvider.sln /p:Configuration=Release /p:Platform=x86 /p:OutputPath="$PSScriptRoot\Engine\bin\Release\net462\x86\"

    New-Item -Path "$Output\x86" -ItemType Directory
    Copy-Item "$PSScriptRoot\Engine\bin\Release\net462\x86\Engine.dll" "$Output\x86"
    Copy-Item "$PSScriptRoot\Release\AmsiProvider.dll" "$Output\x86"

    Copy-Item "$PSScriptRoot\PowerShellProtect.psd1" $Output
    Copy-Item "$PSScriptRoot\PowerShellProtect.psm1" $Output
    Copy-Item "$PSScriptRoot\Engine\Configuration\config.xml" $Output
    Pop-Location

    $Assemblies = @(
        "x86/Engine.dll"
        "x86/AmsiProvider.dll"
        "x64/Engine.dll"
        "x64/AmsiProvider.dll"
        "PowerShellProtect.dll"
    )
}

task Test {
    $Pester = Get-Module -Name Pester -ListAvailable
    if (-not $Pester) {
        Install-Module Pester -Scope CurrentUser -Force
    }

    Invoke-Pester -Path "$PSScriptRoot\tests"
}

task Publish {
    New-Item -ItemType Directory -Path "$PSScriptRoot\publish\PowerShellProtect"
    Copy-Item -Path "$output\*" -Destination "$PSScriptRoot\publish\PowerShellProtect" -Recurse -Container
    Publish-Module -Path "$PSScriptRoot\publish\PowerShellProtect" -NuGetApiKey $Env:PowerShellGalleryKey
}

task . Clean, Build