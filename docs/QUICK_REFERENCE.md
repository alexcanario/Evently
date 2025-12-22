# 🚀 Quick Reference - Docusaurus + OpenAPI

## Passo a Passo em 5 Minutos

### ✅ Pré-requisitos
```bash
node --version   # >= 18.0
docker --version # Qualquer versão recente
```

### 📦 1. Instalar (APENAS UMA VEZ)
```bash
cd docs
npm install
```

### 🔄 2. Workflow Diário

#### Opção A - Automático (Windows) ⭐
```bash
cd docs
update-docs.bat
```

#### Opção B - Manual
```bash
# Passo 1: Garantir API rodando
docker-compose up -d

# Passo 2: Baixar OpenAPI
cd docs
npm run fetch-openapi

# Passo 3: Gerar docs
npm run gen-api-docs

# Passo 4: Iniciar
npm start
```

---

## 🎯 Comandos Essenciais

| Comando | O Que Faz |
|---------|-----------|
| `npm start` | Inicia Docusaurus (http://localhost:3000) |
| `npm run fetch-openapi` | Baixa OpenAPI da API |
| `npm run gen-api-docs` | Gera docs da API |
| `npm run clear` | Limpa cache |
| `npm run build` | Build de produção |

---

## 📂 Arquivos Importantes

```
docs/
├── docs/
│   ├── intro.md                              ← EDITAR ✏️
│   ├── quick-start.md                        ← EDITAR ✏️
│   ├── guides/                               
│   │   ├── docusaurus-openapi-integration.md ← LER 📖
│   │   └── docker-setup.md                   ← EDITAR ✏️
│   └── api/                                  ← NÃO EDITAR! 🤖
│
├── static/openapi/evently-api.json           ← GERADO 🔄
├── docusaurus.config.js                      ← CONFIGURAR ⚙️
├── sidebars.js                               ← CONFIGURAR 📑
└── update-docs.bat                           ← EXECUTAR 🚀
```

---

## 🔧 Quando Alterar Endpoints .NET

```bash
# 1. Editar código C#
# Adicionar novo MapPost/MapGet em CreateEvent.cs ou similar

# 2. Rebuild
docker-compose build

# 3. Restart
docker-compose up -d

# 4. Atualizar docs
cd docs
update-docs.bat
```

---

## 🐛 Solução Rápida de Problemas

### ❌ API não responde
```bash
docker-compose ps
docker-compose up -d
docker logs evently.api
```

### ❌ fetch-openapi falha
```bash
# Testar manualmente
curl http://localhost:5050/openapi/v1.json

# Verificar se porta 5050 está livre
netstat -an | findstr 5050
```

### ❌ gen-api-docs erro
```bash
# Verificar se JSON existe
dir static\openapi\evently-api.json

# Se não existe
npm run fetch-openapi

# Limpar cache e tentar novamente
npm run clear
npm run gen-api-docs
```

### ❌ Página 404 ou branca
```bash
npm run clear
rm -rf node_modules
npm install
npm start
```

---

## 🌐 URLs de Acesso

| URL | O Que É |
|-----|---------|
| http://localhost:3000 | **Docusaurus** - Sua documentação |
| http://localhost:3000/docs/api | **API Reference** - Docs da API |
| http://localhost:5050/scalar/v1 | **Scalar UI** - Testar API |
| http://localhost:5050/openapi/v1.json | **OpenAPI JSON** - Spec raw |

---

## 🎨 Personalizar

### Cores
Edite `src/css/custom.css`:
```css
:root {
  --ifm-color-primary: #7c3aed;
}
```

### Logo
1. Coloque em `static/img/logo.svg`
2. Edite `docusaurus.config.js`:
```javascript
logo: { src: 'img/logo.svg' }
```

### Novo Guia
1. Crie `docs/guides/meu-guia.md`
2. Adicione em `sidebars.js`

---

## ✅ Checklist Rápido

Está funcionando se você conseguir:

- [ ] Acessar http://localhost:3000
- [ ] Ver "API Reference" no menu
- [ ] Clicar em "POST /events"
- [ ] Usar "Try it out"
- [ ] Ver response 201 Created

---

## 📚 Onde Aprender Mais

- **Guia Completo:** `docs/guides/docusaurus-openapi-integration.md`
- **Checklist Detalhado:** `CHECKLIST.md`
- **README Completo:** `README.md`

---

## 💡 Dicas Pro

1. **Busca rápida:** Pressione `Ctrl+K` na documentação
2. **Dark mode:** Clique no ícone 🌙 no topo
3. **Mobile:** Acesse http://localhost:3000 no celular
4. **Deploy:** `npm run build` → Upload `build/` para Netlify/Vercel

---

**Pronto! 🎉 Agora você tem docs profissionais em 5 minutos!**

*Para detalhes completos, veja: `docs/guides/docusaurus-openapi-integration.md`*
