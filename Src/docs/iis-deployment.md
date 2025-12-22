---
sidebar_position: 4
---

# Executar API com IIS

Guia completo para hospedar a API Evently no IIS do Windows.

## 📋 Pré-requisitos

### 1. Windows com IIS

- ✅ Windows Server 2016+
- ✅ Windows 10/11 Pro, Enterprise ou Education
- ❌ Windows Home (não suporta IIS)

### 2. Instalar IIS

**Painel de Controle → Programas → Ativar recursos do Windows:**

```
✅ Serviços de Informações da Internet
   ✅ Serviços da World Wide Web
      ✅ Recursos de Desenvolvimento de Aplicativos
         ✅ WebSockets Protocol
      ✅ Recursos Comuns de HTTP
         ✅ Documento Padrão
         ✅ Conteúdo Estático
      ✅ Integridade e Diagnóstico
         ✅ Log de HTTP
   ✅ Ferramentas de Gerenciamento da Web
      ✅ Console de Gerenciamento do IIS
```

### 3. ASP.NET Core Hosting Bundle

**CRÍTICO** - Baixe e instale:

https://dotnet.microsoft.com/download/dotnet/10.0

Procure por **"Hosting Bundle"** → Baixe → Instale → **Reinicie o PC**

Verificar:
```powershell
dotnet --info
# Deve mostrar runtime 10.0.x
```

## 🚀 Passo 1: Publicar Aplicação

### Via Visual Studio

1. Botão direito em `Evently.Api` → **Publish**
2. Target: **Folder**
3. Caminho: `C:\inetpub\wwwroot\EventlyApi`
4. **Publish**

### Via CLI

```powershell
cd Src\Api\Evently.Api
dotnet publish -c Release -o C:\inetpub\wwwroot\EventlyApi
```

## 🚀 Passo 2: Configurar Connection String

Edite `C:\inetpub\wwwroot\EventlyApi\appsettings.json`:

```json
{
  "ConnectionStrings": {
    "EventsDatabase": "Host=localhost;Port=5432;Database=evently;Username=postgres;Password=C@151867"
  }
}
```

## 🚀 Passo 3: Criar Application Pool

1. Abra **Gerenciador do IIS** (`Win+R` → `inetmgr`)
2. **Application Pools** → **Add Application Pool**
3. Configure:
   - Name: `EventlyApiPool`
   - .NET CLR version: **No Managed Code**
   - Managed pipeline: Integrated
4. **OK**

**Advanced Settings:**
- Identity: ApplicationPoolIdentity
- Load User Profile: **True**

## 🚀 Passo 4: Criar Website

1. **Sites** → **Add Website**
2. Configure:
   - Site name: `EventlyApi`
   - Application pool: `EventlyApiPool`
   - Physical path: `C:\inetpub\wwwroot\EventlyApi`
   - Port: `5050`
3. **OK**

## 🚀 Passo 5: Permissões

1. `C:\inetpub\wwwroot\EventlyApi` → Propriedades → Segurança
2. Adicionar usuário: `IIS_IUSRS`
3. Permissões:
   - ✅ Read & Execute
   - ✅ List folder contents
   - ✅ Read

## 🚀 Passo 6: Iniciar Site

No IIS Manager:
1. Selecione `EventlyApi`
2. **Start** (painel direito)

## ✅ Testar

Navegador:
```
http://localhost:5050/events
```

PowerShell:
```powershell
curl http://localhost:5050/events
```

## 🐛 Troubleshooting

### Erro 500.19

**Causa:** ASP.NET Core Module não instalado

**Solução:** 
1. Instalar Hosting Bundle
2. Reiniciar PC
3. `iisreset` no PowerShell

### Erro 500.30

**Causa:** Aplicação falhou ao iniciar

**Solução:**
1. Verificar logs: `C:\inetpub\wwwroot\EventlyApi\logs\`
2. Verificar connection string
3. Testar conexão PostgreSQL:
   ```powershell
   Test-NetConnection localhost -Port 5432
   ```

### Site não inicia

```powershell
# PowerShell como Admin
Import-Module WebAdministration
Get-WebAppPoolState -Name "EventlyApiPool"
Start-WebAppPool -Name "EventlyApiPool"
```

## 📊 Verificar Status

```powershell
Import-Module WebAdministration
Get-WebsiteState -Name "EventlyApi"
Get-WebAppPoolState -Name "EventlyApiPool"
```

## 🔧 web.config

O arquivo é gerado automaticamente, mas você pode personalizá-lo:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet" 
                  arguments=".\Evently.Api.dll" 
                  stdoutLogEnabled="true" 
                  stdoutLogFile=".\logs\stdout" 
                  hostingModel="inprocess">
        <environmentVariables>
          <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
        </environmentVariables>
      </aspNetCore>
    </system.webServer>
  </location>
</configuration>
```

Criar pasta logs:
```powershell
mkdir C:\inetpub\wwwroot\EventlyApi\logs
```

## 🔐 Firewall (Opcional)

```powershell
# PowerShell como Admin
New-NetFirewallRule -DisplayName "Evently API HTTP" -Direction Inbound -LocalPort 5050 -Protocol TCP -Action Allow
```

## ✅ Checklist

- [ ] IIS instalado
- [ ] Hosting Bundle instalado e PC reiniciado
- [ ] Aplicação publicada
- [ ] Connection string configurada
- [ ] Application Pool criado (No Managed Code)
- [ ] Website criado
- [ ] Permissões IIS_IUSRS configuradas
- [ ] Site iniciado
- [ ] Teste HTTP funcionando

## 📚 Recursos

- [Host ASP.NET Core on IIS](https://learn.microsoft.com/aspnet/core/host-and-deploy/iis/)
- [ASP.NET Core Module](https://learn.microsoft.com/aspnet/core/host-and-deploy/aspnet-core-module)

**Pronto! Sua API está rodando no IIS! 🎉**
