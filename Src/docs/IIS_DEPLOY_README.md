# 🌐 Deploy IIS - Evently API

Scripts e guias para deploy da API Evently no IIS.

## 🚀 Quick Start

### Pré-requisitos

1. **Windows com IIS instalado**
2. **ASP.NET Core 10.0 Hosting Bundle** instalado
3. **PostgreSQL** rodando (local ou remoto)

### Deploy Automático

```powershell
# Execute como Administrador
.\deploy-to-iis.ps1
```

**O script faz:**
- ✅ Para o site (se já existir)
- ✅ Publica aplicação para `C:\inetpub\wwwroot\EventlyApi`
- ✅ Cria Application Pool
- ✅ Cria Website
- ✅ Inicia o site

### Testar

```powershell
# Testar endpoint
curl http://localhost:5050/events
```

Navegador:
```
http://localhost:5050/scalar/v1
```

## 📚 Guia Completo

Para instruções detalhadas passo a passo:

**[docs/docs/guides/iis-deployment.md](docs/docs/guides/iis-deployment.md)**

Ou acesse a documentação interativa:
```
http://localhost:3000/docs/guides/iis-deployment
```

## 🔧 Customizar Deploy

Edite parâmetros no script:

```powershell
.\deploy-to-iis.ps1 `
  -ProjectPath "D:\MeuProjeto\Src\Api\Evently.Api" `
  -PublishPath "D:\Sites\EventlyApi" `
  -SiteName "MinhAPI" `
  -AppPoolName "MeuPool"
```

## 📁 Estrutura Após Deploy

```
C:\inetpub\wwwroot\EventlyApi\
├── Evently.Api.dll
├── appsettings.json          ← Editar connection string
├── appsettings.Production.json
├── web.config               ← Gerado automaticamente
├── logs\                    ← Logs da aplicação
└── wwwroot\
```

## ⚙️ Configuração Manual

Se preferir configurar manualmente, siga o guia completo em:
- **[docs/docs/guides/iis-deployment.md](docs/docs/guides/iis-deployment.md)**

## 🐛 Troubleshooting

### Site não inicia

```powershell
# Verificar status
Get-WebsiteState -Name "EventlyApi"
Get-WebAppPoolState -Name "EventlyApiPool"

# Ver logs
Get-Content C:\inetpub\wwwroot\EventlyApi\logs\stdout_*.log -Wait
```

### Erro 500.19

- Reinstale **ASP.NET Core Hosting Bundle**
- Reinicie o PC
- Execute `iisreset` no PowerShell

### Erro de Conexão com Banco

Edite `C:\inetpub\wwwroot\EventlyApi\appsettings.json`:

```json
{
  "ConnectionStrings": {
    "EventsDatabase": "Host=localhost;Port=5432;Database=evently;Username=postgres;Password=SuaSenha"
  }
}
```

## 📊 Comandos Úteis

```powershell
# Ver logs em tempo real
Get-Content C:\inetpub\wwwroot\EventlyApi\logs\stdout_*.log -Wait

# Parar site
Stop-WebSite -Name "EventlyApi"

# Iniciar site
Start-WebSite -Name "EventlyApi"

# Reciclar Application Pool
Restart-WebAppPool -Name "EventlyApiPool"

# Verificar status
Get-WebsiteState -Name "EventlyApi"
```

## ✅ Checklist

- [ ] IIS instalado
- [ ] ASP.NET Core 10.0 Hosting Bundle instalado
- [ ] PC reiniciado após instalação
- [ ] PostgreSQL rodando
- [ ] Script executado como Administrador
- [ ] Site acessível em http://localhost:5050

## 📚 Recursos

- [Guia Completo IIS](docs/docs/guides/iis-deployment.md)
- [Documentação Microsoft](https://learn.microsoft.com/aspnet/core/host-and-deploy/iis/)

---

**Boa sorte com o deploy! 🚀**
