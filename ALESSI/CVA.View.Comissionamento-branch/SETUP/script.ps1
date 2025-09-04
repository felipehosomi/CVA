<#
.SYNOPSIS
    Parameters - An example on how to read parameters from an ini file.
.NOTES
    File Name  : Parameters.ps1 
    Author     : Fabrice ZERROUKI - fabricezerrouki@hotmail.com
.EXAMPLE
    PS D:\>Parameters.ps1
#>
Write-Host "Reading parameters from 'prefs.ini' file..." -ForegroundColor DarkGreen;
Get-Content "prefs.ini" | ForEach-Object -Begin {$settings=@{}} -Process {$store = [regex]::split($_,'='); if(($store[0].CompareTo("") -ne 0) -and ($store[0].StartsWith("[") -ne $True) -and ($store[0].StartsWith("#") -ne $True)) {$settings.Add($store[0], $store[1])}}
 
Write-Host "Integrating parameters from 'prefs.ini' file in variables... `n" -ForegroundColor DarkGreen;
$Param1 = $settings.Get_Item("USERNAME")
$Param2 = $settings.Get_Item("PASSWORD")
$Param3 = $settings.Get_Item("SERVER")
 
# Displaying the parameters
Write-Host "Param1 value is: $Param1" -ForegroundColor DarkYellow;
Write-Host "Param2 value is: $Param2" -ForegroundColor DarkYellow;
Write-Host "Param3 value is: $Param3`n" -ForegroundColor DarkYellow;

(gc CVA_Con_Service.exe.config) -replace 'Cva@sa', $Param2 | Out-File CVA_Con_Service.exe.config
(gc CVA_Con_Service.exe.config) -replace 'VM-001', $Param3 | Out-File CVA_Con_Service.exe.config
(gc CVA_Con_Service.exe.config) -replace 'VM-002', $Param3 | Out-File CVA_Con_Service.exe.config


