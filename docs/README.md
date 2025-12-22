# 📚 Evently Documentation

Documentação interativa da API Evently usando **Docusaurus** + **OpenAPI**.

---

## 🚀 Quick Start (Para Iniciantes)

### 1️⃣ Instalar Dependências

```bash
cd docs
npm install
```

⏱️ **Tempo:** 2-3 minutos

### 2️⃣ Garantir que a API está Rodando

```bash
# Voltar para raiz do projeto
cd ..

# Iniciar containers
docker-compose up -d

# Aguardar 15 segundos
timeout /t 15

# Verificar
curl http://localhost:5050/openapi/v1.json
```

### 3️⃣ Atualizar Documentação (Automático - Windows)

```bash
cd docs
update-docs.bat
```

**OU Manual:**

```bash
cd docs
npm run fetch-openapi
npm run gen-api-docs
npm start
```

### 4️⃣ Acessar

Abre automaticamente em **http://localhost:3000**

---

## 📖 Guias Completos

Para aprender passo a passo:

1. **[Guia Completo de Integração](docs/guides/docusaurus-openapi-integration.md)** ⭐
   - Tutorial detalhado com explicações
   - Conceitos fundamentais
   - Troubleshooting

2. **[Checklist de Validação](CHECKLIST.md)**
   - Verificar se tudo foi configurado
   - Comandos essenciais
   - Resolução rápida de problemas

3. **[Configuração Docker](docs/guides/docker-setup.md)**
   - Detalhes dos containers
   - Variáveis de ambiente
   - Comandos úteis

---

## 🛠️ Comandos Disponíveis

```bash
npm start              # Inicia servidor dev (http://localhost:3000)
npm run build          # Build para produção
npm run serve          # Testa build de produção
npm run clear          # Limpa cache do Docusaurus
npm run fetch-openapi  # Baixa spec OpenAPI da API
npm run gen-api-docs   # Gera docs da API
```

---

## 📁 Estrutura de Arquivos

```
docs/
├── docs/                          # 📝 Documentação Manual
│   ├── intro.md                   # Página inicial
│   ├── quick-start.md             # Guia rápido
│   ├── api/                       # 🤖 GERADO AUTOMATICAMENTE
│   │   ├── create-event.mdx       # (NÃO EDITAR!)
│   │   └── ...                    
│   └── guides/
│       ├── docker-setup.md
│       └── docusaurus-openapi-integration.md  # ⭐ Guia completo
│
├── static/
│   ├── openapi/
│   │   └── evently-api.json       # 📥 OpenAPI baixado da API
│   └── img/                       # Imagens, logos
│
├── src/
│   └── css/
│       └── custom.css             # 🎨 Customização de cores
│
├── docusaurus.config.js           # ⚙️ Configuração principal
├── sidebars.js                    # 📑 Estrutura do menu
├── package.json                   # 📦 Dependências
├── update-docs.bat                # 🔄 Script de atualização (Windows)
├── CHECKLIST.md                   # ✅ Validação
└── README.md                      # 📖 Este arquivo
```

---

## 🔄 Workflow de Atualização

### Quando Adicionar/Modificar Endpoints na API

```bash
# 1. Editar código .NET (ex: adicionar novo MapPost)

# 2. Rebuild da API
docker-compose build
docker-compose up -d

# 3. Atualizar docs (escolha um):

# Opção A - Automático (Windows)
cd docs
update-docs.bat

# Opção B - Manual
cd docs
npm run fetch-openapi
npm run gen-api-docs
npm start
```

---

## 🎨 Personalização

### Alterar Cores

Edite `src/css/custom.css`:

```css
:root {
  --ifm-color-primary: #7c3aed;  /* Roxo do Evently */
  --ifm-color-primary-dark: #6d28d9;
  /* ... */
}
```

### Adicionar Logo

1. Coloque logo em `static/img/logo.svg`
2. Edite `docusaurus.config.js`:

```javascript
navbar: {
  logo: {
    alt: 'Evently Logo',
    src: 'img/logo.svg',
  },
}
```

### Adicionar Novo Guia

1. Crie arquivo em `docs/guides/meu-guia.md`
2. Adicione ao `sidebars.js`:

```javascript
{
  type: 'category',
  label: 'Guides',
  items: [
    'guides/docker-setup',
    'guides/docusaurus-openapi-integration',
    'guides/meu-guia',  // ← Novo
  ],
}
```

---

## 🐛 Problemas Comuns

