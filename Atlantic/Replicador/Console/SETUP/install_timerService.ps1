function Test-Topshelf
{
	param([string]$path)
	$dllname = 'topshelf.dll'
	$topshelf = $false
	if (test-path -Path $path -PathType Container)
	{
		Push-Location
		Set-Location $path
		if(get-childitem -name $dllname)
		{
			$topshelf = $true
		}
		Pop-Location
		else
		{
			Push-Location
			Set-Location (get-item $path).DirectoryName
			if(get-childitem -name $dllname)
			{
			$topshelf = $true
			}
			Pop-Location
		}
		$topshelf
	}
}
function Install-Service
{
	param
	(
		[string]$servicename,
		[string]$username,
		[string]$password,
		[string]$path
	)
	#check to see if service includes topshelf.
	if(Test-TopShelf $path)
	{
		Start-Process $path -ArgumentList "install -username:$username -password:$password"
		#PS D:\myService\Services\csharpService> .\csharpService.exe install -username:mydomain\myServiceaccount -password:SerivcePassword
	}
	else
	{
		Write-Output "Did not detect a topshelf.dll in the directory $path is in"
	}
}
function Uninstall-Service
{
	param
	(
		[string]$servicename,
		[string]$path
	)
	#check to see if service includes topshelf.
	if(Test-TopShelf $path)
	{
		Start-Process $path -ArgumentList " uninstall"
		#PS D:\myService\Services\csharpService> .\csharpService.exe uninstall
	}
	else
	{
		Write-Output "Did not detect a topshelf.dll in the directory $path is in"
	}
}
function Stop-TopShelfService
{
	param
	(
		[string[]] $servers,
		[string[]] $svcNames
	)
	foreach($s in $servers)
	{
		foreach($v in $svcNames)
		{
			if((get-svcinstalled -name $v -ComputerName $s).installed -eq $true)
			{
				Stop-Service -inputobject (Get-Service -Name $v -ComputerName $s)
			}
		}
	}
}
function Start-TopShelfService
{
	param
	(
		[string[]] $servers,
		[string[]] $svcNames
	)
	foreach($s in $servers)
	{
		foreach($v in $svcNames)
		{
			if((get-svcinstalled -name $v -ComputerName $s).installed -eq $true)
			{
				Start-Service -inputobject (Get-Service -Name $v -ComputerName $s)
			}
		}
	}
}
function Get-SvcInstalled
{
	[CmdletBinding()]
	param
	(
		[parameter(ValueFromPipelineByPropertyName=$true)]
		[validatenotnullorempty()]
		[string[]]
		[Alias('Name')]
		$servicename,
		[parameter(ValueFromPipelineByPropertyName=$true)]
		[string[]]
		[Alias('ServerName','Server','MachineName')]
		$ComputerName
	)
	Begin
	{
		if(!($processedServices))
		{ 
			$processedServices = @()
		}
	}
	Process
	{
		#write-host $servicename
		if($ComputerName -eq $null)
		{
			$ComputerName = $env:COMPUTERNAME
		}
		$s = Get-service $servicename -ComputerName $ComputerName -ErrorAction SilentlyContinue
		if($s)
		{ 
			$returnval = $true
		}
		else
		{
			$returnval = $false
		}
		$processedServices += new-object psobject -Property (@{'serviceName' = $servicename;'installed' = $returnval;'ComputerName' = $ComputerName})
	}
	End
	{
		$processedServices
	}
}