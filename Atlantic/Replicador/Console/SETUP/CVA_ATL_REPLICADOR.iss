#include <idp.iss>
#define MyAppName "Replicador"
#define MyAppVersion "0.0.0.36"
#define MyAppPublisher "CVA Consultoria"
#define MyAppUrl "http://www.cvaconsultoria.com.br"
#define TimerExe "CVA_Rep_Timer.exe"
#define GerenciadorExe "CVA_RepConfig.exe"
#define AppPublisher "CVA"

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
OutputBaseFilename=CVAReplicadorSetup
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
Name: "{group}\Replicador\Gerenciador"; IconFilename: "{app}\Replicador\gerenciador.ico"; Filename: "{app}\Replicador\Gerenciador\{#GerenciadorExe}"
Name: "{group}\Replicador\Timer"; IconFilename: "{app}\Replicador\timer.ico"; Filename: "{app}\Replicador\Timer\{#TimerExe}"

[UninstallDelete]
Type: filesandordirs; Name: "{app}\*.*"

[Dirs]
Name: "{app}\Replicador\Gerenciador"
Name: "{app}\Replicador\Service"
Name: "{app}\Replicador\Timer"

[Files]
Source: "..\CVA_Rep_Gerenciador\CVA_RepConfig\bin\Debug\CVA_Rep_BLL.dll"; DestDir: "{app}\Replicador\Gerenciador"; Flags: ignoreversion
Source: "..\CVA_Rep_Gerenciador\CVA_RepConfig\bin\Debug\CVA_Rep_DAL.dll"; DestDir: "{app}\Replicador\Gerenciador"; Flags: ignoreversion
Source: "..\CVA_Rep_Gerenciador\CVA_RepConfig\bin\Debug\CVA_Rep_Exception.dll"; DestDir: "{app}\Replicador\Gerenciador"; Flags: ignoreversion
Source: "..\CVA_Rep_Gerenciador\CVA_RepConfig\bin\Debug\CVA_Rep_Logging.dll"; DestDir: "{app}\Replicador\Gerenciador"; Flags: ignoreversion
Source: "..\CVA_Rep_Gerenciador\CVA_RepConfig\bin\Debug\CVA_RepConfig.exe"; DestDir: "{app}\Replicador\Gerenciador"; Flags: ignoreversion
Source: "..\CVA_Rep_Gerenciador\CVA_RepConfig\bin\Debug\CVA_RepConfig.exe.config"; DestDir: "{app}\Replicador\Gerenciador"; Flags: ignoreversion
Source: "..\CVA_Rep_Gerenciador\CVA_RepConfig\bin\Debug\EntityFramework.dll"; DestDir: "{app}\Replicador\Gerenciador"; Flags: ignoreversion
Source: "..\CVA_Rep_Gerenciador\CVA_RepConfig\bin\Debug\EntityFramework.SqlServer.dll"; DestDir: "{app}\Replicador\Gerenciador"; Flags: ignoreversion
Source: "..\CVA_Rep_Gerenciador\CVA_RepConfig\bin\Debug\EntityFramework.SqlServer.xml"; DestDir: "{app}\Replicador\Gerenciador"; Flags: ignoreversion
Source: "..\CVA_Rep_Gerenciador\CVA_RepConfig\bin\Debug\EntityFramework.xml"; DestDir: "{app}\Replicador\Gerenciador"; Flags: ignoreversion
Source: "..\CVA_Rep_Gerenciador\CVA_RepConfig\bin\Debug\log4net.config"; DestDir: "{app}\Replicador\Gerenciador"; Flags: ignoreversion
Source: "..\CVA_Rep_Gerenciador\CVA_RepConfig\bin\Debug\log4net.dll"; DestDir: "{app}\Replicador\Gerenciador"; Flags: ignoreversion
Source: "..\CVA_Rep_Gerenciador\CVA_RepConfig\bin\Debug\log4net.xml"; DestDir: "{app}\Replicador\Gerenciador"; Flags: ignoreversion
Source: "..\CVA_Rep_Gerenciador\CVA_RepConfig\bin\Debug\Resources\*"; DestDir: "{app}\Replicador\Gerenciador\Resources"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\CVA_Rep_Service\CVA_Rep_Service\bin\x64\Debug\CVA_Rep_Service.exe"; DestDir: "{app}\Replicador\Service"; Flags: ignoreversion
Source: "..\CVA_Rep_Service\CVA_Rep_Service\bin\x64\Debug\CVA_Obj_Shared.dll"; DestDir: "{app}\Replicador\Service"; Flags: ignoreversion
Source: "..\CVA_Rep_Service\CVA_Rep_Service\bin\x64\Debug\CVA_Rep_BLL.dll"; DestDir: "{app}\Replicador\Service"; Flags: ignoreversion
Source: "..\CVA_Rep_Service\CVA_Rep_Service\bin\x64\Debug\CVA_Rep_DAL.dll"; DestDir: "{app}\Replicador\Service"; Flags: ignoreversion
Source: "..\CVA_Rep_Service\CVA_Rep_Service\bin\x64\Debug\CVA_Rep_Exception.dll"; DestDir: "{app}\Replicador\Service"; Flags: ignoreversion
Source: "..\CVA_Rep_Service\CVA_Rep_Service\bin\x64\Debug\CVA_Rep_Logging.dll"; DestDir: "{app}\Replicador\Service"; Flags: ignoreversion
Source: "..\CVA_Rep_Service\CVA_Rep_Service\bin\x64\Debug\EntityFramework.dll"; DestDir: "{app}\Replicador\Service"; Flags: ignoreversion
Source: "..\CVA_Rep_Service\CVA_Rep_Service\bin\x64\Debug\EntityFramework.SqlServer.dll"; DestDir: "{app}\Replicador\Service"; Flags: ignoreversion
Source: "..\CVA_Rep_Service\CVA_Rep_Service\bin\x64\Debug\log4net.dll"; DestDir: "{app}\Replicador\Service"; Flags: ignoreversion
Source: "..\CVA_Rep_Service\CVA_Rep_Service\bin\x64\Debug\app-settings.ico"; DestDir: "{app}\Replicador\Service"; Flags: ignoreversion
Source: "..\CVA_Rep_Service\CVA_Rep_Service\bin\x64\Debug\Topshelf.dll"; DestDir: "{app}\Replicador\Service"; Flags: ignoreversion
Source: "..\CVA_Rep_Service\CVA_Rep_Service\bin\x64\Debug\CVA_Rep_Service.exe.config"; DestDir: "{app}\Replicador\Service"; Flags: ignoreversion
Source: "..\CVA_Rep_Service\CVA_Rep_Service\bin\x64\Debug\log4net.config"; DestDir: "{app}\Replicador\Service"; Flags: ignoreversion
Source: "..\CVA_Rep_Service\CVA_Rep_Service\bin\x64\Debug\EntityFramework.SqlServer.xml"; DestDir: "{app}\Replicador\Service"; Flags: ignoreversion
Source: "..\CVA_Rep_Service\CVA_Rep_Service\bin\x64\Debug\EntityFramework.xml"; DestDir: "{app}\Replicador\Service"; Flags: ignoreversion
Source: "..\CVA_Rep_Service\CVA_Rep_Service\bin\x64\Debug\log4net.xml"; DestDir: "{app}\Replicador\Service"; Flags: ignoreversion
Source: "..\CVA_Rep_Service\CVA_Rep_Service\bin\x64\Debug\Plugins\*"; DestDir: "{app}\Replicador\Service\Plugins"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\CVA_Rep_Timer.exe"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\Common.Logging.Core.dll"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\Common.Logging.dll"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\CVA_Rep_BLL.dll"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\CVA_Rep_DAL.dll"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\CVA_Rep_Exception.dll"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\EntityFramework.dll"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\EntityFramework.SqlServer.dll"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\log4net.dll"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\Quartz.dll"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\CVA_Rep_Timer.exe.config"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\Common.Logging.Core.xml"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\Common.Logging.xml"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\EntityFramework.SqlServer.xml"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\EntityFramework.xml"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\log4net.xml"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\Quartz.xml"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\Topshelf.dll"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\Topshelf.xml"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\Topshelf.Log4Net.dll"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\Topshelf.Log4Net.xml"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "..\CVA_Rep_Timer\CVA_Rep_Timer\bin\Debug\log4net.config"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "install_svc.bat"; DestDir: "{app}\Replicador\Timer"; Flags: ignoreversion
Source: "timer.ico"; DestDir: "{app}\Replicador"; Flags: ignoreversion
Source: "gerenciador.ico"; DestDir: "{app}\Replicador"; Flags: ignoreversion
Source: "cva.config"; DestDir: "{app}\"; Flags: ignoreversion
Source: "config.bat"; DestDir: "{app}\"; Flags: ignoreversion
Source: "MEF\ISSkin.dll"; Flags: dontcopy
Source: "MEF\Vista.cjstyles"; Flags: dontcopy
Source: "script1.sql"; Flags: dontcopy
Source: "script2.sql"; Flags: dontcopy
Source: "script3.sql"; Flags: dontcopy
					
