# ⚙️ Configurações para Executar no IIS Localmente

## 📋 Checklist de Alterações

### ✅ Arquivos Modificados/Criados

- [x] `Src/Api/Evently.Api/Program.cs` - Adicionado suporte IIS
- [x] `Src/Api/Evently.Api/appsettings.Production.json` - Criado
- [ ] PostgreSQL configurado localmente (veja abaixo)

---

## 🔧 1. Alterações no Código (JÁ FEITAS ✅)

### `Program.cs`

Foram adicionadas as seguintes configurações:

```csharp
// Integração com IIS
builder.WebHost.UseIISIntegration();

// Forwarded Headers (para proxies/IIS)
app.UseForwardedHeaders();

// OpenAPI e Scalar também em produção
if (app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    
    // Migrações automáticas (controlado por config)
    var applyMigrationsInProduction = builder.Configuration.GetValue<bool>("ApplyMigrationsOnStartup", false);
    if (applyMigrationsInProduction)
    {
        app.ApplyMigrations();
    }
}
```

**O que isso faz:**
- ✅ `UseIISIntegration()` - Configura porta, path base e outras configs do IIS
- ✅ `UseForwardedHeaders()` - Trata headers de proxy (X-Forwarded-For, etc)
- ✅ OpenAPI em produção - Permite testar API via Scalar mesmo em produção
- ✅ Migrações opcionais - Aplica migrations automaticamente se configurado

### `appsettings.Production.json`

Criado com:

```json
{
  "ConnectionStrings": {
    "EventsDatabase": "Host=localhost;Port=5432;Database=evently;Username=postgres;Password=C@151867"
  },
  "ApplyMigrationsOnStartup": true  ← Aplica migrações ao iniciar
}
```

---

## 🐘 2. Configurar PostgreSQL Local

### Opção A: Instalar PostgreSQL Localmente

1. **Baixar PostgreSQL:**
   https://www.postgresql.org/download/windows/

2. **Instalar:**
   - Porta: `5432` (padrão)
   - Password: `C@151867` (ou altere no appsettings)

3. **Criar Database:**
   ```sql
   -- Abrir pgAdmin ou psql
   CREATE DATABASE evently;
   ```

### Opção B: Usar Docker ⭐ (Recomendado)

```powershell
# Apenas PostgreSQL (sem a API)
docker run -d `
  --name postgres-evently `
  -e POSTGRES_USER=postgres `
  -e POSTGRES_PASSWORD=C@151867 `
  -e POSTGRES_DB=evently `
  -p 5432:5432 `
  -v evently-data:/var/lib/postgresql/data `
  postgres:latest

# Verificar
docker ps

# Parar
docker stop postgres-evently

# Iniciar novamente
docker start postgres-evently

# Remover (cuidado, apaga dados!)
docker rm -f postgres-evently
```

### Opção C: Usar PostgreSQL do Docker Compose

```powershell
# Iniciar apenas o banco
docker-compose up -d evently.database

# Verificar
docker-compose ps

# Parar
docker-compose stop evently.database
```

### Opção D: Usar PostgreSQL Remoto

Edite `appsettings.Production.json`:

```json
{
  "ConnectionStrings": {
    "EventsDatabase": "Host=SEU-IP-REMOTO;Port=5432;Database=evently;Username=postgres;Password=SUA-SENHA"
  }
}
```

---

## 🔍 3. Verificar Connection String

Depois de configurar PostgreSQL, teste a conexão:

```powershell
# PowerShell
Test-NetConnection localhost -Port 5432

# Ou via psql (se instalado)
psql -h localhost -U postgres -d evently

