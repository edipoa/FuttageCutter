@echo off
setlocal enabledelayedexpansion

echo ========================================
echo  Futtage Cutter - Build do Instalador
echo  .NET 8.0 Version
echo ========================================
echo.

REM Definir variáveis
set PROJECT_NAME=Cuttage
set BUILD_CONFIG=Release
set TARGET_FRAMEWORK=net8.0-windows
set PUBLISH_DIR=bin\%BUILD_CONFIG%\%TARGET_FRAMEWORK%\publish
set INSTALLER_DIR=installer

echo [1/6] Limpando builds anteriores...
if exist "%PUBLISH_DIR%" rmdir /s /q "%PUBLISH_DIR%"
if exist "%INSTALLER_DIR%" rmdir /s /q "%INSTALLER_DIR%"

echo [2/6] Verificando .NET 8.0...
dotnet --version >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo ERRO: .NET CLI não encontrado!
    echo Instale o .NET 8.0 SDK de: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

echo Versão do .NET detectada:
dotnet --version

echo [3/6] Restaurando pacotes NuGet...
dotnet restore
if %ERRORLEVEL% neq 0 (
    echo ERRO: Falha ao restaurar pacotes!
    pause
    exit /b 1
)

echo [4/6] Compilando o projeto...
dotnet build -c %BUILD_CONFIG% -f %TARGET_FRAMEWORK%
if %ERRORLEVEL% neq 0 (
    echo ERRO: Falha na compilação!
    pause
    exit /b 1
)

echo [5/6] Publicando aplicação...
dotnet publish -c %BUILD_CONFIG% -f %TARGET_FRAMEWORK% --self-contained false -p:PublishSingleFile=false
if %ERRORLEVEL% neq 0 (
    echo ERRO: Falha na publicação!
    pause
    exit /b 1
)

echo [6/6] Verificando arquivos necessários...

REM Verificar se o executável foi criado
if not exist "%PUBLISH_DIR%\%PROJECT_NAME%.exe" (
    echo ERRO: Executável não encontrado em %PUBLISH_DIR%!
    pause
    exit /b 1
)

echo Arquivos publicados encontrados:
dir "%PUBLISH_DIR%" /b

REM Verificar se o FFmpeg existe (opcional)
if not exist "ffmpeg\ffmpeg.exe" (
    echo.
    echo AVISO: ffmpeg.exe não encontrado na pasta ffmpeg\
    echo Você pode:
    echo 1. Baixar o FFmpeg de https://www.gyan.dev/ffmpeg/builds/
    echo 2. Criar a pasta ffmpeg\ e colocar o ffmpeg.exe lá
    echo 3. Ou continuar sem incluir o FFmpeg no instalador
    echo.
    set /p choice="Continuar sem FFmpeg? (s/n): "
    if /i "!choice!" neq "s" (
        echo.
        echo Para incluir FFmpeg:
        echo 1. Crie a pasta 'ffmpeg' na raiz do projeto
        echo 2. Baixe FFmpeg de https://www.gyan.dev/ffmpeg/builds/
        echo 3. Extraia ffmpeg.exe e ffprobe.exe para a pasta 'ffmpeg'
        echo 4. Execute este script novamente
        pause
        exit /b 1
    )
) else (
    echo FFmpeg encontrado: ffmpeg\ffmpeg.exe
)

REM Verificar se o script do Inno Setup existe
if not exist "FuttageCutter.iss" (
    echo ERRO: Arquivo FuttageCutter.iss não encontrado!
    echo Certifique-se de que o script do Inno Setup está na pasta raiz do projeto.
    pause
    exit /b 1
)

echo [7/7] Compilando instalador com Inno Setup...

REM Tentar encontrar o Inno Setup Compiler
set INNO_COMPILER=""
if exist "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" (
    set INNO_COMPILER="C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
) else if exist "C:\Program Files\Inno Setup 6\ISCC.exe" (
    set INNO_COMPILER="C:\Program Files\Inno Setup 6\ISCC.exe"
) else if exist "C:\Program Files (x86)\Inno Setup 5\ISCC.exe" (
    set INNO_COMPILER="C:\Program Files (x86)\Inno Setup 5\ISCC.exe"
) else if exist "C:\Program Files\Inno Setup 5\ISCC.exe" (
    set INNO_COMPILER="C:\Program Files\Inno Setup 5\ISCC.exe"
) else (
    echo ERRO: Inno Setup Compiler não encontrado!
    echo.
    echo Procurado em:
    echo - C:\Program Files (x86)\Inno Setup 6\ISCC.exe
    echo - C:\Program Files\Inno Setup 6\ISCC.exe
    echo - C:\Program Files (x86)\Inno Setup 5\ISCC.exe
    echo - C:\Program Files\Inno Setup 5\ISCC.exe
    echo.
    echo Instale o Inno Setup de: https://jrsoftware.org/isinfo.php
    pause
    exit /b 1
)

echo Usando Inno Setup Compiler: %INNO_COMPILER%

REM Compilar o instalador
%INNO_COMPILER% "FuttageCutter.iss"
if %ERRORLEVEL% neq 0 (
    echo ERRO: Falha ao compilar o instalador!
    echo Verifique o arquivo FuttageCutter.iss para possíveis erros.
    pause
    exit /b 1
)

