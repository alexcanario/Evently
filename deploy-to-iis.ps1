# ========================================
# Deploy Evently API para IIS
# ========================================

param(
    [string]$ProjectPath = "$PSScriptRoot\Src\Api\Evently.Api",
    [string]$PublishPath = "C:\inetpub\wwwroot\EventlyApi",
    [string]$SiteName = "EventlyApi",
    [string]$AppPoolName = "EventlyApiPool"
)

# Requer Admin
if (-NOT ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) {
    Write-Error "Execute como Administrador!"
    exit 1
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host " Deploy Evently API para IIS" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

# Importar módulo IIS
Import-Module WebAdministration -ErrorAction Stop

# 1. Parar o site
Write-Host "[1/6] Parando site IIS..." -ForegroundColor Yellow
try {
    if (Test-Path "IIS:\Sites\$SiteName") {
        Stop-WebSite -Name $SiteName -ErrorAction SilentlyContinue
    }
    if (Test-Path "IIS:\AppPools\$AppPoolName") {
        Stop-WebAppPool -Name $AppPoolName -ErrorAction SilentlyContinue
    }
    Start-Sleep -Seconds 3
    Write-Host "   OK - Site parado" -ForegroundColor Green
} catch {
    Write-Host "   AVISO - Site pode nao existir ainda" -ForegroundColor DarkYellow
}

# 2. Publicar aplicação
Write-Host "[2/6] Publicando aplicacao..." -ForegroundColor Yellow
Push-Location $ProjectPath
try {
    dotnet publish -c Release -o $PublishPath --nologo
    if ($LASTEXITCODE -ne 0) { throw "Erro ao publicar" }
    Write-Host "   OK - Aplicacao publicada em $PublishPath" -ForegroundColor Green
} catch {
    Write-Error "Erro ao publicar aplicacao: $_"
    Pop-Location
    exit 1
}
Pop-Location

# 3. Criar pasta de logs
Write-Host "[3/6] Criando pasta de logs..." -ForegroundColor Yellow
$LogPath = Join-Path $PublishPath "logs"
if (-not (Test-Path $LogPath)) {
    New-Item -ItemType Directory -Path $LogPath -Force | Out-Null
}
Write-Host "   OK - Pasta de logs criada" -ForegroundColor Green

# 4. Configurar Application Pool
Write-Host "[4/6] Configurando Application Pool..." -ForegroundColor Yellow
if (-not (Test-Path "IIS:\AppPools\$AppPoolName")) {
    New-WebAppPool -Name $AppPoolName | Out-Null
    Write-Host "   OK - Application Pool criado" -ForegroundColor Green
} else {
    Write-Host "   OK - Application Pool ja existe" -ForegroundColor Green
}

Set-ItemProperty "IIS:\AppPools\$AppPoolName" -Name "managedRuntimeVersion" -Value ""
Set-ItemProperty "IIS:\AppPools\$AppPoolName" -Name "enable32BitAppOnWin64" -Value $false
Set-ItemProperty "IIS:\AppPools\$AppPoolName" -Name "processModel.loadUserProfile" -Value $true

# 5. Configurar Website
Write-Host "[5/6] Configurando Website..." -ForegroundColor Yellow
if (-not (Test-Path "IIS:\Sites\$SiteName")) {
    New-Website -Name $SiteName `
                -PhysicalPath $PublishPath `
                -ApplicationPool $AppPoolName `
                -Port 5050 | Out-Null
    Write-Host "   OK - Website criado" -ForegroundColor Green
} else {
    Set-ItemProperty "IIS:\Sites\$SiteName" -Name "physicalPath" -Value $PublishPath
    Set-ItemProperty "IIS:\Sites\$SiteName" -Name "applicationPool" -Value $AppPoolName
    Write-Host "   OK - Website atualizado" -ForegroundColor Green
}

# 6. Iniciar o site
Write-Host "[6/6] Iniciando site..." -ForegroundColor Yellow
Start-WebAppPool -Name $AppPoolName
Start-Sleep -Seconds 2
Start-WebSite -Name $SiteName

$status = Get-WebsiteState -Name $SiteName
Write-Host "   OK - Status do site: $($status.Value)" -ForegroundColor Green

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host " Deploy concluido com sucesso!" -ForegroundColor Green
Write-Host "========================================`n" -ForegroundColor Cyan

Write-Host "URLs de acesso:" -ForegroundColor Cyan
Write-Host "  HTTP:  http://localhost:5050" -ForegroundColor White
Write-Host "  Logs:  $LogPath`n" -ForegroundColor White

Write-Host "Comandos uteis:" -ForegroundColor Cyan
Write-Host "  Ver logs:     Get-Content $LogPath\stdout_*.log -Wait" -ForegroundColor White
Write-Host "  Parar site:   Stop-WebSite -Name $SiteName" -ForegroundColor White
Write-Host "  Iniciar site: Start-WebSite -Name $SiteName`n" -ForegroundColor White
