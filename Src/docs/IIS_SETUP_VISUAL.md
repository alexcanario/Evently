# 🎯 IIS Setup - Guia Visual Rápido

## 📊 Fluxo Completo

```
┌─────────────────────────────────────────────────────────────┐
│  1. PREPARAÇÃO (Uma Vez)                                    │
├─────────────────────────────────────────────────────────────┤
│  ✅ IIS instalado                                           │
│  ✅ Hosting Bundle instalado + PC reiniciado                │
│  ✅ PostgreSQL rodando (Docker/Local/Remoto)                │
└─────────────────────────────────────────────────────────────┘
                           ↓
┌─────────────────────────────────────────────────────────────┐
│  2. ALTERAÇÕES NO CÓDIGO (JÁ FEITO ✅)                      │
├─────────────────────────────────────────────────────────────┤
│  ✅ Program.cs atualizado                                   │
│  ✅ appsettings.Production.json criado                      │
└─────────────────────────────────────────────────────────────┘
                           ↓
┌─────────────────────────────────────────────────────────────┐
│  3. DEPLOY                                                  │
├─────────────────────────────────────────────────────────────┤
│  Opção A: .\deploy-to-iis.ps1                              │
│  Opção B: dotnet publish + IIS Manager                     │
└─────────────────────────────────────────────────────────────┘
                           ↓
┌─────────────────────────────────────────────────────────────┐
│  4. TESTE                                                   │
├─────────────────────────────────────────────────────────────┤
│  http://localhost:5050/scalar/v1                            │
│  http://localhost:5050/events                               │
└─────────────────────────────────────────────────────────────┘
```

---

## ⚙️ Alterações Feitas no Código

### Program.cs

```diff
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

+ // NOVO: Integração com IIS
+ builder.WebHost.UseIISIntegration();

builder.Services.AddOpenApi();
// ... resto do código ...

WebApplication app = builder.Build();

+ // NOVO: Forwarded Headers
+ app.UseForwardedHeaders();

app.UseRequestLocalization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.ApplyMigrations();
    app.MapScalarApiReference();
}

+ // NOVO: OpenAPI e Scalar em Production
+ if (app.Environment.IsProduction())
+ {
+     app.MapOpenApi();
+     app.MapScalarApiReference();
+     
+     var applyMigrationsInProduction = builder.Configuration.GetValue<bool>("ApplyMigrationsOnStartup", false);
+     if (applyMigrationsInProduction)
+     {
+         app.ApplyMigrations();
+     }
+ }

EventsModuleConfig.MapEndpoints(app);
```

---

### appsettings.Production.json (NOVO)

```json
{
  "ConnectionStrings": {
    "EventsDatabase": "Host=localhost;Port=5432;Database=evently;Username=postgres;Password=C@151867"
  },
  "ApplyMigrationsOnStartup": true,
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

---

## 🐘 PostgreSQL - Escolha Uma Opção

### Opção 1: Docker (Mais Rápido)

```powershell
docker run -d `
  --name postgres-evently `
  -e POSTGRES_PASSWORD=C@151867 `
  -e POSTGRES_DB=evently `
  -p 5432:5432 `
  postgres:latest
```

### Opção 2: Docker Compose

```powershell
docker-compose up -d evently.database
```

### Opção 3: Instalação Local

- Download: https://www.postgresql.org/download/windows/
- Porta: 5432
- Password: C@151867
- Database: evently

---

## 🚀 Deploy - Escolha Uma Opção

### Opção 1: Script Automático ⭐

```powershell
# PowerShell como Administrador
.\deploy-to-iis.ps1
```

**Faz:**
- Para site (se existir)
- Publica aplicação
- Cria Application Pool
- Cria Website
- Inicia site

### Opção 2: Manual

```powershell
# 1. Publicar
cd Src\Api\Evently.Api
dotnet publish -c Release -o C:\inetpub\wwwroot\EventlyApi

# 2. Configurar IIS
# - Abrir IIS Manager
# - Criar Application Pool (No Managed Code)
# - Criar Website (porta 5050)
# - Iniciar site
```

---

## ✅ Teste

```bash
# PowerShell
curl http://localhost:5050/events

# Navegador
http://localhost:5050/scalar/v1
```

---

## 🐛 Problemas Comuns

### ❌ Erro 500.19

```powershell
# Reinstalar Hosting Bundle
# https://dotnet.microsoft.com/download/dotnet/10.0
# Reiniciar PC
iisreset
```

### ❌ Erro 500.30 (Migration Failed)

```powershell
# Verificar PostgreSQL
Test-NetConnection localhost -Port 5432

# Aplicar migrations manualmente
cd Src\Api\Evently.Api
dotnet ef database update --project ..\..\Modules\Events\Evently.Modules.Events.Api\Evently.Modules.Events.Api.csproj
```

### ❌ Site não inicia

```powershell
# Verificar status
Get-WebsiteState -Name EventlyApi
Get-WebAppPoolState -Name EventlyApiPool

# Iniciar
Start-WebAppPool -Name EventlyApiPool
Start-WebSite -Name EventlyApi

# Ver logs
Get-Content C:\inetpub\wwwroot\EventlyApi\logs\stdout_*.log -Wait
```

---

## 📁 Estrutura de Arquivos

```
Evently/
├── Src/Api/Evently.Api/
│   ├── Program.cs                      ← ⚙️ MODIFICADO
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── appsettings.Production.json     ← ✨ NOVO
│
├── deploy-to-iis.ps1                   ← 🚀 Script deploy
├── CONFIGURACOES_IIS.md                ← 📖 Guia configuração
├── ALTERACOES_RESUMO.md                ← 📋 Resumo alterações
├── IIS_DEPLOY_README.md                ← 📘 README deploy
└── IIS_QUICK_REFERENCE.md              ← ⚡ Referência rápida
```

---

## 📚 Documentação

| Arquivo | Para Quê |
|---------|----------|
| **CONFIGURACOES_IIS.md** | 📖 Guia completo de configuração |
| **ALTERACOES_RESUMO.md** | 📋 Resumo das mudanças |
| **IIS_DEPLOY_README.md** | 📘 Overview do deploy |
| **IIS_QUICK_REFERENCE.md** | ⚡ Comandos rápidos |
| **deploy-to-iis.ps1** | 🚀 Script automático |

---

## 🎯 Comandos Essenciais

```powershell
# Deploy
.\deploy-to-iis.ps1

# Status
Get-WebsiteState -Name EventlyApi

# Logs
Get-Content C:\inetpub\wwwroot\EventlyApi\logs\stdout_*.log -Wait

# Parar
Stop-WebSite -Name EventlyApi

# Iniciar
Start-WebSite -Name EventlyApi

# Reciclar
Restart-WebAppPool -Name EventlyApiPool
```

---

## 🔄 Workflow Diário

```
1. Fazer mudanças no código
   ↓
2. Executar: .\deploy-to-iis.ps1
   ↓
3. Testar: http://localhost:5050/scalar/v1
   ↓
4. Ver logs (se houver problema)
```

---

**Pronto! Consulte [CONFIGURACOES_IIS.md](CONFIGURACOES_IIS.md) para detalhes! 🚀**
