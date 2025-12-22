---
sidebar_position: 1
---

# Integração Docusaurus + OpenAPI - Guia Completo

Este guia ensina como integrar documentação interativa com Docusaurus e OpenAPI em um projeto .NET 10.

## 📋 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- ✅ [Node.js 18+](https://nodejs.org/) - Para executar o Docusaurus
- ✅ [.NET 10 SDK](https://dotnet.microsoft.com/download) - Já instalado no projeto
- ✅ [Docker Desktop](https://www.docker.com/products/docker-desktop) - Para rodar a API
- ✅ Editor de código (VS Code ou Visual Studio 2025)

**Verificar versões:**
```bash
node --version    # Deve ser >= 18.0
npm --version     # Vem com o Node.js
dotnet --version  # Deve ser >= 10.0
```

---

## 🎯 O Que Vamos Fazer?

1. Configurar OpenAPI no projeto .NET
2. Criar projeto Docusaurus
3. Instalar plugins OpenAPI
4. Configurar integração
5. Gerar documentação automaticamente
6. Personalizar a documentação

**Tempo estimado:** 30-45 minutos

---

## Parte 1: Configurar OpenAPI no .NET 10

### Passo 1.1: Verificar Pacotes NuGet

O projeto Evently já tem os pacotes necessários. Verifique no arquivo `.csproj`:

```bash
# Navegar para o projeto API
cd Src/Api/Evently.Api
```

Verifique se tem:
```xml
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="10.0.0" />
<PackageReference Include="Scalar.AspNetCore" Version="1.2.42" />
```

Se não tiver, instale:
```bash
dotnet add package Microsoft.AspNetCore.OpenApi
dotnet add package Scalar.AspNetCore
```

### Passo 1.2: Verificar Program.cs

Abra `Src/Api/Evently.Api/Program.cs` e confirme que tem:

```csharp
// 1. Registrar OpenAPI
builder.Services.AddOpenApi();

// 2. Mapear endpoint OpenAPI (Development apenas)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();  // ✅ Expõe /openapi/v1.json
    app.MapScalarApiReference(); // ✅ UI interativa
}
```

**O que cada linha faz:**
- `AddOpenApi()` - Configura geração automática do spec OpenAPI
- `MapOpenApi()` - Expõe JSON em `/openapi/v1.json`
- `MapScalarApiReference()` - UI interativa em `/scalar/v1`

### Passo 1.3: Testar OpenAPI

```bash
# 1. Iniciar a API
docker-compose up -d

# 2. Esperar 10-15 segundos para containers iniciarem

# 3. Acessar OpenAPI JSON
curl http://localhost:5050/openapi/v1.json

# Ou abrir no navegador:
# http://localhost:5050/openapi/v1.json
```

**Resultado esperado:** JSON grande com especificação OpenAPI 3.0

### Passo 1.4: Entender o OpenAPI Gerado

O .NET gera automaticamente:
- **Endpoints** - Todos os `MapPost`, `MapGet`, etc.
- **Schemas** - DTOs como `CreateEventRequest`
- **Tags** - Agrupamento por `WithTags(Tags.Events)`
- **Metadata** - Nomes, descrições, parâmetros

**Exemplo do JSON gerado:**
```json
{
  "openapi": "3.0.1",
  "info": {
    "title": "Evently.Api",
    "version": "1.0"
  },
  "paths": {
    "/events": {
      "post": {
        "tags": ["Events"],
        "operationId": "CreateEvent",
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/CreateEventRequest"
              }
            }
          }
        }
      }
    }
  }
}
```

---

## Parte 2: Criar Projeto Docusaurus

### Passo 2.1: Criar Estrutura de Diretórios

Na **raiz do projeto Evently**, crie a pasta `docs`:

```bash
# Voltar para raiz do projeto
cd D:/code/learning/_Javanovic/Evently

# Criar pasta docs
mkdir docs
cd docs
```

### Passo 2.2: Inicializar Projeto Node.js

```bash
# Criar package.json
npm init -y
```

Isso cria um `package.json` básico. Vamos editá-lo no próximo passo.

### Passo 2.3: Editar package.json

Substitua o conteúdo de `docs/package.json`:

```json
{
  "name": "evently-docs",
  "version": "1.0.0",
  "private": true,
  "scripts": {
    "docusaurus": "docusaurus",
    "start": "docusaurus start",
    "build": "docusaurus build",
    "serve": "docusaurus serve",
    "clear": "docusaurus clear",
    "fetch-openapi": "curl http://localhost:5050/openapi/v1.json -o static/openapi/evently-api.json",
    "gen-api-docs": "docusaurus gen-api-docs all"
  },
  "dependencies": {
    "@docusaurus/core": "^3.6.3",
    "@docusaurus/preset-classic": "^3.6.3",
    "@mdx-js/react": "^3.1.0",
    "clsx": "^2.1.1",
    "docusaurus-plugin-openapi-docs": "^4.3.0",
    "docusaurus-theme-openapi-docs": "^4.3.0",
    "prism-react-renderer": "^2.4.1",
    "react": "^18.3.1",
    "react-dom": "^18.3.1"
  },
  "devDependencies": {
    "@docusaurus/module-type-aliases": "^3.6.3",
    "@docusaurus/types": "^3.6.3"
  },
  "engines": {
    "node": ">=18.0"
  }
}
```

**O que cada dependência faz:**
- `@docusaurus/core` - Motor principal do Docusaurus
- `@docusaurus/preset-classic` - Tema e plugins padrão
- `docusaurus-plugin-openapi-docs` - **Processa OpenAPI → Markdown**
- `docusaurus-theme-openapi-docs` - **UI interativa para API**

### Passo 2.4: Instalar Dependências

```bash
npm install
```

**Aguarde 2-3 minutos.** O npm vai baixar todos os pacotes.

**Resultado esperado:**
```
added 1234 packages in 2m
```

---

## Parte 3: Configurar Docusaurus

### Passo 3.1: Criar Estrutura de Pastas

```bash
# Dentro de docs/
mkdir -p docs/guides
mkdir -p static/openapi
mkdir -p static/img
mkdir -p src/css
```

**Estrutura final:**
```
docs/
├── docs/              ← Documentação em Markdown
│   ├── intro.md
│   ├── quick-start.md
│   └── guides/
├── static/
│   ├── openapi/       ← OpenAPI JSON
│   └── img/           ← Imagens/logos
├── src/
│   └── css/
│       └── custom.css
├── package.json
└── docusaurus.config.js  ← Vamos criar agora
```

### Passo 3.2: Criar docusaurus.config.js

Crie `docs/docusaurus.config.js`:

```javascript
// @ts-check
import {themes as prismThemes} from 'prism-react-renderer';

/** @type {import('@docusaurus/types').Config} */
const config = {
  title: 'Evently API Documentation',
  tagline: 'Modern event management platform',
  favicon: 'img/favicon.ico',

  // URL do site (altere quando fizer deploy)
  url: 'https://your-domain.com',
  baseUrl: '/',

  // GitHub info (opcional)
  organizationName: 'alexcanario',
  projectName: 'evently',

  onBrokenLinks: 'warn',
  onBrokenMarkdownLinks: 'warn',

  // Idiomas suportados
  i18n: {
    defaultLocale: 'en',
    locales: ['en', 'pt', 'es'],
  },

  presets: [
    [
      'classic',
      /** @type {import('@docusaurus/preset-classic').Options} */
      ({
        docs: {
          sidebarPath: './sidebars.js',
          // ⭐ Importante: usa componente customizado para API
          docItemComponent: "@theme/ApiItem",
        },
        blog: {
          showReadingTime: true,
        },
        theme: {
          customCss: './src/css/custom.css',
        },
      }),
    ],
  ],

  // ⭐⭐⭐ CONFIGURAÇÃO PRINCIPAL - OpenAPI Plugin
  plugins: [
    [
      'docusaurus-plugin-openapi-docs',
      {
        id: "api",
        docsPluginId: "classic",
        config: {
          evently: {  // Nome da API
            // Onde está o JSON OpenAPI
            specPath: "static/openapi/evently-api.json",
            // Onde gerar os docs
            outputDir: "docs/api",
            sidebarOptions: {
              groupPathsBy: "tag",  // Agrupar por tags (Events, etc)
              categoryLinkSource: "tag",
            },
          },
        },
      },
    ],
  ],

  // ⭐ Tema OpenAPI
  themes: ["docusaurus-theme-openapi-docs"],

  themeConfig:
    /** @type {import('@docusaurus/preset-classic').ThemeConfig} */
    ({
      navbar: {
        title: 'Evently',
        items: [
          {
            type: 'docSidebar',
            sidebarId: 'tutorialSidebar',
            position: 'left',
            label: 'Docs',
          },
          {
            to: '/docs/api',  // ⭐ Link para API Reference
            label: 'API Reference',
            position: 'left',
          },
          {
            href: 'http://localhost:5050/scalar/v1',
            label: 'Scalar UI',
            position: 'right',
          },
          {
            href: 'https://github.com/alexcanario/Evently',
            label: 'GitHub',
            position: 'right',
          },
        ],
      },
      footer: {
        style: 'dark',
        copyright: `Copyright © ${new Date().getFullYear()} Evently.`,
      },
      prism: {
        theme: prismThemes.github,
        darkTheme: prismThemes.dracula,
        // Linguagens para syntax highlighting
        additionalLanguages: ['csharp', 'json', 'bash', 'powershell'],
      },
    }),
};

export default config;
```

**Pontos-chave:**
1. **specPath** - Caminho para o OpenAPI JSON
2. **outputDir** - Onde gerar os docs da API
3. **groupPathsBy: "tag"** - Agrupa endpoints por tags do OpenAPI

### Passo 3.3: Criar sidebars.js

Crie `docs/sidebars.js`:

```javascript
/** @type {import('@docusaurus/plugin-content-docs').SidebarsConfig} */
const sidebars = {
  tutorialSidebar: [
    {
      type: 'category',
      label: 'Getting Started',
      items: ['intro', 'quick-start'],
    },
    {
      type: 'category',
      label: 'Guides',
      items: ['guides/docker-setup'],
    },
  ],
};

export default sidebars;
```

### Passo 3.4: Criar Arquivo CSS Customizado

Crie `docs/src/css/custom.css`:

```css
/**
 * Cores personalizadas do Evently
 */
:root {
  --ifm-color-primary: #7c3aed;
  --ifm-color-primary-dark: #6d28d9;
  --ifm-color-primary-darker: #5b21b6;
  --ifm-color-primary-darkest: #4c1d95;
  --ifm-color-primary-light: #8b5cf6;
  --ifm-color-primary-lighter: #a78bfa;
  --ifm-color-primary-lightest: #c4b5fd;
  --ifm-code-font-size: 95%;
  --docusaurus-highlighted-code-line-bg: rgba(0, 0, 0, 0.1);
}

[data-theme='dark'] {
  --docusaurus-highlighted-code-line-bg: rgba(0, 0, 0, 0.3);
}
```

---

## Parte 4: Criar Conteúdo Inicial

### Passo 4.1: Criar docs/docs/intro.md

```markdown
---
sidebar_position: 1
---

# Bem-vindo ao Evently

**Evently** é uma plataforma moderna de gerenciamento de eventos construída com **.NET 10** e **PostgreSQL**.

## 🎯 Recursos

- ✅ Gerenciamento completo de eventos
- ✅ Suporte multi-idioma (EN, PT, ES)
- ✅ API REST com OpenAPI
- ✅ Docker Compose para desenvolvimento
- ✅ PostgreSQL como banco de dados

## 🚀 Links Rápidos

- [Guia de Início Rápido](./quick-start)
- [Referência da API](/docs/api)
- [Scalar UI Interativa](http://localhost:5050/scalar/v1)

## 📚 Stack Tecnológica

| Tecnologia | Versão | Uso |
|-----------|--------|-----|
| .NET | 10.0 | Framework principal |
| PostgreSQL | Latest | Banco de dados |
| EF Core | 10.0 | ORM |
| Docker | Latest | Containerização |
| Scalar | 1.2+ | UI da API |
```

### Passo 4.2: Criar docs/docs/quick-start.md

```markdown
---
sidebar_position: 2
---

# Início Rápido

Configure e execute o Evently em menos de 5 minutos!

## ✅ Pré-requisitos

- Docker Desktop instalado e rodando
- .NET 10 SDK
- Visual Studio 2025 ou VS Code

## 🐳 Executar com Docker Compose

### 1. Clonar o repositório

\`\`\`bash
git clone https://github.com/alexcanario/Evently.git
cd Evently
\`\`\`

### 2. Iniciar os containers

\`\`\`bash
docker-compose up -d
\`\`\`

**O que isso faz:**
- ✅ Inicia PostgreSQL na porta 5432
- ✅ Inicia API na porta 5050 (HTTP) e 5051 (HTTPS)
- ✅ Aplica migrações automaticamente
- ✅ Configura volumes persistentes

### 3. Verificar se está rodando

\`\`\`bash
docker ps
\`\`\`

Deve mostrar:
\`\`\`
evently.api         Up
evently.database    Up (healthy)
\`\`\`

### 4. Acessar a API

- **Scalar UI**: http://localhost:5050/scalar/v1
- **OpenAPI Spec**: http://localhost:5050/openapi/v1.json

## 📝 Criar Primeiro Evento

### Usando curl

\`\`\`bash
curl -X POST http://localhost:5050/events \\
  -H "Content-Type: application/json" \\
  -d '{
    "title": "Meu Primeiro Evento",
    "description": "Testando a API Evently",
    "location": "Online",
    "startsAtUtc": "2024-12-31T18:00:00Z",
    "endsAtUtc": "2024-12-31T20:00:00Z"
  }'
\`\`\`

### Usando Scalar UI

1. Abra http://localhost:5050/scalar/v1
2. Expanda `POST /events`
3. Clique em "Try it"
4. Preencha os campos
5. Clique em "Send"

## 🛑 Parar os Containers

\`\`\`bash
docker-compose down
\`\`\`

## ⚡ Próximos Passos

- [Configuração do Docker](./guides/docker-setup)
- [Referência Completa da API](/docs/api)
```

### Passo 4.3: Criar docs/docs/guides/docker-setup.md

```markdown
---
sidebar_position: 1
---

# Configuração Docker

Detalhes sobre a configuração Docker do Evently.

## 📦 Serviços

### evently.api

**Imagem:** `mcr.microsoft.com/dotnet/aspnet:10.0`

**Portas:**
- `5050:8080` - HTTP
- `5051:8081` - HTTPS

**Variáveis de Ambiente:**
\`\`\`yaml
ASPNETCORE_ENVIRONMENT: Development
ASPNETCORE_HTTP_PORTS: 8080
ASPNETCORE_HTTPS_PORTS: 8081
\`\`\`

### evently.database

**Imagem:** `postgres:latest`

**Porta:** `5432:5432`

**Credenciais:**
\`\`\`yaml
POSTGRES_USER: postgres
POSTGRES_PASSWORD: C@151867
POSTGRES_DB: evently
\`\`\`

## 🔍 Healthcheck

O PostgreSQL tem healthcheck configurado:

\`\`\`yaml
healthcheck:
  test: ["CMD-SHELL", "pg_isready -U postgres -d evently"]
  interval: 10s
  timeout: 5s
  retries: 5
\`\`\`

A API só inicia após o banco estar saudável.

## 💾 Volumes

### postgres_events

Volume nomeado para persistência dos dados:

\`\`\`yaml
volumes:
  postgres_events:/var/lib/postgresql/data
\`\`\`

**Localização:** Gerenciado pelo Docker

**Backup:**
\`\`\`bash
docker exec evently.database pg_dump -U postgres evently > backup.sql
\`\`\`

**Restore:**
\`\`\`bash
docker exec -i evently.database psql -U postgres evently < backup.sql
\`\`\`

## 🔧 Comandos Úteis

### Ver logs

\`\`\`bash
# API
docker logs evently.api -f

# Database
docker logs evently.database -f
\`\`\`

### Acessar container

\`\`\`bash
# API
docker exec -it evently.api bash

# Database
docker exec -it evently.database psql -U postgres -d evently
\`\`\`

### Rebuild

\`\`\`bash
docker-compose build --no-cache
docker-compose up -d
\`\`\`
```

---

## Parte 5: Gerar Documentação da API

### Passo 5.1: Garantir que a API está Rodando

```bash
# Voltar para raiz
cd D:/code/learning/_Javanovic/Evently

# Iniciar API
docker-compose up -d

# Aguardar 15 segundos
timeout /t 15

# Testar
curl http://localhost:5050/openapi/v1.json
```

### Passo 5.2: Buscar OpenAPI JSON

```bash
# Voltar para docs
cd docs

# Criar pasta se não existir
mkdir -p static/openapi

# Baixar OpenAPI
npm run fetch-openapi
```

**Windows (PowerShell):**
```powershell
# Se curl não funcionar no Windows, use:
Invoke-WebRequest -Uri http://localhost:5050/openapi/v1.json -OutFile static/openapi/evently-api.json
```

**Verificar:**
```bash
# Deve existir o arquivo
dir static/openapi/evently-api.json
```

### Passo 5.3: Gerar Documentação

```bash
npm run gen-api-docs
```

**O que acontece:**
1. Plugin lê `static/openapi/evently-api.json`
2. Gera arquivos `.mdx` em `docs/api/`
3. Cria sidebar automático
4. Processa schemas, endpoints, exemplos

**Resultado esperado:**
```
✔ Successfully generated docs for evently
📝 Created 15 markdown files in docs/api/
```

### Passo 5.4: Iniciar Docusaurus

```bash
npm start
```

**Aguarde 30-60 segundos.** Abre automaticamente em:
```
http://localhost:3000
```

---

## Parte 6: Explorar a Documentação

### Menu de Navegação

1. **Docs** - Guias e tutoriais (intro, quick-start)
2. **API Reference** - Documentação gerada do OpenAPI
3. **Scalar UI** - Link para interface interativa
4. **GitHub** - Link para repositório

### Estrutura da API Reference

```
API Reference
└── Events
    ├── POST /events (CreateEvent)
    ├── GET /events (GetEvents)
    └── ... outros endpoints
```

### Recursos Interativos

Cada endpoint tem:
- ✅ **Descrição** do endpoint
- ✅ **Parâmetros** (query, path, body)
- ✅ **Request body** com schema
- ✅ **Responses** possíveis (200, 400, 404, etc)
- ✅ **Try it out** - testar direto na documentação
- ✅ **Code samples** - exemplos em várias linguagens

---

## Parte 7: Atualizar Documentação (Workflow)

### Quando Adicionar Novos Endpoints

1. **Criar endpoint no .NET**
```csharp
app.MapGet("events/{id}", (Guid id) => { ... })
   .WithName("GetEventById")
   .WithTags(Tags.Events);
```

2. **Rebuild da API**
```bash
docker-compose build
docker-compose up -d
```

3. **Atualizar docs**
```bash
cd docs

# Baixar novo OpenAPI
npm run fetch-openapi

# Regenerar docs
npm run gen-api-docs

# Ver resultado
npm start
```

### Automatizar com Script

Crie `docs/update-docs.sh` (Linux/Mac) ou `update-docs.bat` (Windows):

**Windows (update-docs.bat):**
```batch
@echo off
echo [1/3] Fetching OpenAPI spec...
curl http://localhost:5050/openapi/v1.json -o static/openapi/evently-api.json

echo [2/3] Generating API docs...
call npm run gen-api-docs

echo [3/3] Starting Docusaurus...
call npm start
```

**Executar:**
```bash
cd docs
update-docs.bat
```

---

## Parte 8: Build para Produção

### Passo 8.1: Build Estático

```bash
cd docs
npm run build
```

**Resultado:**
- Gera site estático em `docs/build/`
- HTML, CSS, JS otimizados
- Pronto para deploy

### Passo 8.2: Testar Build

```bash
npm run serve
```

Abre em `http://localhost:3000` (versão de produção)

### Passo 8.3: Deploy

**Opções:**

1. **GitHub Pages**
```bash
npm run deploy
```

2. **Netlify/Vercel**
- Conectar repositório
- Build command: `cd docs && npm run build`
- Publish directory: `docs/build`

3. **Azure Static Web Apps**
```bash
az staticwebapp create --name evently-docs --source docs
```

---

## 🎓 Conceitos-Chave para Aprender

### 1. OpenAPI Specification

**O que é:**
- Padrão para descrever APIs REST
- JSON/YAML com estrutura específica
- Independente de linguagem

**Estrutura:**
```json
{
  "openapi": "3.0.1",
  "info": { ... },
  "paths": {
    "/events": {
      "post": { ... }
    }
  },
  "components": {
    "schemas": { ... }
  }
}
```

### 2. Docusaurus Plugins

**docusaurus-plugin-openapi-docs:**
- Lê OpenAPI JSON
- Converte para Markdown (MDX)
- Gera sidebar automático

**docusaurus-theme-openapi-docs:**
- Componentes React customizados
- UI interativa (Try it out)
- Syntax highlighting

### 3. MDX (Markdown + JSX)

Arquivos `.mdx` permitem:
```mdx
# Meu Endpoint

Descrição normal em Markdown.

<ApiExample endpoint="POST /events" />

Mais markdown aqui.
```

### 4. Tags no OpenAPI

```csharp
// .NET
app.MapPost("events", ...)
   .WithTags(Tags.Events);  // ← Agrupa endpoints

// Gera no OpenAPI:
{
  "paths": {
    "/events": {
      "post": {
        "tags": ["Events"]  // ← Usado pelo Docusaurus
      }
    }
  }
}
```

---

## 🐛 Troubleshooting

### Problema: fetch-openapi falha

**Solução:**
```bash
# Verificar se API está rodando
curl http://localhost:5050/openapi/v1.json

# Se não funcionar:
docker-compose ps  # Ver status
docker-compose up -d  # Reiniciar
```

### Problema: gen-api-docs não gera nada

**Solução:**
```bash
# Verificar se JSON existe
dir static/openapi/evently-api.json

# Verificar config
cat docusaurus.config.js | grep specPath

# Limpar cache
npm run clear
npm run gen-api-docs
```

### Problema: Página 404 na API Reference

**Solução:**
```bash
# Regenerar sidebar
npm run gen-api-docs

# Limpar e reiniciar
npm run clear
npm start
```

### Problema: "Module not found"

**Solução:**
```bash
# Reinstalar dependências
rm -rf node_modules package-lock.json
npm install
```

---

## 📚 Recursos Adicionais

### Documentação Oficial

- [Docusaurus](https://docusaurus.io/)
- [OpenAPI Specification](https://swagger.io/specification/)
- [docusaurus-openapi-docs](https://github.com/PaloAltoNetworks/docusaurus-openapi-docs)
- [.NET OpenAPI](https://learn.microsoft.com/aspnet/core/web-api/microsoft.aspnetcore.openapi)

### Exemplos

- [Docusaurus Showcase](https://docusaurus.io/showcase)
- [OpenAPI Examples](https://github.com/OAI/OpenAPI-Specification/tree/main/examples)

---

## ✅ Checklist Final

Você completou a integração quando conseguir:

- [ ] Acessar http://localhost:5050/openapi/v1.json
- [ ] Rodar `npm run fetch-openapi` com sucesso
- [ ] Rodar `npm run gen-api-docs` sem erros
- [ ] Ver docs em http://localhost:3000
- [ ] Navegar em "API Reference" → "Events"
- [ ] Testar "Try it out" em um endpoint
- [ ] Adicionar novo endpoint e atualizar docs

---

## 🎉 Próximos Passos

Agora que você tem a base:

1. **Personalizar** - Cores, logo, conteúdo
2. **Adicionar guias** - Autenticação, erros comuns
3. **Versionar API** - OpenAPI v1, v2, etc
4. **CI/CD** - Automatizar build e deploy
5. **Feedback** - Adicionar comentários, analytics

**Parabéns! 🚀 Você agora tem documentação profissional e interativa!**