# Ou via Docker (se usando Docker)
docker exec -it postgres-evently psql -U postgres -d evently
```

**Resultado esperado:**
- ✅ `TcpTestSucceeded : True` (PowerShell)
- ✅ Conexão bem-sucedida ao banco

---

## 📝 4. Configurações Adicionais (Opcional)

### Desabilitar Migrações Automáticas em Produção Real

Se for publicar em servidor de produção real, edite `appsettings.Production.json`:

```json
{
  "ApplyMigrationsOnStartup": false  ← Desabilitar
}
```

E aplique migrations manualmente:

```powershell
cd Src\Api\Evently.Api
dotnet ef database update --project ..\..\Modules\Events\Evently.Modules.Events.Api\Evently.Modules.Events.Api.csproj
```

### Alterar Senha do PostgreSQL

Se quiser usar senha diferente:

1. Altere no PostgreSQL (pgAdmin ou psql)
2. Atualize `appsettings.Production.json`:
   ```json
   {
     "ConnectionStrings": {
       "EventsDatabase": "Host=localhost;Port=5432;Database=evently;Username=postgres;Password=NOVA-SENHA"
     }
   }
   ```

### Habilitar Logs Detalhados

Em `appsettings.Production.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Information",        ← Mudar para Information
      "Microsoft.EntityFrameworkCore": "Information" ← Ver queries SQL
    }
  }
}
```

---

## 🚀 5. Testar Localmente Antes do IIS

Antes de publicar no IIS, teste em modo Production local:

```powershell
cd Src\Api\Evently.Api

# Definir ambiente como Production
$env:ASPNETCORE_ENVIRONMENT="Production"

# Executar
dotnet run

# Testar
curl http://localhost:5050/events
# ou
curl http://localhost:5050/scalar/v1
```

**Se funcionar localmente, funcionará no IIS!**

---

## 📦 6. Publicar para IIS

Agora que tudo está configurado:

### Opção A: Script Automático

```powershell
# Como Administrador
.\deploy-to-iis.ps1
```

### Opção B: Manual

```powershell
cd Src\Api\Evently.Api
dotnet publish -c Release -o C:\inetpub\wwwroot\EventlyApi
```

---

## ✅ Checklist Final

Antes de publicar no IIS:

- [ ] PostgreSQL rodando (local, Docker ou remoto)
- [ ] Connection string correta em `appsettings.Production.json`
- [ ] `ApplyMigrationsOnStartup` configurado (`true` para IIS local)
- [ ] Teste local funcionando (`dotnet run` com `ASPNETCORE_ENVIRONMENT=Production`)
- [ ] Porta 5432 acessível
- [ ] Database `evently` criada (ou será criada automaticamente)

Depois do deploy no IIS:

- [ ] Site acessível em http://localhost:5050
- [ ] Scalar UI funcionando: http://localhost:5050/scalar/v1
- [ ] Criar evento funciona (POST /events)
- [ ] Logs sem erros em `C:\inetpub\wwwroot\EventlyApi\logs\`

---

## 🐛 Troubleshooting

### Erro: "Failed to apply migrations"

**Solução:**
```powershell
# Verificar se PostgreSQL está acessível
Test-NetConnection localhost -Port 5432

# Aplicar migrations manualmente
cd Src\Api\Evently.Api
dotnet ef database update --project ..\..\Modules\Events\Evently.Modules.Events.Api\Evently.Modules.Events.Api.csproj
```

### Erro: "Connection refused" ou "Could not connect to server"

**Causa:** PostgreSQL não está rodando

**Solução:**
```powershell
# Se Docker
docker start postgres-evently
# ou
docker-compose up -d evently.database

# Se PostgreSQL local
Get-Service postgresql* | Start-Service
```

### Erro: "database 'evently' does not exist"

**Solução:**
```powershell
# Via Docker
docker exec -it postgres-evently psql -U postgres -c "CREATE DATABASE evently;"

# Via psql local
psql -U postgres -c "CREATE DATABASE evently;"
```

### Scalar UI não abre em Produção

**Causa:** Configuração faltando

**Verificar em `Program.cs`:**
```csharp
if (app.Environment.IsProduction())
{
    app.MapOpenApi();           ← Deve estar presente
    app.MapScalarApiReference(); ← Deve estar presente
}
```

---

## 📚 Recursos Adicionais

- **Guia IIS Completo:** [docs/docs/guides/iis-deployment.md](docs/docs/guides/iis-deployment.md)
- **Script Deploy:** [deploy-to-iis.ps1](deploy-to-iis.ps1)
- **Quick Reference:** [IIS_QUICK_REFERENCE.md](IIS_QUICK_REFERENCE.md)
- **Checklist:** [IIS_DEPLOYMENT_CHECKLIST.md](IIS_DEPLOYMENT_CHECKLIST.md)

---

**Agora sua aplicação está pronta para rodar no IIS! 🎉**

Próximo passo: Execute `.\deploy-to-iis.ps1` (como Administrador)