echo.
echo ========================================
echo  BUILD CONCLUÍDO COM SUCESSO!
echo ========================================
echo.
echo Resumo do build:
echo - Target Framework: %TARGET_FRAMEWORK%
echo - Configuração: %BUILD_CONFIG%
echo - Aplicação: %PUBLISH_DIR%\%PROJECT_NAME%.exe
if exist "ffmpeg\ffmpeg.exe" (
    echo - FFmpeg: Incluído
) else (
    echo - FFmpeg: Não incluído
)
echo - Instalador: %INSTALLER_DIR%\FuttageCutter_Setup_v1.0.0.exe
echo.

REM Verificar se o instalador foi criado
if exist "%INSTALLER_DIR%\FuttageCutter_Setup_v1.0.0.exe" (
    echo ✅ Instalador criado com sucesso!
    echo.
    echo Tamanho do instalador:
    for %%I in ("%INSTALLER_DIR%\FuttageCutter_Setup_v1.0.0.exe") do echo %%~zI bytes
    echo.
    echo Deseja abrir a pasta do instalador?
    set /p choice="(s/n): "
    if /i "!choice!"=="s" (
        explorer "%INSTALLER_DIR%"
    )
    echo.
    echo Deseja testar o instalador agora?
    set /p choice="(s/n): "
    if /i "!choice!"=="s" (
        start "" "%INSTALLER_DIR%\FuttageCutter_Setup_v1.0.0.exe"
    )
) else (
    echo ❌ ERRO: Instalador não foi criado!
    echo Verifique os logs acima para possíveis erros.
)

echo.
echo Pressione qualquer tecla para sair...
pause >nul@echo off
setlocal enabledelayedexpansion

echo ========================================
echo  Futtage Cutter - Build do Instalador
echo ========================================
echo.

REM Definir variáveis
set PROJECT_NAME=Cuttage
set BUILD_CONFIG=Release
set TARGET_FRAMEWORK=net6.0-windows
set PUBLISH_DIR=bin\%BUILD_CONFIG%\%TARGET_FRAMEWORK%\publish
set INSTALLER_DIR=installer

echo [1/6] Limpando builds anteriores...
if exist "%PUBLISH_DIR%" rmdir /s /q "%PUBLISH_DIR%"
if exist "%INSTALLER_DIR%" rmdir /s /q "%INSTALLER_DIR%"

echo [2/6] Restaurando pacotes NuGet...
dotnet restore
if %ERRORLEVEL% neq 0 (
    echo ERRO: Falha ao restaurar pacotes!
    pause
    exit /b 1
)

echo [3/6] Compilando o projeto...
dotnet build -c %BUILD_CONFIG%
if %ERRORLEVEL% neq 0 (
    echo ERRO: Falha na compilação!
    pause
    exit /b 1
)

echo [4/6] Publicando aplicação...
dotnet publish -c %BUILD_CONFIG% -f %TARGET_FRAMEWORK% --self-contained false -p:PublishSingleFile=false
if %ERRORLEVEL% neq 0 (
    echo ERRO: Falha na publicação!
    pause
    exit /b 1
)

echo [5/6] Verificando arquivos necessários...

REM Verificar se o executável foi criado
if not exist "%PUBLISH_DIR%\%PROJECT_NAME%.exe" (
    echo ERRO: Executável não encontrado em %PUBLISH_DIR%!
    pause
    exit /b 1
)

REM Verificar se o FFmpeg existe (opcional)
if not exist "ffmpeg\ffmpeg.exe" (
    echo AVISO: ffmpeg.exe não encontrado na pasta ffmpeg\
    echo Você pode:
    echo 1. Baixar o FFmpeg de https://ffmpeg.org/download.html
    echo 2. Criar a pasta ffmpeg\ e colocar o ffmpeg.exe lá
    echo 3. Ou continuar sem incluir o FFmpeg no instalador
    echo.
    set /p choice="Continuar sem FFmpeg? (s/n): "
    if /i "!choice!" neq "s" (
        pause
        exit /b 1
    )
)

REM Verificar se o script do Inno Setup existe
if not exist "FuttageCutter.iss" (
    echo ERRO: Arquivo FuttageCutter.iss não encontrado!
    echo Certifique-se de que o script do Inno Setup está na pasta raiz do projeto.
    pause
    exit /b 1
)

echo [6/6] Compilando instalador com Inno Setup...

REM Tentar encontrar o Inno Setup Compiler
set INNO_COMPILER=""
if exist "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" (
    set INNO_COMPILER="C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
) else if exist "C:\Program Files\Inno Setup 6\ISCC.exe" (
    set INNO_COMPILER="C:\Program Files\Inno Setup 6\ISCC.exe"
) else (
    echo ERRO: Inno Setup Compiler não encontrado!
    echo Instale o Inno Setup de: https://jrsoftware.org/isinfo.php
    echo Ou ajuste o caminho no script.
    pause
    exit /b 1
)

REM Compilar o instalador
%INNO_COMPILER% "FuttageCutter.iss"
if %ERRORLEVEL% neq 0 (
    echo ERRO: Falha ao compilar o instalador!
    pause
    exit /b 1
)

echo.
echo ========================================
echo  BUILD CONCLUÍDO COM SUCESSO!
echo ========================================
echo.
echo Arquivos gerados:
echo - Aplicação: %PUBLISH_DIR%\
echo - Instalador: %INSTALLER_DIR%\FuttageCutter_Setup_v1.0.0.exe
echo.

REM Abrir pasta do instalador se foi criado
if exist "%INSTALLER_DIR%\FuttageCutter_Setup_v1.0.0.exe" (
    echo Deseja abrir a pasta do instalador?
    set /p choice="(s/n): "
    if /i "!choice!"=="s" (
        explorer "%INSTALLER_DIR%"
    )
)

echo.
echo Pressione qualquer tecla para sair...
pause >nul