; Script do Inno Setup para Cuttage
; Versão: 1.1
; Self-contained .NET 8 — não requer instalação do .NET na máquina de destino

#define MyAppName "Cuttage"
#define MyAppVersion "1.1.0"
#define MyAppPublisher "Cuttage"
#define MyAppExeName "Cuttage.exe"

[Setup]
AppId={{E7F8B2C3-4D5E-6F7A-8B9C-0D1E2F3A4B5C}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile=LICENSE.txt
OutputDir=installer
OutputBaseFilename=Cuttage_Setup_v{#MyAppVersion}
SetupIconFile=Resources\video-cutter-icon.ico
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=lowest
ArchitecturesInstallIn64BitMode=x64
ArchitecturesAllowed=x64
; Windows 10 1809+ (mínimo para .NET 8)
MinVersion=10.0.17763

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
; Executável principal (self-contained — .NET 8 embutido)
Source: "publish\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion

; DLLs nativas necessárias pelo WinForms/.NET
Source: "publish\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "publish\*.config"; DestDir: "{app}"; Flags: ignoreversion

; FFmpeg — colocado em subpasta ffmpeg\ para o app encontrar automaticamente
Source: "ffmpeg\ffmpeg.exe"; DestDir: "{app}\ffmpeg"; Flags: ignoreversion

; Documentação
Source: "LICENSE.txt"; DestDir: "{app}"; Flags: ignoreversion isreadme

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: filesandordirs; Name: "{app}"

[Code]
function InitializeSetup(): Boolean;
begin
  Result := True;

  if not IsWin64 then
  begin
    MsgBox('Cuttage requer Windows 64-bit (x64).', mbError, MB_OK);
    Result := False;
  end;
end;

[CustomMessages]
brazilianportuguese.LaunchProgram=Executar %1
brazilianportuguese.CreateDesktopIcon=Criar ícone na área de trabalho
brazilianportuguese.UninstallProgram=Desinstalar %1

[Messages]
brazilianportuguese.WelcomeLabel1=Bem-vindo ao instalador do [name]
brazilianportuguese.WelcomeLabel2=Este programa instalará o [name/ver] em seu computador.%n%nO .NET 8 já está incluído — nenhuma instalação adicional é necessária.%n%nÉ recomendado fechar outros aplicativos antes de continuar.
brazilianportuguese.WizardLicense=Contrato de Licença
brazilianportuguese.LicenseLabel=Leia o Contrato de Licença antes de continuar.
brazilianportuguese.LicenseLabel3=Leia o Contrato de Licença antes de continuar.
brazilianportuguese.LicenseAccepted=&Aceito o contrato
brazilianportuguese.LicenseNotAccepted=&Não aceito o contrato