[CustomMessages]
CustomForm_Caption=Conectar ao servidor de banco de dados
CustomForm_Description=Entre com a informação necessária
CustomForm_lblServer_Caption0=Servidor:
CustomForm_lblAuthType_Caption0=Log-in
CustomForm_lblUser_Caption0=Usuário:
CustomForm_lblPassword_Caption0=Senha:
CustomForm_lblDatabase_Caption0=Banco:
CustomForm_lblVersion_Caption0=Versão:
CustomForm_chkSQLAuth_Caption0=Usar Autenticação do SQL
CustomForm_chkWindowsAuth_Caption0=Usar Autenticação do Windows
CustomForm_lstVersion_Line0=MS-SQL Server 2008 R2
CustomForm_lstVersion_Line1=MS-SQL Server 2012
CustomForm_lstVersion_Line2=MS-SQL Server 2014

[Code]
const
ADDON_ID = 'CVA_REP';
PARTNER_ID = 'CVACONSULTORIA';
adCmdUnspecified = $FFFFFFFF;
adCmdUnknown = $00000008;
adCmdText = $00000001;
adCmdTable = $00000002;
adCmdStoredProc = $00000004;
adCmdFile = $00000100;
adCmdTableDirect = $00000200;
adOptionUnspecified = $FFFFFFFF;
adAsyncExecute = $00000010;
adAsyncFetch = $00000020;
adAsyncFetchNonBlocking = $00000040;
adExecuteNoRecords = $00000080;
adExecuteStream = $00000400;
adExecuteRecord = $00000800;

