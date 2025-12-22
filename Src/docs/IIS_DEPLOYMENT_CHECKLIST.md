# ✅ IIS Deployment Checklist

Use este checklist para garantir que o deploy no IIS está correto.

---

## 📋 Pré-requisitos (Uma Vez)

### Sistema Operacional
- [ ] Windows Server 2016+ OU Windows 10/11 Pro/Enterprise/Education
- [ ] **NÃO** é Windows Home (não suporta IIS)

### IIS Instalado
- [ ] IIS instalado (Painel de Controle → Recursos do Windows)
- [ ] Console de Gerenciamento do IIS disponível
- [ ] Serviços da World Wide Web habilitados
- [ ] WebSockets Protocol habilitado (se necessário)

### ASP.NET Core Hosting Bundle
- [ ] ASP.NET Core 10.0 Hosting Bundle baixado
- [ ] Hosting Bundle instalado
- [ ] **PC REINICIADO** após instalação ⚠️
- [ ] `dotnet --info` mostra runtime 10.0.x
- [ ] AspNetCoreModuleV2 aparece no IIS

### PostgreSQL
- [ ] PostgreSQL instalado OU
- [ ] PostgreSQL rodando em Docker OU
- [ ] PostgreSQL acessível remotamente
- [ ] Porta 5432 acessível
- [ ] Credenciais conhecidas (user/password)

---

## 🚀 Deploy da Aplicação

### Publicação
- [ ] Projeto `Evently.Api` compilando sem erros
- [ ] Publicação executada (Visual Studio ou `dotnet publish`)
- [ ] Arquivos em `C:\inetpub\wwwroot\EventlyApi\` (ou caminho customizado)
- [ ] Arquivo `Evently.Api.dll` presente
- [ ] Arquivo `web.config` gerado automaticamente
- [ ] Arquivo `appsettings.json` presente

### Connection String
- [ ] `appsettings.json` editado
- [ ] Connection string do PostgreSQL configurada
- [ ] Host correto (localhost, IP remoto, etc)
- [ ] Porta correta (5432 padrão)
- [ ] Database name correto (evently)
- [ ] Username/Password corretos

### Pasta de Logs
- [ ] Pasta `logs` criada em `C:\inetpub\wwwroot\EventlyApi\logs\`
- [ ] Permissões de escrita configuradas (se necessário)

---

## ⚙️ Configuração IIS

### Application Pool
- [ ] Application Pool criado
- [ ] Nome: `EventlyApiPool` (ou customizado)
- [ ] **.NET CLR Version:** `No Managed Code` ⚠️ CRÍTICO
- [ ] Managed Pipeline Mode: `Integrated`
- [ ] Identity: `ApplicationPoolIdentity`
- [ ] **Load User Profile:** `True` ⚠️
- [ ] Start Application Pool Immediately: Marcado

### Website
- [ ] Website criado
- [ ] Nome: `EventlyApi` (ou customizado)
- [ ] Application Pool: `EventlyApiPool` selecionado
- [ ] Physical Path: `C:\inetpub\wwwroot\EventlyApi`
- [ ] Binding Type: `http`
- [ ] Port: `5050`
- [ ] Host name: (vazio ou conforme necessário)

### Permissões
- [ ] Usuário `IIS_IUSRS` adicionado às permissões da pasta
- [ ] Permissão `Read & Execute` concedida
- [ ] Permissão `List folder contents` concedida
- [ ] Permissão `Read` concedida

---

## 🔧 Configuração Avançada (Opcional)

### HTTPS
- [ ] Certificado SSL instalado (se usar HTTPS)
- [ ] Binding HTTPS configurado (porta 5051 ou outra)
- [ ] Certificado selecionado no binding

### Firewall
- [ ] Regra de firewall criada para porta 5050 (HTTP)
- [ ] Regra de firewall criada para porta 5051 (HTTPS, se aplicável)
- [ ] Testes de acesso remoto funcionando

### web.config Personalizado
- [ ] `hostingModel` configurado (`inprocess` recomendado)
- [ ] `stdoutLogEnabled` = `true` (para debug)
- [ ] `stdoutLogFile` apontando para pasta `logs`
- [ ] Variáveis de ambiente configuradas (se necessário)

---

## ✅ Testes

### Inicialização
- [ ] Application Pool iniciado
- [ ] Website iniciado
- [ ] Status do site: `Started` no IIS Manager
- [ ] Nenhum erro no Event Viewer

### Testes Locais
- [ ] `http://localhost:5050` responde
- [ ] `http://localhost:5050/events` retorna JSON ou 200 OK
- [ ] `http://localhost:5050/scalar/v1` abre Scalar UI (Development apenas)
- [ ] Criar evento via POST funciona
- [ ] Logs aparecem em `C:\inetpub\wwwroot\EventlyApi\logs\`

### Testes PowerShell
```powershell
# Status
- [ ] Get-WebsiteState -Name EventlyApi retorna "Started"
- [ ] Get-WebAppPoolState -Name EventlyApiPool retorna "Started"

