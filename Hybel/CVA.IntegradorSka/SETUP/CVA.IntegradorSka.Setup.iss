#include <idp.iss>
#define MyAppName "Integrador SKA"
#define MyAppVersion "1.0.0.0"
#define MyAppPublisher "CVA Consultoria"
#define MyAppUrl "http://www.cvaconsultoria.com.br"
#define IntegradorSap "CVA.IntegradorSapSka.exe"
#define IntegradorSka "CVA.IntegradorSkaSap.exe"
#define AppPublisher "CVA"
#define DotNetRuntimeExe "NDP461-KB3102436-x86-x64-AllOS-ENU.exe"

[Setup]
AppId={{F31DEEE0-9EE1-4A9C-BB08-FFB9A88A1F2B}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#AppPublisher}
DisableDirPage=no
DefaultGroupName={#AppPublisher}\{#MyAppName}
DisableProgramGroupPage=no
OutputBaseFilename=CVA.IntegradorSapSkaSetup
Compression=lzma
SolidCompression=yes
ShowLanguageDialog=no
WizardImageFile="logo1.bmp"
WizardImageStretch=yes
WizardSmallImageFile="logo2.bmp"
AllowNoIcons=yes
LicenseFile=License.rtf
SetupLogging=yes
Uninstallable=yes

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[LangOptions]
LanguageName=Brazilian Portuguese
LanguageID=$0416
LanguageCodePage=0
DialogFontName=Arial
DialogFontSize=8
WelcomeFontName=Verdana
WelcomeFontSize=12
TitleFontName=Arial
TitleFontSize=29
CopyrightFontName=Arial
CopyrightFontSize=8
RightToLeft=no

[Icons]
Name: "{group}\Integrador SAP"; Filename: "{app}\Integrador\SapSka\{#IntegradorSap}"
Name: "{group}\Integrador SKA"; Filename: "{app}\Integrador\SkaSap\{#IntegradorSka}"

[UninstallDelete]
Type: filesandordirs; Name: "{app}\*.*"

[Dirs]
Name: "{app}\Integrador\SapSka"
Name: "{app}\Integrador\SkaSap"

[Run]
Filename: "{app}\Integrador\SapSka\CVA.IntegradorSapSka.exe"; Parameters: "uninstall"
Filename: "{app}\Integrador\SapSka\CVA.IntegradorSapSka.exe"; Parameters: "install start"   
Filename: "{app}\Integrador\SkaSap\CVA.IntegradorSkaSap.exe"; Parameters: "uninstall"
Filename: "{app}\Integrador\SkaSap\CVA.IntegradorSkaSap.exe"; Parameters: "install start"

[UninstallRun]
Filename: "{app}\Integrador\SapSka\CVA.IntegradorSapSka.exe"; Parameters: "stop"
Filename: "{app}\Integrador\SapSka\CVA.IntegradorSapSka.exe"; Parameters: "uninstall"  Filename: "{app}\Integrador\SkaSap\CVA.IntegradorSkaSap.exe"; Parameters: "stop"
Filename: "{app}\Integrador\SkaSap\CVA.IntegradorSkaSap.exe"; Parameters: "uninstall"

[Files]
Source: "C:\Source\Hybel\CVA.IntegradorSka\SapSkaWs\bin\x86\Debug\CVA.IntegradorSapSka.exe"; DestDir: "{app}\Integrador\SapSka"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SapSkaWs\bin\x86\Debug\CVA.IntegradorSapSka.exe.config"; DestDir: "{app}\Integrador\SapSka"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SapSkaWs\bin\x86\Debug\Common.Logging.Core.xml"; DestDir: "{app}\Integrador\SapSka"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SapSkaWs\bin\x86\Debug\Common.Logging.xml"; DestDir: "{app}\Integrador\SapSka"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SapSkaWs\bin\x86\Debug\log4net.xml"; DestDir: "{app}\Integrador\SapSka"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SapSkaWs\bin\x86\Debug\Quartz.xml"; DestDir: "{app}\Integrador\SapSka"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SapSkaWs\bin\x86\Debug\Topshelf.Log4Net.xml"; DestDir: "{app}\Integrador\SapSka"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SapSkaWs\bin\x86\Debug\Topshelf.xml"; DestDir: "{app}\Integrador\SapSka"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SapSkaWs\bin\x86\Debug\Common.Logging.Core.dll"; DestDir: "{app}\Integrador\SapSka"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SapSkaWs\bin\x86\Debug\Common.Logging.dll"; DestDir: "{app}\Integrador\SapSka"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SapSkaWs\bin\x86\Debug\log4net.dll"; DestDir: "{app}\Integrador\SapSka"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SapSkaWs\bin\x86\Debug\Quartz.dll"; DestDir: "{app}\Integrador\SapSka"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SapSkaWs\bin\x86\Debug\Topshelf.dll"; DestDir: "{app}\Integrador\SapSka"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SapSkaWs\bin\x86\Debug\Topshelf.Log4Net.dll"; DestDir: "{app}\Integrador\SapSka"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SkaSapWs\bin\x86\Debug\CVA.IntegradorSkaSap.exe"; DestDir: "{app}\Integrador\SkaSap"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SkaSapWs\bin\x86\Debug\CVA.IntegradorSkaSap.exe.config"; DestDir: "{app}\Integrador\SkaSap"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SkaSapWs\bin\x86\Debug\Common.Logging.Core.xml"; DestDir: "{app}\Integrador\SkaSap"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SkaSapWs\bin\x86\Debug\Common.Logging.xml"; DestDir: "{app}\Integrador\SkaSap"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SkaSapWs\bin\x86\Debug\log4net.xml"; DestDir: "{app}\Integrador\SkaSap"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SkaSapWs\bin\x86\Debug\Quartz.xml"; DestDir: "{app}\Integrador\SkaSap"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SkaSapWs\bin\x86\Debug\Topshelf.Log4Net.xml"; DestDir: "{app}\Integrador\SkaSap"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SkaSapWs\bin\x86\Debug\Topshelf.xml"; DestDir: "{app}\Integrador\SkaSap"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SkaSapWs\bin\x86\Debug\Common.Logging.Core.dll"; DestDir: "{app}\Integrador\SkaSap"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SkaSapWs\bin\x86\Debug\Common.Logging.dll"; DestDir: "{app}\Integrador\SkaSap"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SkaSapWs\bin\x86\Debug\log4net.dll"; DestDir: "{app}\Integrador\SkaSap"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SkaSapWs\bin\x86\Debug\Quartz.dll"; DestDir: "{app}\Integrador\SkaSap"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SkaSapWs\bin\x86\Debug\Topshelf.dll"; DestDir: "{app}\Integrador\SkaSap"; Flags: ignoreversion
Source: "C:\Source\Hybel\CVA.IntegradorSka\SkaSapWs\bin\x86\Debug\Topshelf.Log4Net.dll"; DestDir: "{app}\Integrador\SkaSap"; Flags: ignoreversion
Source: "install_svc.bat"; DestDir: "{app}\Integrador"; Flags: ignoreversion
Source: "MEF\ISSkin.dll"; Flags: dontcopy
Source: "MEF\Vista.cjstyles"; Flags: dontcopy
Source: "{#DotNetRuntimeExe}"; DestDir: "{tmp}"; Flags: dontcopy nocompression noencryption
					
[CustomMessages]
InstallingDotNetFramework=Installing .NET Framework. This might take a few minutes...
DotNetFrameworkFailedToLaunch=Failed to launch .NET Framework Installer with error "%1". Please fix the error then run this installer again.
DotNetFrameworkFailed1602=.NET Framework installation was cancelled. This installation can continue, but be aware that this application may not run unless the .NET Framework installation is completed successfully.
DotNetFrameworkFailed1603=A fatal error occurred while installing the .NET Framework. Please fix the error, then run the installer again.
DotNetFrameworkFailed5100=Your computer does not meet the requirements of the .NET Framework. Please consult the documentation.
DotNetFrameworkFailedOther=The .NET Framework installer exited with an unexpected status code "%1". Please review any other messages shown by the installer to determine whether the installation completed successfully, and abort this installation and fix the problem if it did not.

[Code]
const
ADDON_ID = 'CVA_INT';
PARTNER_ID = 'CVACONSULTORIA';

var
  requiresRestart: boolean;

procedure LoadSkin(lpszPath: String; lpszIniFileName: String); external 'LoadSkin@files:isskin.dll stdcall';
procedure UnloadSkin(); external 'UnloadSkin@files:isskin.dll stdcall';
function ShowWindow(hWnd: Integer; uType: Integer): Integer; external 'ShowWindow@user32.dll stdcall';
procedure ExitProcess(exitCode:integer); external 'ExitProcess@kernel32.dll stdcall';

function DotNetIsMissing(): Boolean;
var
  readVal: cardinal;
  success: Boolean;
begin
  success := RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', readVal);
  success := success and ((readVal = 394254) or (readVal = 394271));
  Result := not success;
end;

function InstallDotNet(): String;
var
  statusText: string;
  resultCode: Integer;
begin
  statusText := WizardForm.StatusLabel.Caption;
  WizardForm.StatusLabel.Caption := CustomMessage('InstallingDotNetFramework');
  WizardForm.ProgressGauge.Style := npbstMarquee;
  try
    ExtractTemporaryFile('{#DotNetRuntimeExe}');
    if not Exec(ExpandConstant('{tmp}\{#DotNetRuntimeExe}'), '/passive /norestart /showrmui /showfinalerror', '', SW_SHOW, ewWaitUntilTerminated, resultCode) then
    begin
       Result := FmtMessage(CustomMessage('DotNetFrameworkFailedToLaunch'), [SysErrorMessage(resultCode)]);
    end
    else
    begin
      case resultCode of
          0: begin
            // Successful
          end;
          1602 : begin
            MsgBox(CustomMessage('DotNetFrameworkFailed1602'), mbInformation, MB_OK);
          end;
          1603: begin
            Result := CustomMessage('DotNetFrameworkFailed1603');
          end;
          1641: begin
            requiresRestart := True;
          end;
          3010: begin
            requiresRestart := True;
          end;
          5100: begin
            Result := CustomMessage('DotNetFrameworkFailed5100');
          end;
          else begin
            MsgBox(FmtMessage(CustomMessage('DotNetFrameworkFailedOther'), [IntToStr(resultCode)]), mbError, MB_OK);
          end;
      end;
    end;
  finally
      WizardForm.StatusLabel.Caption := statusText;
      WizardForm.ProgressGauge.Style := npbstNormal;
  end;
end;

function PrepareToInstall(var NeedsRestart: Boolean): String;
begin
    if DotNetIsMissing() then
    begin
      Result := InstallDotNet();
    end;
end;

function NeedRestart(): Boolean;
begin
    Result := requiresRestart;
end;

function GetDLLPath(IsUninstalling: boolean): string;
var 
	ValueName: string;
begin
	if IsUninstalling then 
	begin
		RegQueryStringValue(HKEY_LOCAL_MACHINE, 'SOFTWARE\' + PARTNER_ID + '\'	+ ADDON_ID, 'DLLInstallDir', ValueName);
		Result := ValueName;
	end
	else
		Result:=Copy(ParamStr(2), 0, Length(ParamStr(2))-Pos('|', ParamStr(2)));  		
end;    

procedure RegisterAddOnInfo();
begin
	RegWriteStringValue(HKEY_LOCAL_MACHINE, 'SOFTWARE\' + PARTNER_ID + '\' + ADDON_ID, 'InstallDir', 'CVA');
	RegWriteStringValue(HKEY_LOCAL_MACHINE, 'SOFTWARE\' + PARTNER_ID + '\' + ADDON_ID, 'DLLInstallDir', GetDLLPath(false));
end;

procedure UnregisterAddOnInfo();
begin
	RegDeleteValue(HKEY_LOCAL_MACHINE, 'SOFTWARE\' + PARTNER_ID + '\' + ADDON_ID, 'InstallDir');
	RegDeleteValue(HKEY_LOCAL_MACHINE, 'SOFTWARE\' + PARTNER_ID + '\' + ADDON_ID, 'DLLInstallDir');
	RegDeleteKeyIfEmpty(HKEY_LOCAL_MACHINE, 'SOFTWARE\' + PARTNER_ID + '\' + ADDON_ID);
end;           

function GetAddOnInstalledPath(): string;
var 
	ValueName: string;
begin
	RegQueryStringValue(HKEY_LOCAL_MACHINE, 'SOFTWARE\' + PARTNER_ID + '\' + ADDON_ID, 'InstallDir', ValueName);
	Result := ValueName;
end;

//*******************************************************************************************************
//  InitializeSetup (função interna do Inno)
//  Executada sempre que a instalação/desinstalação inicia
//*******************************************************************************************************
function InitializeSetup(): boolean;
var 
	ResultCode: integer;
begin
	// Está desinstalando...
	if ParamStr(2) = '/u' then 
	begin
		ExtractTemporaryFile('Vista.cjstyles');
		LoadSkin(ExpandConstant('{tmp}\Vista.cjstyles'), '');
		SetCurrentDir(GetDLLPath(true));
		Exec(GetAddOnInstalledPath + '\unins000.exe', '', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) 
		Result := false;
	end  
	// Está na instalação...
	else begin   
		ExtractTemporaryFile('Vista.cjstyles');
		LoadSkin(ExpandConstant('{tmp}\Vista.cjstyles'), '');
		SetCurrentDir(GetDLLPath(false));       
		Result := true;
	end;
end;

//*******************************************************************************************************
//  CurStepChanged (função interna do Inno)
//
//*******************************************************************************************************
procedure CurStepChanged(CurStep: TSetupStep);
begin 
	// Ao terminar a instalação com sucesso informa ao SB1 o diretório
	// de instalação e dá o sinal de que a instalação terminou
	if CurStep = ssPostInstall then 
	begin		
		RegisterAddOnInfo(); // grava a chave de instalação do add-on no Registro  
	end;      
end;

procedure DeinitializeSetup();
begin
	ShowWindow(StrToInt(ExpandConstant('{wizardhwnd}')), 0);
	UnloadSkin();
end;

//*******************************************************************************************************
//  CurUninstallStepChanged (função interna do Inno)
//
//*******************************************************************************************************
procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin 
	// Ao terminar a desinstalação com sucesso avisa ao SB1
	if CurUninstallStep = usPostUninstall then 
	begin
		UnregisterAddOnInfo(); // Remove do Registro a pasta de instalação do add-on
	end;
end;