### ❌ "Cannot find module 'docusaurus-plugin-openapi-docs'"

```bash
npm install
```

### ❌ "fetch-openapi failed"

```bash
# Verificar se API está rodando
curl http://localhost:5050/openapi/v1.json

# Se não:
docker-compose up -d
```

### ❌ "gen-api-docs" não gera nada

```bash
# Verificar se JSON existe
dir static\openapi\evently-api.json

# Se não existe:
npm run fetch-openapi

# Então:
npm run gen-api-docs
```

### ❌ Página 404 em /docs/api

```bash
npm run clear
npm run gen-api-docs
npm start
```

---

## 📦 Dependências Principais

| Pacote | Versão | O Que Faz |
|--------|--------|-----------|
| `@docusaurus/core` | ^3.6.3 | Motor do Docusaurus |
| `docusaurus-plugin-openapi-docs` | ^4.3.0 | Processa OpenAPI → Markdown |
| `docusaurus-theme-openapi-docs` | ^4.3.0 | UI interativa da API |

---

## 🌐 URLs Importantes

| Serviço | URL | Descrição |
|---------|-----|-----------|
| **Docusaurus** | http://localhost:3000 | Documentação principal |
| **API Reference** | http://localhost:3000/docs/api | Docs gerados do OpenAPI |
| **Scalar UI** | http://localhost:5050/scalar/v1 | UI interativa da API (.NET) |
| **OpenAPI JSON** | http://localhost:5050/openapi/v1.json | Spec OpenAPI raw |
| **API Base** | http://localhost:5050 | Evently API |

---

## 🎓 Recursos de Aprendizado

### Documentação Oficial

- 📘 [Docusaurus](https://docusaurus.io/)
- 📗 [OpenAPI Specification](https://swagger.io/specification/)
- 📙 [docusaurus-openapi-docs](https://github.com/PaloAltoNetworks/docusaurus-openapi-docs)
- 📕 [.NET OpenAPI](https://learn.microsoft.com/aspnet/core/web-api/microsoft.aspnetcore.openapi)

### Guias Internos

- ⭐ **[Guia Completo de Integração](docs/guides/docusaurus-openapi-integration.md)** - Passo a passo detalhado
- ✅ **[Checklist](CHECKLIST.md)** - Validação e troubleshooting
- 🐳 **[Docker Setup](docs/guides/docker-setup.md)** - Configuração dos containers

---

## 🚢 Deploy (Produção)

### Build Estático

```bash
npm run build
```

Gera site em `build/` pronto para deploy.

### Opções de Deploy

**1. GitHub Pages**
```bash
# Configure no docusaurus.config.js:
organizationName: 'alexcanario'
projectName: 'evently'

# Deploy:
npm run deploy
```

**2. Netlify**
- Build command: `cd docs && npm run build`
- Publish directory: `docs/build`

**3. Vercel**
- Framework: Docusaurus
- Root directory: `docs`

**4. Azure Static Web Apps**
```bash
az staticwebapp create \
  --name evently-docs \
  --source docs \
  --build-command "npm run build" \
  --output-location build
```

---

## ✨ Recursos da Documentação

Quando tudo estiver funcionando:

✅ **Documentação Viva** - Sincronizada com a API  
✅ **Try It Out** - Testar endpoints direto na docs  
✅ **Busca Integrada** - Ctrl+K para buscar  
✅ **Multi-idioma** - EN, PT, ES suportados  
✅ **Dark Mode** - Tema claro/escuro  
✅ **Code Samples** - Exemplos em várias linguagens  
✅ **Versionamento** - Suporte a múltiplas versões da API  
✅ **Mobile Friendly** - Responsivo  

---

## 🤝 Contribuindo

Para adicionar ou melhorar a documentação:

1. Edite arquivos `.md` em `docs/docs/`
2. Para docs da API, edite o código .NET (gera automaticamente)
3. Teste localmente: `npm start`
4. Commit e push

**NÃO EDITE:**
- Arquivos em `docs/api/*.mdx` (gerados automaticamente)

---

## 📞 Suporte

- 🐛 **Issues:** [GitHub Issues](https://github.com/alexcanario/Evently/issues)
- 💬 **Discussões:** [GitHub Discussions](https://github.com/alexcanario/Evently/discussions)
- 📧 **Email:** seu-email@exemplo.com

---

## 📜 Licença

Este projeto está sob a licença MIT.

---

**Feito com ❤️ usando Docusaurus**
