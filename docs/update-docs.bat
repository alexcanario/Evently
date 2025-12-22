@echo off
REM ============================================
REM Script de Atualização da Documentação
REM Evently - Docusaurus + OpenAPI
REM ============================================

echo.
echo ========================================
echo  Evently - Atualizador de Documentacao
echo ========================================
echo.

REM Verificar se está na pasta docs
if not exist "package.json" (
    echo [ERRO] Execute este script dentro da pasta 'docs'
    echo Exemplo: cd docs ^&^& update-docs.bat
    pause
    exit /b 1
)

echo [1/5] Verificando se a API esta rodando...
curl -s -o nul -w "%%{http_code}" http://localhost:5050/openapi/v1.json > temp_status.txt
set /p STATUS=<temp_status.txt
del temp_status.txt

if not "%STATUS%"=="200" (
    echo [AVISO] API nao esta respondendo em http://localhost:5050
    echo.
    echo Por favor, inicie a API primeiro:
    echo   docker-compose up -d
    echo.
    pause
    exit /b 1
)
echo [OK] API respondendo na porta 5050
echo.

echo [2/5] Baixando especificacao OpenAPI...
if not exist "static\openapi" mkdir static\openapi
curl -s http://localhost:5050/openapi/v1.json -o static/openapi/evently-api.json

if errorlevel 1 (
    echo [ERRO] Falha ao baixar OpenAPI
    pause
    exit /b 1
)
echo [OK] OpenAPI salvo em static/openapi/evently-api.json
echo.

echo [3/5] Gerando documentacao da API...
call npm run gen-api-docs

if errorlevel 1 (
    echo [ERRO] Falha ao gerar documentacao
    pause
    exit /b 1
)
echo [OK] Documentacao gerada em docs/api/
echo.

echo [4/5] Limpando cache do Docusaurus...
call npm run clear
echo [OK] Cache limpo
echo.

echo [5/5] Iniciando servidor Docusaurus...
echo.
echo ========================================
echo  Docusaurus iniciando em:
echo  http://localhost:3000
echo ========================================
echo.
echo Pressione Ctrl+C para parar o servidor
echo.

call npm start
