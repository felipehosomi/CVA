@ECHO OFF
cd "C:\Program Files (x86)\[CVA] Conciliador\Conciliador"
PowerShell.exe -NoProfile -Command "& {Start-Process PowerShell.exe -ArgumentList '-NoProfile -ExecutionPolicy Bypass -File ""script.ps1""' -Verb RunAs}"
PAUSE