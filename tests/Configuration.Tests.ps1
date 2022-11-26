Import-Module "$PSScriptRoot\..\output\PowerShellProtect.psd1"

Describe "Configuration" {

    BeforeEach {
        $TempFile = [System.IO.Path]::GetTempFileName()
    }

    AfterEach {
        Remove-Item $TempFile -ErrorAction SilentlyContinue
    }

    Context "Rules" {
        It "should create a rule" {

            $Condition = New-PSPCondition -Property "admin" -eq -Value $true
            $Action = New-PSPAction -Block
            $Rule = New-PSPRule -Name 'Rule' -Condition $Condition -Action $Action 
            $Config = New-PSPConfiguration -Rule $Rule -Action $Action 

            Save-PSPConfiguration -Configuration $Config -Path $TempFile

            [System.IO.File]::Exists($TempFile) | Should -Be $true
        }

        It "should test rule" {
            $Condition = New-PSPCondition -Property "command" -eq -Value "Get-Process"
            $Action = New-PSPAction -Block
            $Rule = New-PSPRule -Name 'Rule' -Condition $Condition -Action $Action 
            $Config = New-PSPConfiguration -Rule $Rule -Action $Action

            Test-PSPConfiguration -Configuration $Config -ScriptBlock { Get-Process } | Should -be 'AdminBlock'
        }

        
        It "should test file" {
            $Condition = New-PSPCondition -Property "command" -eq -Value "Get-Process"
            $Action = New-PSPAction -Block
            $Rule = New-PSPRule -Name 'Rule' -Condition $Condition -Action $Action 
            $Config = New-PSPConfiguration -Rule $Rule -Action $Action

            Save-PSPConfiguration -Configuration $Config -Path $TempFile

            Test-PSPConfiguration -ConfigurationPath $TempFile -ScriptBlock { Get-Process } | Should -be 'AdminBlock'
        }

        It "should test multiple rules" {
            $Condition = New-PSPCondition -Property "command" -eq -Value "Get-Process"
            $Condition2 = New-PSPCondition -Property "command" -eq -Value "Start-Process"

            $Action = New-PSPAction -Block
            $Rule = New-PSPRule -Name 'Rule' -Condition $Condition -Action $Action 
            $Rule2 = New-PSPRule -Name 'Rule2' -Condition $Condition2 -Action $Action 
            $Config = New-PSPConfiguration -Rule @($Rule, $Rule2) -Action $Action

            Save-PSPConfiguration -Configuration $Config -Path $TempFile

            Test-PSPConfiguration -ConfigurationPath $TempFile -ScriptBlock { Start-Process } | Should -be 'AdminBlock'
        }

        It "should test multiple conditions" {
            $Condition = New-PSPCondition -Property "command" -eq -Value "Get-Process"
            $Condition2 = New-PSPCondition -Property "command" -eq -Value "Start-Process"

            $Action = New-PSPAction -Block
            $Rule = New-PSPRule -Name 'Rule' -Condition @($Condition, $Condition2) -Action $Action 
            $Config = New-PSPConfiguration -Rule $Rule -Action $Action

            Save-PSPConfiguration -Configuration $Config -Path $TempFile

            Test-PSPConfiguration -ConfigurationPath $TempFile -ScriptBlock { Start-Process } | Should -be 'Ok'
            Test-PSPConfiguration -ConfigurationPath $TempFile -ScriptBlock { Start-Process; Get-Process } | Should -be 'AdminBlock'
        }

        It "should not block if not assigned" {
            $Condition = New-PSPCondition -Property "command" -eq -Value "Get-Process"

            $Action = New-PSPAction -Block
            $Rule = New-PSPRule -Name 'Rule' -Condition $Condition 
            $Config = New-PSPConfiguration -Rule $Rule -Action $Action

            Save-PSPConfiguration -Configuration $Config -Path $TempFile

            Test-PSPConfiguration -ConfigurationPath $TempFile -ScriptBlock { Get-Process } | Should -be 'Ok'
        }
    }
}