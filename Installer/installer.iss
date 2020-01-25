; Script generated by the Inno Script Studio Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Retrospect Client Notifier"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Ben's World"
#define MyAppURL "https://aldaviva.com"
#define MyAppExeName "RetrospectClientNotificationReceiver.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{6FF8262D-369E-451C-B55A-9A1E4E553016}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
VersionInfoVersion={#MyAppVersion}
VersionInfoCompany={#MyAppPublisher}
VersionInfoCopyright=2020 {#MyAppPublisher}
VersionInfoProductName={#MyAppName}
VersionInfoDescription={#MyAppName}
AppVerName={#MyAppName}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={code:GetDefaultDirName}
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir=bin
OutputBaseFilename=Retrospect Client Notifier setup
SetupIconFile=..\RetrospectClientNotificationReceiver\retroico.ico
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "..\RetrospectClientNotificationReceiver\bin\Release\RetrospectClientNotificationReceiver.exe"; DestDir: "{app}"; Flags: ignoreversion; BeforeInstall: TaskKill('RetrospectClientNotificationReceiver.exe')
Source: "..\RetrospectClientNotificationReceiver\bin\Release\DataSizeUnits.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\RetrospectClientNotificationSender\bin\Release\retroeventhandler.exe"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: files; Name: "{app}\RetrospectClientNotificationReceiver.exe"; BeforeInstall: TaskKill('RetrospectClientNotificationReceiver.exe')
Type: files; Name: "{app}\DataSizeUnits.dll"
Type: files; Name: "{app}\retroeventhandler.exe"

[Code]
procedure TaskKill(ProcessName: String);
var
  exitCode: Integer;
begin
  Exec('taskkill.exe', '/f /im "' + ProcessName + '"', '', SW_HIDE, ewWaitUntilTerminated, exitCode);
end;

function GetDefaultDirName(unused: string): string;
begin
  Result := ExpandConstant('{reg:HKLM\SYSTEM\CurrentControlSet\Services\Retrospect Client,ImagePath|{commonpf32}\Retrospect\Retrospect Client\RemotSvc.exe}');
  StringChangeEx(Result, '"', '', True);
  Result := Result + '\..';
end;