var
lblVersion: TLabel;
lstVersion: TComboBox;
lblServer: TLabel;
lblAuthType: TLabel;
lblUser: TLabel;
lblPassword: TLabel;
lblDatabase: TLabel;
chkSQLAuth: TRadioButton;
txtServer: TEdit;
chkWindowsAuth: TRadioButton;
txtUsername: TEdit;
txtPassword: TPasswordEdit;
lstDatabase: TComboBox;
bIsNextEnabled: Boolean;

var
Page: TWizardPage;

procedure LoadSkin(lpszPath: String; lpszIniFileName: String); external 'LoadSkin@files:isskin.dll stdcall';
procedure UnloadSkin(); external 'UnloadSkin@files:isskin.dll stdcall';
function ShowWindow(hWnd: Integer; uType: Integer): Integer; external 'ShowWindow@user32.dll stdcall';
procedure ExitProcess(exitCode:integer); external 'ExitProcess@kernel32.dll stdcall';

Procedure VersionOnChange (Sender: TObject);
begin                            
	lblServer.Enabled := True;
	txtServer.Enabled := True;
end;

Procedure ServerOnChange (Sender: TObject);
begin                            
	lstDatabase.Items.Clear;
	lstDatabase.Text := '';
	bIsNextEnabled := False;
	WizardForm.NextButton.Enabled := bIsNextEnabled;

	if Length(txtServer.Text) > 0 then
	begin
		lblAuthType.Enabled := True;
		lblDatabase.Enabled := True;
		lstDatabase.Enabled := True;
		chkWindowsAuth.Enabled := True;
		chkSQLAuth.Enabled := True;
	end
	else
	begin
		lblAuthType.Enabled := False;
		lblDatabase.Enabled := False;
		lstDatabase.Enabled := False; 
		chkWindowsAuth.Enabled := False;
		chkSQLAuth.Enabled := False;
	end
