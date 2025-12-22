# 📋 Resumo: Alterações para Executar no IIS

## ✅ O Que Foi Alterado

### 1. **`Program.cs`** - Adicionado Suporte IIS

**Arquivo:** `Src/Api/Evently.Api/Program.cs`

**Alterações:**
```csharp
// NOVO: Integração com IIS
builder.WebHost.UseIISIntegration();

// NOVO: Forwarded Headers
app.UseForwardedHeaders();

// NOVO: OpenAPI e Scalar em Production
if (app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    
    // NOVO: Migrações opcionais em Production
    var applyMigrationsInProduction = builder.Configuration.GetValue<bool>("ApplyMigrationsOnStartup", false);
    if (applyMigrationsInProduction)
    {
        app.ApplyMigrations();
    }
}
```

---

### 2. **`appsettings.Production.json`** - Criado

**Arquivo:** `Src/Api/Evently.Api/appsettings.Production.json`

**Conteúdo:**
```json
{
  "ConnectionStrings": {
    "EventsDatabase": "Host=localhost;Port=5432;Database=evently;Username=postgres;Password=C@151867"
  },
  "ApplyMigrationsOnStartup": true,
  "Logging": { ... }
}
```

**Importante:**
- ✅ Connection string para PostgreSQL local
- ✅ Migrações automáticas habilitadas
- ✅ Logs configurados

---

### 3. **Documentação** - Criada

**Arquivo:** `CONFIGURACOES_IIS.md`

Guia completo de como configurar PostgreSQL e deploy no IIS.

---

## 🚀 O Que Fazer Agora

### Passo 1: Configurar PostgreSQL

Escolha uma opção:

**Opção A - Docker (Rápido):**
```powershell
docker run -d --name postgres-evently -e POSTGRES_PASSWORD=C@151867 -e POSTGRES_DB=evently -p 5432:5432 postgres:latest
```

**Opção B - Docker Compose:**
```powershell
docker-compose up -d evently.database
```

**Opção C - Instalar PostgreSQL:**
- Baixar: https://www.postgresql.org/download/windows/
- Criar database `evently`

---

### Passo 2: Testar Localmente (Opcional)

```powershell
cd Src\Api\Evently.Api
$env:ASPNETCORE_ENVIRONMENT="Production"
dotnet run

# Testar em outro terminal
curl http://localhost:5050/scalar/v1
```

---

### Passo 3: Deploy no IIS

```powershell
# Como Administrador
.\deploy-to-iis.ps1
```

**Ou manualmente:**
```powershell
cd Src\Api\Evently.Api
dotnet publish -c Release -o C:\inetpub\wwwroot\EventlyApi
```

Depois configure IIS conforme: [IIS_DEPLOY_README.md](IIS_DEPLOY_README.md)

---

### Passo 4: Testar

```
http://localhost:5050/scalar/v1
http://localhost:5050/events
```

---

## 📝 Alterações Detalhadas

### Por Que Cada Alteração

| Alteração | Por Quê | Impacto |
|-----------|---------|---------|
| `UseIISIntegration()` | IIS injeta variáveis de ambiente (porta, path) | Necessário para IIS funcionar |
| `UseForwardedHeaders()` | IIS é um proxy reverso | Headers X-Forwarded-* funcionam corretamente |
| `MapOpenApi()` em Production | Permitir testar API via Scalar | Facilita validação pós-deploy |
| `ApplyMigrations()` em Production | Migrations automáticas no IIS local | Evita erro "tabela não existe" |
| `appsettings.Production.json` | IIS usa `ASPNETCORE_ENVIRONMENT=Production` | Connection string e configs de produção |

---

## ⚠️ Diferenças Docker vs IIS

| Aspecto | Docker | IIS |
|---------|--------|-----|
| **Ambiente** | `Development` | `Production` |
| **Migrações** | Automáticas (sempre) | Opcionais (configurável) |
| **Connection String** | `evently.database` | `localhost` ou IP |
| **Scalar/OpenAPI** | Apenas Development | Adicionado em Production |
| **Logs** | stdout do container | `C:\inetpub\wwwroot\EventlyApi\logs\` |

---

## 🔒 Segurança - Produção Real

⚠️ **IMPORTANTE:** As configurações acima são para **IIS LOCAL/DESENVOLVIMENTO**.

Para **produção real**, altere:

### `appsettings.Production.json`
```json
{
  "ApplyMigrationsOnStartup": false,  ← Desabilitar!
  // ...
}
```

### `Program.cs`
Remova ou comente:
```csharp
if (app.Environment.IsProduction())
{
    // app.MapOpenApi();           ← Remover em produção
    // app.MapScalarApiReference(); ← Remover em produção
}
```

Aplique migrations via script SQL ou `dotnet ef`:
```powershell
dotnet ef database update
```

---

## ✅ Checklist Rápido

- [ ] `Program.cs` atualizado
- [ ] `appsettings.Production.json` criado
- [ ] PostgreSQL rodando (local, Docker ou remoto)
- [ ] Connection string correta
- [ ] IIS instalado
- [ ] ASP.NET Core 10 Hosting Bundle instalado
- [ ] Script `deploy-to-iis.ps1` executado
- [ ] Site testado: http://localhost:5050

---

## 📚 Documentação Completa

- **Configuração:** [CONFIGURACOES_IIS.md](CONFIGURACOES_IIS.md)
- **Deploy IIS:** [IIS_DEPLOY_README.md](IIS_DEPLOY_README.md)
- **Guia Completo:** [docs/docs/guides/iis-deployment.md](docs/docs/guides/iis-deployment.md)
- **Quick Reference:** [IIS_QUICK_REFERENCE.md](IIS_QUICK_REFERENCE.md)

---

## 🎯 Próximos Passos

1. **Leia:** [CONFIGURACOES_IIS.md](CONFIGURACOES_IIS.md)
2. **Configure:** PostgreSQL (Docker, local ou remoto)
3. **Deploy:** Execute `.\deploy-to-iis.ps1`
4. **Teste:** http://localhost:5050/scalar/v1

---

**Pronto! Sua aplicação agora está compatível com IIS! 🚀**

Dúvidas? Consulte [CONFIGURACOES_IIS.md](CONFIGURACOES_IIS.md)
