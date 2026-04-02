; Defter2Fis.ForMikro - Inno Setup Script
; E-Defter XML'den Mikro ERP V16 Jump Muhasebe Fişi Oluşturucu

#define MyAppName "Defter2Fis"
#define MyAppVersion "2.18.1"
#define MyAppPublisher "Zafer Bilgisayar"
#define MyAppExeName "Defter2Fis.ForMikro.exe"
#define MyAppDescription "E-Defter XML'den Mikro ERP V16 Jump Muhasebe Fişi Oluşturucu"

[Setup]
AppId={{73345C3A-E418-4994-B4DA-1B6FA6FC6277}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} v{#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=..\Installer\Output
OutputBaseFilename=Defter2Fis_Setup_v{#MyAppVersion}
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=admin
UninstallDisplayName={#MyAppName}
VersionInfoVersion={#MyAppVersion}.0
VersionInfoCompany={#MyAppPublisher}
VersionInfoDescription={#MyAppDescription}
VersionInfoProductName={#MyAppName}
VersionInfoProductVersion={#MyAppVersion}

[Languages]
Name: "turkish"; MessagesFile: "compiler:Languages\Turkish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
; Ana uygulama
Source: "..\Defter2Fis.ForMikro\bin\Release\Defter2Fis.ForMikro.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Defter2Fis.ForMikro\bin\Release\Defter2Fis.ForMikro.exe.config"; DestDir: "{app}"; Flags: ignoreversion
; Bağımlılıklar
Source: "..\Defter2Fis.ForMikro\bin\Release\Krypton.Toolkit.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Defter2Fis.ForMikro\bin\Release\Microsoft.Data.ConnectionUI.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Defter2Fis.ForMikro\bin\Release\Microsoft.Data.ConnectionUI.Dialog.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Defter2Fis.ForMikro\bin\Release\Microsoft.Data.SqlClient.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Defter2Fis.ForMikro\bin\Release\Microsoft.Data.SqlClient.SNI.x86.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\Defter2Fis.ForMikro\bin\Release\Microsoft.Data.SqlClient.SNI.x64.dll"; DestDir: "{app}"; Flags: ignoreversion
; PDB (debug sembolü - opsiyonel, hata raporlama için faydalı)
Source: "..\Defter2Fis.ForMikro\bin\Release\Defter2Fis.ForMikro.pdb"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{#MyAppName} Kaldır"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
function IsDotNetFramework48Installed: Boolean;
var
  Release: Cardinal;
begin
  Result := False;
  if RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full', 'Release', Release) then
  begin
    // .NET Framework 4.8 = 528040 (Windows 10 May 2019+) veya 528049 (diğer)
    Result := (Release >= 528040);
  end;
end;

function InitializeSetup: Boolean;
begin
  Result := True;
  if not IsDotNetFramework48Installed then
  begin
    MsgBox('.NET Framework 4.8 yüklü değil.' + #13#10 + #13#10 +
           'Lütfen önce .NET Framework 4.8 Runtime indirip kurun:' + #13#10 +
           'https://dotnet.microsoft.com/download/dotnet-framework/net48', mbError, MB_OK);
    Result := False;
  end;
end;