end;

procedure  AuthOnChange (Sender: TObject);
begin
	if chkSQLAuth.Checked then
	begin
		lblUser.Enabled := true;
		lblPassword.Enabled := true;
		txtUsername.Enabled := true;
		txtPassword.Enabled := true;
	end
	else
	begin
		lblUser.Enabled := false;
		lblPassword.Enabled := false;
		txtUsername.Enabled := false;
		txtPassword.Enabled := false;
	end
end;

Procedure DatabaseOnChange (Sender: TObject);
begin
	if (Length(lstDatabase.Text) > 0) and (lstDatabase.Enabled) then
	begin
		bIsNextEnabled := True;
		WizardForm.NextButton.Enabled := bIsNextEnabled;  
	end
	else
	begin
		bIsNextEnabled := False;
		WizardForm.NextButton.Enabled := bIsNextEnabled;  
	end
end;

procedure RetrieveDatabaseList(Sender: TObject);
var  
	ADOCommand: Variant;
	ADORecordset: Variant;
	ADOConnection: Variant;  
begin
	lstDatabase.Items.Clear;

	try
		ADOConnection := CreateOleObject('ADODB.Connection');
		ADOConnection.ConnectionString := 
			'Provider=SQLOLEDB;' +
			'Data Source=' + txtServer.Text + ';' +
			'Application Name=' + '{#SetupSetting("AppName")}' + ' DB List;'
		if chkWindowsAuth.Checked then
			ADOConnection.ConnectionString := ADOConnection.ConnectionString +
				'Integrated Security=SSPI;'
	else
		ADOConnection.ConnectionString := ADOConnection.ConnectionString +
			'User Id=' + txtUsername.Text + ';' +
			'Password=' + txtPassword.Text + ';';
			ADOConnection.Open;
			
	try
		ADOCommand := CreateOleObject('ADODB.Command');
		ADOCommand.ActiveConnection := ADOConnection;
		ADOCommand.CommandText := 'SELECT name FROM master.dbo.sysdatabases WHERE HAS_DBACCESS(name) = 1 ORDER BY name';
		ADOCommand.CommandType := adCmdText;
		ADORecordset := ADOCommand.Execute;
		
		while not ADORecordset.eof do 
		begin
			lstDatabase.Items.Add(ADORecordset.Fields(0));
			ADORecordset.MoveNext;
		end      
	finally
		ADOConnection.Close;
	end;
	except
		MsgBox(GetExceptionMessage, mbError, MB_OK);
	end;
end;

function Framework45IsNotInstalled() : Boolean;
var
	bSuccess: Boolean;
	regVersion: Cardinal;
begin
	Result := True;
	bSuccess := RegQueryDWordValue(HKLM, 'Software\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', regVersion);
	if (True = bSuccess) and (regVersion >= 379893) then begin
		Result := False;
	end;
end;

function LoadScriptFromFile(const FileName: string; out CommandList: TStrings): Integer;
var
	I: Integer;
	SQLCommand: string;
	ScriptFile: TStringList;
begin
	Result := 0;
	ScriptFile := TStringList.Create;
	
	try
		SQLCommand := '';
		ScriptFile.LoadFromFile(FileName);
		
		for I := 0 to ScriptFile.Count - 1 do
		begin
			if Pos('go', LowerCase(Trim(ScriptFile[I]))) = 1 then
			begin
				Result := Result + 1;
				CommandList.Add(SQLCommand);
				SQLCommand := '';
			end
			else
				SQLCommand := SQLCommand + ScriptFile[I] + #13#10;
		end;
		
		CommandList.Add(SQLCommand);
		Result := Result + 1;
	finally
		ScriptFile.Free;
	end;