# HTTP
- [ ] curl http://localhost:5050/events retorna resposta
```

### Testes Remotos (se aplicável)
- [ ] Acesso de outra máquina funciona: `http://<IP>:5050/events`
- [ ] Firewall permite conexões externas
- [ ] Rede configurada corretamente

---

## 🐛 Troubleshooting

### Erros Comuns Verificados
- [ ] **NÃO** aparece erro 500.19 (Configuration Error)
- [ ] **NÃO** aparece erro 500.30 (ANCM In-Process Start Failure)
- [ ] **NÃO** aparece erro 502.5 (Process Failure)
- [ ] **NÃO** aparece erro 503 (Service Unavailable)

### Logs Verificados
- [ ] Logs de stdout sem erros críticos
- [ ] Event Viewer → Application sem erros do IIS
- [ ] IIS logs sem erros 500

### Conexão Banco de Dados
- [ ] PostgreSQL acessível: `Test-NetConnection localhost -Port 5432`
- [ ] Connection string válida
- [ ] Credenciais corretas
- [ ] Database `evently` existe

---

## 📊 Monitoramento (Opcional)

### Performance
- [ ] Performance Monitor configurado (opcional)
- [ ] Contadores de .NET CLR Memory monitorados
- [ ] CPU e Memory sendo monitorados

### Health Checks
- [ ] Endpoint de health check configurado (opcional)
- [ ] Monitoramento automático configurado (opcional)

---

## 🔐 Segurança (Produção)

### SSL/TLS
- [ ] Certificado SSL válido instalado
- [ ] HTTPS obrigatório configurado
- [ ] HTTP → HTTPS redirect configurado

### Headers de Segurança
- [ ] `X-Powered-By` removido do web.config
- [ ] Server header removido
- [ ] HSTS configurado (se HTTPS)

### Hardening
- [ ] Versões de servidor ocultadas
- [ ] Rate limiting configurado (se necessário)
- [ ] CORS configurado corretamente

---

## 📚 Documentação

### Scripts
- [ ] Script `deploy-to-iis.ps1` funciona
- [ ] README files criados
- [ ] Guias acessíveis pela equipe

### Docusaurus
- [ ] Guia em `docs/docs/guides/iis-deployment.md` acessível
- [ ] Documentação atualizada no http://localhost:3000

---

## 🚀 CI/CD (Futuro)

### Automação
- [ ] Pipeline de deploy planejado
- [ ] Testes automatizados antes de deploy
- [ ] Rollback strategy definida

---

## ✅ Checklist Final - Rápido

**Primeira vez:**
- [ ] IIS instalado
- [ ] Hosting Bundle instalado e PC reiniciado
- [ ] PostgreSQL rodando
- [ ] Aplicação publicada
- [ ] Application Pool criado (No Managed Code!)
- [ ] Website criado
- [ ] Permissões IIS_IUSRS configuradas
- [ ] Connection string editada
- [ ] Site iniciado
- [ ] HTTP teste: ✅

**Atualizações:**
- [ ] Código atualizado
- [ ] `deploy-to-iis.ps1` executado
- [ ] Logs verificados
- [ ] Testes passando

---

## 🎉 Sucesso!

Se todos os itens estão marcados, parabéns! 🎉

Sua API Evently está rodando profissionalmente no IIS!

**URLs:**
- Local: http://localhost:5050
- Remoto: http://<SEU-IP>:5050

**Comandos úteis:**
```powershell
# Status
Get-WebsiteState -Name EventlyApi
Get-WebAppPoolState -Name EventlyApiPool

# Logs
Get-Content C:\inetpub\wwwroot\EventlyApi\logs\stdout_*.log -Wait

# Controle
Start-WebSite -Name EventlyApi
Stop-WebSite -Name EventlyApi
Restart-WebAppPool -Name EventlyApiPool
```

---

**Última atualização:** $(Get-Date -Format "yyyy-MM-dd HH:mm")
