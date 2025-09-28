; Script do Inno Setup para Futtage Cutter
; Versão: 1.0
; Autor: Seu Nome
; .NET 8.0 Version

#define MyAppName "Futtage Cutter"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Seu Nome"
#define MyAppURL "https://seusite.com"
#define MyAppExeName "Cuttage.exe"
#define MyAppAssocName MyAppName + " File"
#define MyAppAssocExt ".vcp"
#define MyAppAssocKey StringChange(MyAppAssocName, " ", "") + MyAppAssocExt

[Setup]
; NOTE: O valor de AppId identifica unicamente esta aplicação.
; Não use o mesmo valor AppId em instaladores para outras aplicações.
; (Para gerar um novo GUID, clique em Tools | Generate GUID dentro do IDE.)
AppId={{E7F8B2C3-4D5E-6F7A-8B9C-0D1E2F3A4B5C}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile=LICENSE.txt
InfoBeforeFile=README.txt
OutputDir=installer
OutputBaseFilename=FuttageCutter_Setup_v{#MyAppVersion}
SetupIconFile=Resources\video-cutter-icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
; Privilégios administrativos apenas se necessário
PrivilegesRequired=lowest
; Arquitetura
ArchitecturesInstallIn64BitMode=x64
; Requisitos mínimos do Windows para .NET 8
MinVersion=10.0.17763

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 6.1
Name: "associatefiles"; Description: "&Associar arquivos de projeto do Futtage Cutter"; GroupDescription: "Associações de arquivo:"; Flags: unchecked

[Files]
; Arquivos principais da aplicação (.NET 8)
Source: "bin\Release\net8.0-windows\publish\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net8.0-windows\publish\*.dll"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "bin\Release\net8.0-windows\publish\*.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net8.0-windows\publish\*.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion

; FFmpeg (se incluído na distribuição)
Source: "ffmpeg\ffmpeg.exe"; DestDir: "{app}"; Flags: ignoreversion

; Documentação
Source: "README.md"; DestDir: "{app}"; DestName: "README.txt"; Flags: ignoreversion isreadme
Source: "LICENSE.txt"; DestDir: "{app}"; DestName: "LICENSE.txt"; Flags: ignoreversion

; Ícones e recursos
Source: "Resources\*"; DestDir: "{app}\Resources"; Flags: ignoreversion recursesubdirs createallsubdirs

; NOTA: Não use "Flags: ignoreversion" em nenhum arquivo compartilhado do sistema

[Registry]
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocExt}\OpenWithProgids"; ValueType: string; ValueName: "{#MyAppAssocKey}"; ValueData: ""; Flags: uninsdeletevalue; Tasks: associatefiles
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}"; ValueType: string; ValueName: ""; ValueData: "{#MyAppAssocName}"; Flags: uninsdeletekey; Tasks: associatefiles
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppExeName},0"; Tasks: associatefiles
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""; Tasks: associatefiles
Root: HKA; Subkey: "Software\Classes\Applications\{#MyAppExeName}\SupportedTypes"; ValueType: string; ValueName: ".vcp"; ValueData: ""; Tasks: associatefiles

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: filesandordirs; Name: "{app}\Logs"
Type: filesandordirs; Name: "{app}\Temp"

[Code]
const
  NET8_URL = 'https://dotnet.microsoft.com/download/dotnet/8.0';
  NET8_DESKTOP_RUNTIME_URL = 'https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-8.0.0-windows-x64-installer';

function IsDotNet8Installed(): Boolean;
var
  ResultCode: Integer;
  Output: AnsiString;
begin
  Result := False;
  
  // Verificar se dotnet está disponível
  if not Exec('dotnet', '--list-runtimes', '', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
    Exit;
    
  if ResultCode <> 0 then
    Exit;
    
  // Verificar especificamente o .NET 8.0 Desktop Runtime
  if Exec('dotnet', '--list-runtimes', '', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
  begin
    Result := (ResultCode = 0);
    // Uma verificação mais robusta seria analisar a saída para encontrar "Microsoft.WindowsDesktop.App 8."
  end;
end;

function GetDotNet8DownloadUrl(): String;
begin
  if IsWin64 then
    Result := 'https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-8.0.0-windows-x64-installer'
  else
    Result := 'https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-8.0.0-windows-x86-installer';
end;

function InitializeSetup(): Boolean;
var
  ResultCode: Integer;
begin
  Result := True;
  
  // Verificar versão mínima do Windows (Windows 10 1809 para .NET 8)
  if not IsWin64 then
  begin
    MsgBox('Este aplicativo requer Windows 64-bit.', mbError, MB_OK);
    Result := False;
    Exit;
  end;
  
  // Verificar se .NET 8.0 Desktop Runtime está instalado
  if not IsDotNet8Installed() then
  begin
    if MsgBox('.NET 8.0 Desktop Runtime é necessário para executar este aplicativo.' + #13#10 + #13#10 +
              'Este runtime inclui todas as bibliotecas necessárias para executar aplicações Windows Forms e WPF.' + #13#10 + #13#10 +
              'Deseja abrir a página de download do .NET 8.0 Desktop Runtime?', 
              mbConfirmation, MB_YESNO) = IDYES then
    begin
      ShellExec('open', GetDotNet8DownloadUrl(), '', '', SW_SHOWNORMAL, ewNoWait, ResultCode);
    end;
    
    MsgBox('Por favor, instale o .NET 8.0 Desktop Runtime e execute o instalador novamente.', mbInformation, MB_OK);
    Result := False;
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
  ResultCode: Integer;
begin
  if CurStep = ssPostInstall then
  begin
    // Verificar se FFmpeg está funcionando
    if FileExists(ExpandConstant('{app}\ffmpeg.exe')) then
    begin
      Exec(ExpandConstant('{app}\ffmpeg.exe'), '-version', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
      if ResultCode <> 0 then
      begin
        MsgBox('Aviso: FFmpeg pode não estar funcionando corretamente. ' +
               'Verifique se todas as dependências estão instaladas.' + #13#10 + #13#10 +
               'Você pode baixar uma versão completa do FFmpeg em: https://www.gyan.dev/ffmpeg/builds/', 
               mbInformation, MB_OK);
      end;
    end else
    begin
      MsgBox('FFmpeg não foi incluído nesta instalação. ' + #13#10 +
             'Para funcionalidade completa, baixe o FFmpeg de:' + #13#10 +
             'https://www.gyan.dev/ffmpeg/builds/' + #13#10 + #13#10 +
             'E coloque o ffmpeg.exe na pasta de instalação do aplicativo.', 
             mbInformation, MB_OK);
    end;
  end;
end;

function ShouldSkipPage(PageID: Integer): Boolean;
begin
  // Pular página de seleção de componentes se não houver componentes opcionais
  Result := False;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usPostUninstall then
  begin
    // Limpar configurações de usuário se desejar
    if MsgBox('Deseja remover também as configurações de usuário?', mbConfirmation, MB_YESNO) = IDYES then
    begin
      DelTree(ExpandConstant('{userappdata}\FuttageCutter'), True, True, True);
    end;
  end;
end;

[CustomMessages]
brazilianportuguese.LaunchProgram=Executar %1
brazilianportuguese.CreateDesktopIcon=Criar ícone na área de trabalho
brazilianportuguese.CreateQuickLaunchIcon=Criar ícone na barra de inicialização rápida
brazilianportuguese.ProgramOnTheWeb=%1 na Web
brazilianportuguese.UninstallProgram=Desinstalar %1
brazilianportuguese.AssocFileExtension=&Associar %1 com a extensão de arquivo %2
brazilianportuguese.AssocingFileExtension=Associando %1 com a extensão de arquivo %2...

[Messages]
brazilianportuguese.WelcomeLabel1=Bem-vindo ao Assistente de Instalação do [name]
brazilianportuguese.WelcomeLabel2=Este programa instalará o [name/ver] em seu computador.%n%nÉ recomendado que você feche todos os outros aplicativos antes de continuar.%n%nREQUISITO: .NET 8.0 Desktop Runtime
brazilianportuguese.WizardLicense=Contrato de Licença
brazilianportuguese.LicenseLabel=Leia o seguinte Contrato de Licença. Você deve aceitar os termos deste contrato antes de continuar a instalação.
brazilianportuguese.LicenseLabel3=Leia o seguinte Contrato de Licença. Você deve aceitar os termos deste contrato antes de continuar a instalação.
brazilianportuguese.LicenseAccepted=&Aceito o contrato
brazilianportuguese.LicenseNotAccepted=&Não aceito o contrato