end;

procedure DeploySQL(FileName: string; SFileName: string);
var  
	Script2014: AnsiString;    
	Script2008: AnsiString;
	Script2012: AnsiString;   
	ADOCommand: Variant;
	ADOConnection: Variant;
	UnicodeStr: string;
	ANSIStr: AnsiString;
	I: Integer;
	CommandList: TStrings; 
begin
	if lstVersion.Text='MS-SQL Server 2008 R2' then
		ExtractTemporaryFile(SFileName);
	if lstVersion.text='MS-SQL Server 2012' then
		ExtractTemporaryFile(SFileName);
	if lstVersion.Text='MS-SQL Server 2014' then
		ExtractTemporaryFile(SFileName);

	try
		ADOConnection := CreateOleObject('ADODB.Connection');
		ADOConnection.ConnectionString := 
			'Provider=SQLOLEDB;' +
			'Data Source=' + txtServer.Text + ';' +
			'Initial Catalog=' + lstDatabase.Text + ';' +
			'Application Name=' + '{#SetupSetting("AppName")}' + ' Execute SQL;' ;     
		
		if chkWindowsAuth.Checked then
			ADOConnection.ConnectionString := ADOConnection.ConnectionString +
				'Integrated Security=SSPI;'
		else
			ADOConnection.ConnectionString := ADOConnection.ConnectionString +
			'User Id=' + txtUsername.Text + ';' +
			'Password=' + txtPassword.Text + ';';
		
		ADOConnection.Open;
		
		try
			ADOCommand := CreateOleObject('ADODB.Command');
			ADOCommand.ActiveConnection := ADOConnection;       
	
			if LoadStringFromFile(ExpandConstant(FileName), Script2014) then
			begin            				
				if LoadStringFromFile(ExpandConstant(FileName), ANSIStr) then
				begin
					UnicodeStr := String(ANSIStr);
					SaveStringToFile(ExpandConstant(FileName), UnicodeStr, False);
					LoadStringFromFile(ExpandConstant(FileName), Script2014)
				end;
				
				CommandList := TStringList.Create;
				if LoadScriptFromFile(ExpandConstant(FileName), CommandList) > 0 then
					for I := 0 to CommandList.Count - 1 do
					begin
						Log(CommandList[I]);
						ADOCommand.CommandText := CommandList[I];
						ADOCommand.Execute(NULL, NULL, adCmdText or adExecuteNoRecords);
					end;				
			end
			else
			begin
				MsgBox('Installation files missing.', mbError, MB_OK);
				ExitProcess(7);
			end
		finally
			CommandList.Free;
			ADOConnection.Close;
		end;
		except
			MsgBox(GetExceptionMessage, mbError, MB_OK);
			ExitProcess(5);
		end;  
end;

{ CustomForm_NextkButtonClick }
function CustomForm_NextButtonClick(Page: TWizardPage): Boolean;
var  
	ADOConnection: Variant;  
begin
	ADOConnection := CreateOleObject('ADODB.Connection');
	ADOConnection.ConnectionString := 
		'Provider=SQLOLEDB;' +
		'Data Source=' + txtServer.Text + ';' +
		'Initial Catalog=' + lstDatabase.Text + ';' +
		'Application Name=' + '{#SetupSetting("AppName")}' + ' Execute SQL;' ;
				 
	if chkWindowsAuth.Checked then
		ADOConnection.ConnectionString := ADOConnection.ConnectionString +
			'Integrated Security=SSPI;'
	else
		ADOConnection.ConnectionString := ADOConnection.ConnectionString +
			'User Id=' + txtUsername.Text + ';' +
			'Password=' + txtPassword.Text + ';';

	ADOConnection.Open;
	Result := True;
