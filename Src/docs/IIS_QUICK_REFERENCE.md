# 🚀 IIS Deploy - Quick Reference

## ⚡ Deploy em 3 Comandos

```powershell
# 1. PowerShell como Administrador
cd D:\code\learning\_Javanovic\Evently

# 2. Executar script de deploy
.\deploy-to-iis.ps1

# 3. Testar
curl http://localhost:5050/events
```

**Pronto! ✅**

---

## 📋 Pré-requisitos (Uma Vez Apenas)

### 1. Instalar IIS
**Painel de Controle → Programas → Recursos do Windows:**
- ✅ Serviços de Informações da Internet
- ✅ Console de Gerenciamento do IIS

### 2. Instalar Hosting Bundle
https://dotnet.microsoft.com/download/dotnet/10.0
- Baixe "Hosting Bundle"
- Instale
- **REINICIE O PC** ⚠️

### 3. PostgreSQL Rodando
```powershell
# Se usar Docker:
docker-compose up -d evently.database

# Ou instalar PostgreSQL localmente
```

---

## 🔧 Primeiro Deploy

### Opção A: Script Automático ⭐

```powershell
# Como Administrador
.\deploy-to-iis.ps1
```

### Opção B: Manual

```powershell
# 1. Publicar
cd Src\Api\Evently.Api
dotnet publish -c Release -o C:\inetpub\wwwroot\EventlyApi

# 2. Editar connection string
notepad C:\inetpub\wwwroot\EventlyApi\appsettings.json

# 3. Criar Application Pool (IIS Manager)
# - Nome: EventlyApiPool
# - .NET CLR: No Managed Code

# 4. Criar Website (IIS Manager)
# - Nome: EventlyApi
# - Porta: 5050
# - Pool: EventlyApiPool
# - Caminho: C:\inetpub\wwwroot\EventlyApi

# 5. Iniciar
Start-WebSite -Name EventlyApi
```

---

## 🔄 Atualizar Deploy

Quando fizer mudanças no código:

```powershell
# Automático
.\deploy-to-iis.ps1

# Manual
cd Src\Api\Evently.Api
dotnet publish -c Release -o C:\inetpub\wwwroot\EventlyApi
Restart-WebAppPool -Name EventlyApiPool
```

---

## ✅ Testar

### Navegador
```
http://localhost:5050/scalar/v1
http://localhost:5050/events
```

### PowerShell
```powershell
# Listar eventos
curl http://localhost:5050/events

# Criar evento
$body = @{
    title = "Teste IIS"
    description = "Evento de teste"
    location = "Virtual"
    startsAtUtc = "2024-12-31T18:00:00Z"
    endsAtUtc = "2024-12-31T20:00:00Z"
} | ConvertTo-Json

Invoke-RestMethod -Method Post -Uri http://localhost:5050/events -Body $body -ContentType "application/json"
```

---

## 🐛 Solução Rápida de Problemas

### Erro 500.19
```powershell
# Reinstalar Hosting Bundle
# Baixar de: https://dotnet.microsoft.com/download/dotnet/10.0
# Reiniciar PC
iisreset
```

### Erro 500.30
```powershell
# Ver logs
Get-Content C:\inetpub\wwwroot\EventlyApi\logs\stdout_*.log -Wait

# Verificar connection string
notepad C:\inetpub\wwwroot\EventlyApi\appsettings.json

# Testar PostgreSQL
Test-NetConnection localhost -Port 5432
```

### Site não responde
```powershell
# Verificar status
Get-WebsiteState -Name EventlyApi
Get-WebAppPoolState -Name EventlyApiPool

# Reiniciar
Restart-WebAppPool -Name EventlyApiPool
Start-WebSite -Name EventlyApi
```

### Erro de permissão
```powershell
# Dar permissões ao IIS
$path = "C:\inetpub\wwwroot\EventlyApi"
$acl = Get-Acl $path
$rule = New-Object System.Security.AccessControl.FileSystemAccessRule("IIS_IUSRS","ReadAndExecute","ContainerInherit,ObjectInherit","None","Allow")
$acl.SetAccessRule($rule)
Set-Acl $path $acl
```

---

## 📊 Comandos Úteis

```powershell
# Ver logs em tempo real
Get-Content C:\inetpub\wwwroot\EventlyApi\logs\stdout_*.log -Wait

# Status
Get-WebsiteState -Name EventlyApi
Get-WebAppPoolState -Name EventlyApiPool

# Controle
Start-WebSite -Name EventlyApi
Stop-WebSite -Name EventlyApi
Restart-WebAppPool -Name EventlyApiPool

# Listar sites
Get-Website
Get-WebAppPool

# Remover (se necessário)
Remove-Website -Name EventlyApi
Remove-WebAppPool -Name EventlyApiPool
```

---

## 📁 Estrutura de Arquivos

```
C:\inetpub\wwwroot\EventlyApi\
├── Evently.Api.dll           ← Aplicação principal
├── appsettings.json          ← ⚙️ EDITAR connection string
├── appsettings.Production.json
├── web.config                ← Gerado automaticamente
├── logs\                     ← 📝 Logs da aplicação
│   └── stdout_*.log
└── wwwroot\
```

---

## 🔐 Connection String

Edite `C:\inetpub\wwwroot\EventlyApi\appsettings.json`:

```json
{
  "ConnectionStrings": {
    "EventsDatabase": "Host=localhost;Port=5432;Database=evently;Username=postgres;Password=SuaSenha"
  }
}
```

**Opções:**

| Cenário | Connection String |
|---------|------------------|
| PostgreSQL local | `Host=localhost;Port=5432;Database=evently;Username=postgres;Password=senha` |
| PostgreSQL remoto | `Host=192.168.1.100;Port=5432;Database=evently;Username=postgres;Password=senha` |
| PostgreSQL Docker (mesma máquina) | `Host=host.docker.internal;Port=5432;Database=evently;Username=postgres;Password=senha` |

---

## 🌐 Acessar de Outras Máquinas

### 1. Abrir Firewall

```powershell
# Como Administrador
New-NetFirewallRule -DisplayName "Evently API" -Direction Inbound -LocalPort 5050 -Protocol TCP -Action Allow
```

### 2. Acessar

De outra máquina na rede:
```
http://<IP-DO-SERVIDOR>:5050/events
```

Descobrir IP:
```powershell
ipconfig
# Procure por "IPv4 Address"
```

---

## 📚 Guia Completo

Para instruções detalhadas:

- **Guia Completo:** [docs/docs/guides/iis-deployment.md](docs/docs/guides/iis-deployment.md)
- **README Deploy:** [IIS_DEPLOY_README.md](IIS_DEPLOY_README.md)
- **Docusaurus:** http://localhost:3000/docs/guides/iis-deployment

---

## ✅ Checklist

### Primeira Vez
- [ ] IIS instalado
- [ ] Hosting Bundle instalado
- [ ] PC reiniciado
- [ ] PostgreSQL rodando
- [ ] Deploy executado
- [ ] Site testado

### Cada Deploy
- [ ] Código commitado
- [ ] `deploy-to-iis.ps1` executado
- [ ] Logs verificados
- [ ] API testada

---

**Pronto para produção! 🚀**
