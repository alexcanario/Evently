# 📋 Checklist - Integração Docusaurus + OpenAPI

Use este checklist para verificar se você completou todos os passos.

## ✅ Parte 1: Configuração .NET

- [ ] Pacote `Microsoft.AspNetCore.OpenApi` instalado
- [ ] Pacote `Scalar.AspNetCore` instalado  
- [ ] `builder.Services.AddOpenApi()` no Program.cs
- [ ] `app.MapOpenApi()` no Program.cs (dentro de Development)
- [ ] API rodando: `docker-compose up -d`
- [ ] OpenAPI acessível: http://localhost:5050/openapi/v1.json
- [ ] Scalar UI acessível: http://localhost:5050/scalar/v1

## ✅ Parte 2: Estrutura Docusaurus

- [ ] Pasta `docs/` criada na raiz do projeto
- [ ] `package.json` criado com dependências corretas
- [ ] `npm install` executado com sucesso
- [ ] Pastas criadas: `docs/docs`, `static/openapi`, `src/css`

## ✅ Parte 3: Arquivos de Configuração

- [ ] `docusaurus.config.js` criado e configurado
- [ ] `sidebars.js` criado
- [ ] `src/css/custom.css` criado
- [ ] Plugin `docusaurus-plugin-openapi-docs` configurado
- [ ] Theme `docusaurus-theme-openapi-docs` configurado

## ✅ Parte 4: Conteúdo Inicial

- [ ] `docs/docs/intro.md` criado
- [ ] `docs/docs/quick-start.md` criado
- [ ] `docs/docs/guides/docker-setup.md` criado
- [ ] `docs/docs/guides/docusaurus-openapi-integration.md` criado

## ✅ Parte 5: Geração da Documentação

- [ ] API rodando (http://localhost:5050)
- [ ] `npm run fetch-openapi` executado (ou curl manual)
- [ ] Arquivo `static/openapi/evently-api.json` existe
- [ ] `npm run gen-api-docs` executado sem erros
- [ ] Pasta `docs/api/` criada com arquivos .mdx

## ✅ Parte 6: Execução

- [ ] `npm start` executado com sucesso
- [ ] Docusaurus abre em http://localhost:3000
- [ ] Menu "Docs" acessível
- [ ] Menu "API Reference" acessível
- [ ] Endpoint "POST /events" visível em API Reference
- [ ] "Try it out" funciona

## ✅ Parte 7: Testes

- [ ] Criar novo evento pela Scalar UI
- [ ] Criar novo evento pelo Docusaurus (Try it out)
- [ ] Verificar response 201 Created
- [ ] Navegar entre diferentes endpoints
- [ ] Testar busca na documentação (Ctrl+K)

## ✅ Parte 8: Workflow de Atualização

- [ ] Script `update-docs.bat` criado (Windows)
- [ ] Adicionar novo endpoint no .NET
- [ ] Rebuild API: `docker-compose build`
- [ ] Executar `update-docs.bat`
- [ ] Verificar novo endpoint na documentação

## ✅ Bônus: Personalização

- [ ] Alterar cores em `src/css/custom.css`
- [ ] Adicionar logo em `static/img/`
- [ ] Configurar i18n (múltiplos idiomas)
- [ ] Adicionar mais guias em `docs/guides/`
- [ ] Configurar GitHub Pages ou Netlify

---

## 🎯 Comandos Essenciais (Resumo)

### Iniciar API
```bash
docker-compose up -d
```

### Atualizar Documentação (Automático - Windows)
```bash
cd docs
update-docs.bat
```

### Atualizar Documentação (Manual)
```bash
cd docs
npm run fetch-openapi
npm run gen-api-docs
npm start
```

### Build para Produção
```bash
cd docs
npm run build
npm run serve
```

---

## 🐛 Troubleshooting Rápido

### API não responde
```bash
docker-compose ps
docker-compose up -d
docker logs evently.api
```

### fetch-openapi falha
```bash
# Testar manualmente
curl http://localhost:5050/openapi/v1.json

# Verificar portas
netstat -an | findstr 5050
```

### gen-api-docs não funciona
```bash
# Verificar se JSON existe
dir static\openapi\evently-api.json

# Limpar cache
npm run clear
npm run gen-api-docs
```

### Página em branco no Docusaurus
```bash
# Limpar tudo e reconstruir
npm run clear
rm -rf node_modules
npm install
npm start
```

---

## 📚 Arquivos Importantes

| Arquivo | Propósito |
|---------|-----------|
| `package.json` | Dependências e scripts npm |
| `docusaurus.config.js` | Configuração principal |
| `sidebars.js` | Estrutura do menu lateral |
| `static/openapi/*.json` | Especificações OpenAPI |
| `docs/api/*.mdx` | Docs gerados (não editar!) |
| `docs/docs/*.md` | Documentação manual |

---

## ✨ Resultado Final

Quando tudo estiver funcionando, você terá:

✅ Documentação viva que atualiza com a API  
✅ Interface interativa para testar endpoints  
✅ Busca integrada (Ctrl+K)  
✅ Suporte a múltiplos idiomas  
✅ Versionamento de docs  
✅ Deploy automático possível  
✅ Código samples em várias linguagens  
✅ Dark mode  

**Parabéns! 🎉**