end;

{ CustomForm_CreatePage }
function CustomForm_CreatePage(PreviousPageId: Integer): Integer;
begin
	Page := CreateCustomPage(
		PreviousPageId,
		ExpandConstant('{cm:CustomForm_Caption}'),
		ExpandConstant('{cm:CustomForm_Description}')
	);

	{ lblVersion }
	lblVersion := TLabel.Create(Page);
	with lblVersion do
	begin
		Parent := Page.Surface;
		Caption := ExpandConstant('{cm:CustomForm_lblVersion_Caption0}');
		Left := ScaleX(24);
		Top := ScaleY(8);
		Width := ScaleX(61);
		Height := ScaleY(13);
	end;

	{ lstVersion }
	lstVersion := TComboBox.Create(Page);
	with lstVersion do
	begin
		Parent := Page.Surface;
		Left := ScaleX(112);
		Top := ScaleY(8);
		Width := ScaleX(145);
		Height := ScaleY(21);
		Style := csDropDownList;
		DropDownCount := 3;
		TabOrder := 0;
		Items.Add(ExpandConstant('{cm:CustomForm_lstVersion_Line0}'));
		Items.Add(ExpandConstant('{cm:CustomForm_lstVersion_Line1}'));
		Items.Add(ExpandConstant('{cm:CustomForm_lstVersion_Line2}'));
		OnChange:= @VersionOnChange;
	end;

	{ lblServer }
	lblServer := TLabel.Create(Page);
	with lblServer do
	begin
		Parent := Page.Surface;
		Caption := ExpandConstant('{cm:CustomForm_lblServer_Caption0}');
		Left := ScaleX(24);
		Top := ScaleY(32);
		Width := ScaleX(68);
		Height := ScaleY(13);
		Enabled := False;
	end;

	{ txtServer }
	txtServer := TEdit.Create(Page);
	with txtServer do
	begin
		Parent := Page.Surface;
		Left := ScaleX(112);
		Top := ScaleY(32);
		Width := ScaleX(273);
		Height := ScaleY(21);
		TabOrder := 1;
		Enabled := False;
		OnChange := @ServerOnChange;
	end;

	{ lblAuthType }
	lblAuthType := TLabel.Create(Page);
	with lblAuthType do
	begin
		Parent := Page.Surface;
		Caption := ExpandConstant('{cm:CustomForm_lblAuthType_Caption0}');
		Left := ScaleX(24);
		Top := ScaleY(72);
		Width := ScaleX(87);
		Height := ScaleY(13);
		Enabled := False;
	end;

	{ chkWindowsAuth }
	chkWindowsAuth := TRadioButton.Create(Page);
	with chkWindowsAuth do
	begin
		Parent := Page.Surface;
		Caption := ExpandConstant('{cm:CustomForm_chkWindowsAuth_Caption0}');
		Left := ScaleX(32);
		Top := ScaleY(88);
		Width := ScaleX(177);
		Height := ScaleY(17);
		Checked := True;
		TabOrder := 2;
		TabStop := True;
		OnClick := @AuthOnChange;
		Enabled := False;
	end;

	{ chkSQLAuth }
	chkSQLAuth := TRadioButton.Create(Page);
	with chkSQLAuth do
	begin
		Parent := Page.Surface;
		Caption := ExpandConstant('{cm:CustomForm_chkSQLAuth_Caption0}');
		Left := ScaleX(32);
		Top := ScaleY(108);
		Width := ScaleX(185);
		Height := ScaleY(17);
		TabOrder := 3;
		OnClick := @AuthOnChange;
		Enabled := False;
	end;

	{ lblUser }
	lblUser := TLabel.Create(Page);
	with lblUser do
	begin
		Parent := Page.Surface;
		Caption := ExpandConstant('{cm:CustomForm_lblUser_Caption0}');
		Left := ScaleX(56);
		Top := ScaleY(128);
		Width := ScaleX(58);
		Height := ScaleY(13);
		Enabled := False;
	end;

	{ lblPassword }
	lblPassword := TLabel.Create(Page);
	with lblPassword do
	begin
		Parent := Page.Surface;
		Caption := ExpandConstant('{cm:CustomForm_lblPassword_Caption0}');
		Left := ScaleX(56);
		Top := ScaleY(152);
		Width := ScaleX(53);
		Height := ScaleY(13);
		Enabled := False;
	end;

	{ txtUsername }
	txtUsername := TEdit.Create(Page);
	with txtUsername do
	begin
		Parent := Page.Surface;
		Left := ScaleX(120);
		Top := ScaleY(128);
		Width := ScaleX(241);
		Height := ScaleY(21);
		Enabled := False;
		TabOrder := 4;
	end;

	{ txtPassword }
	txtPassword := TPasswordEdit.Create(Page);
	with txtPassword do
	begin
		Parent := Page.Surface;
		Left := ScaleX(120);
		Top := ScaleY(152);
		Width := ScaleX(241);
		Height := ScaleY(21);
		Enabled := False;
		TabOrder := 5;
	end;

	{ lblDatabase }
	lblDatabase := TLabel.Create(Page);
	with lblDatabase do
	begin
		Parent := Page.Surface;
		Caption := ExpandConstant('{cm:CustomForm_lblDatabase_Caption0}');
		Left := ScaleX(56);
		Top := ScaleY(192);
		Width := ScaleX(53);
		Height := ScaleY(13);
		Enabled := False;
	end;

	{ lstDatabase }
	lstDatabase := TComboBox.Create(Page);
	with lstDatabase do
	begin
		Parent := Page.Surface;
		Left := ScaleX(120);
		Top := ScaleY(192);
		Width := ScaleX(145);
		Height := ScaleY(21);
		Enabled := False;
		TabOrder := 6;    
		OnDropDown:= @RetrieveDatabaseList;
		OnChange:= @DatabaseOnChange;
	end;

	with Page do
	begin
		OnNextButtonClick := @CustomForm_NextButtonClick;
	end;

	Result := Page.ID;
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
procedure InitializeWizard();
begin
	if Framework45IsNotInstalled() then
	begin
	    idpAddFile('http://go.microsoft.com/fwlink/?LinkId=397707', ExpandConstant('{tmp}\NetFrameworkInstaller.exe'));
			idpDownloadAfter(wpReady);
	end;
	bIsNextEnabled := False;
	CustomForm_CreatePage(wpLicense);
