@echo off
REM ============================================
REM Script de Geração de Postman Collection
REM Evently - OpenAPI to Postman
REM ============================================

echo.
echo ========================================
echo  Evently - Gerador Postman Collection
echo ========================================
echo.

REM Verificar se a API está rodando
echo [1/4] Verificando se a API esta rodando...
curl -s -o nul -w "%%{http_code}" http://localhost:5050/openapi/v1.json > temp_status.txt
set /p STATUS=<temp_status.txt
del temp_status.txt

if not "%STATUS%"=="200" (
    echo [ERRO] API nao esta respondendo em http://localhost:5050
    echo.
    echo Por favor, inicie a API primeiro:
    echo   docker-compose up -d
    echo.
    pause
    exit /b 1
)
echo [OK] API respondendo na porta 5050
echo.

REM Criar pasta para collections
if not exist "postman" mkdir postman

echo [2/4] Baixando especificacao OpenAPI...
curl -s http://localhost:5050/openapi/v1.json -o postman/evently-openapi.json
echo [OK] OpenAPI salvo em postman/evently-openapi.json
echo.

echo [3/4] Convertendo OpenAPI para Postman Collection...
npx openapi-to-postmanv2 -s postman/evently-openapi.json -o postman/Evently-API.postman_collection.json -p

if errorlevel 1 (
    echo [ERRO] Falha ao converter para Postman Collection
    echo.
    echo Instalando dependencias...
    npm install -g openapi-to-postmanv2
    echo.
    echo Tente executar o script novamente.
    pause
    exit /b 1
)
echo [OK] Collection gerada: postman/Evently-API.postman_collection.json
echo.

echo [4/4] Criando Environment do Postman...
(
echo {
echo   "id": "evently-dev",
echo   "name": "Evently - Development",
echo   "values": [
echo     {
echo       "key": "baseUrl",
echo       "value": "http://localhost:5050",
echo       "enabled": true
echo     },
echo     {
echo       "key": "baseUrlHttps",
echo       "value": "https://localhost:5051",
echo       "enabled": true
echo     }
echo   ]
echo }
) > postman/Evently-Dev.postman_environment.json

echo [OK] Environment gerado: postman/Evently-Dev.postman_environment.json
echo.

echo ========================================
echo  Sucesso! Arquivos gerados:
echo ========================================
echo.
echo  1. postman/Evently-API.postman_collection.json
echo  2. postman/Evently-Dev.postman_environment.json
echo  3. postman/evently-openapi.json
echo.
echo ========================================
echo  Como usar:
echo ========================================
echo.
echo  1. Abra o Postman
echo  2. Import ^> File
echo  3. Selecione ambos arquivos .json
echo  4. Use o environment "Evently - Development"
echo.
pause