end;

procedure InstallFramework;
var
	StatusText: string;
	ResultCode: Integer;
begin
	StatusText := WizardForm.StatusLabel.Caption;
	WizardForm.StatusLabel.Caption := 'Installing .NET Framework 4.5.2. This might take a few minutes...';
	WizardForm.ProgressGauge.Style := npbstMarquee;
	try
		if not Exec(ExpandConstant('{tmp}\NetFrameworkInstaller.exe'), '/passive /norestart', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
		begin
			MsgBox('.NET installation failed with code: ' + IntToStr(ResultCode) + '.', mbError, MB_OK);
		end;
	finally
		WizardForm.StatusLabel.Caption := StatusText;
		WizardForm.ProgressGauge.Style := npbstNormal;
		
	DeleteFile(ExpandConstant('{tmp}\NetFrameworkInstaller.exe'));
	end;
end;

procedure CurPageChanged(CurPageID: Integer);
begin
	// set initial status of next button. Should be disabled when page is first loaded, but should be enabled if user clicked back.
	if CurPageID = Page.ID then
		WizardForm.NextButton.Enabled := bIsNextEnabled;  
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
		if Framework45IsNotInstalled() then
		begin
			InstallFramework();
		end;
		RegisterAddOnInfo(); // grava a chave de instalação do add-on no Registro  
	end;      

	if CurStep = ssInstall then
	begin
		DeploySQL('{tmp}\script1.sql', 'script1.sql');
		DeploySQL('{tmp}\script2.sql', 'script2.sql');
		DeploySQL('{tmp}\script3.sql', 'script3.sql');
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