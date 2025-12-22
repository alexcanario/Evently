---
sidebar_position: 3
---

# Postman Collections

Aprenda a usar Postman para testar a API Evently.

## 🎯 O Que São Postman Collections?

**Postman Collections** são conjuntos de requisições HTTP organizadas que podem ser:
- ✅ Executadas manualmente
- ✅ Executadas automaticamente (testes)
- ✅ Compartilhadas com a equipe
- ✅ Versionadas no Git
- ✅ Geradas automaticamente do OpenAPI

---

## 🚀 Opção 1: Import Direto (Mais Rápido) ⭐

### Passo 1: Iniciar a API

```bash
docker-compose up -d
```

### Passo 2: Importar no Postman

1. Abra o **Postman Desktop**
2. Clique em **"Import"** (canto superior esquerdo)
3. Selecione **"Link"**
4. Cole a URL: `http://localhost:5050/openapi/v1.json`
5. Clique em **"Import"**

**Pronto!** Todos os endpoints aparecem automaticamente na collection.

---

## 🔄 Opção 2: Gerar Collection Automaticamente

Use o script fornecido:

```bash
# Na raiz do projeto Evently
generate-postman-collection.bat
```

**O que o script faz:**
1. ✅ Verifica se a API está rodando
2. ✅ Baixa OpenAPI spec de `http://localhost:5050/openapi/v1.json`
3. ✅ Converte para Postman Collection
4. ✅ Cria Environment de desenvolvimento
5. ✅ Salva tudo em `postman/`

**Arquivos gerados:**
```
postman/
├── Evently-API.postman_collection.json     ← Collection
├── Evently-Dev.postman_environment.json    ← Environment
└── evently-openapi.json                    ← OpenAPI source
```

### Importar Arquivos Gerados

1. No Postman: **Import** → **Files**
2. Selecione ambos `.json`:
   - `Evently-API.postman_collection.json`
   - `Evently-Dev.postman_environment.json`
3. Clique em **Import**

---

## ⚙️ Configurar Environment

### Selecionar Environment

1. Clique no **dropdown** de environments (canto superior direito)
2. Selecione **"Evently - Development"**

### Variáveis Disponíveis

| Variável | Valor | Uso |
|----------|-------|-----|
| `baseUrl` | `http://localhost:5050` | Requisições HTTP |
| `baseUrlHttps` | `https://localhost:5051` | Requisições HTTPS |
| `apiVersion` | `v1` | Versão da API |

### Usar Variáveis

Nos requests, use `{{variavel}}`:

```
GET {{baseUrl}}/events
POST {{baseUrl}}/events
```

---

## 📝 Testar Endpoints

### Exemplo: Criar Evento

1. **Expanda a Collection:**
   - `Evently API` → `Events` → `Create Event`

2. **Configure o Body:**
   
   Clique na aba **Body**, selecione **raw** → **JSON**:
   
   ```json
   {
     "title": "Tech Conference 2024",
     "description": "Annual tech conference",
     "location": "São Paulo Convention Center",
     "startsAtUtc": "2024-12-15T09:00:00Z",
     "endsAtUtc": "2024-12-15T18:00:00Z"
   }
   ```

3. **Enviar Request:**
   
   Clique em **Send**

4. **Verificar Response:**
   
   ```json
   {
     "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
     "message": "Event 'Tech Conference 2024' created successfully!"
   }
   ```

---

## 🧪 Adicionar Testes Automatizados

### Testes Básicos

Clique na aba **Tests** e adicione:

```javascript
// Verificar status code
pm.test("Status is 201 Created", function () {
    pm.response.to.have.status(201);
});

// Verificar que response tem ID
pm.test("Response contains event ID", function () {
    var jsonData = pm.response.json();
    pm.expect(jsonData).to.have.property('id');
    pm.expect(jsonData.id).to.be.a('string');
});

// Verificar mensagem de sucesso
pm.test("Response contains success message", function () {
    var jsonData = pm.response.json();
    pm.expect(jsonData).to.have.property('message');
});

// Salvar ID para usar em outros requests
pm.environment.set("lastEventId", pm.response.json().id);
```

### Testes Avançados

```javascript
// Validar schema
pm.test("Response schema is valid", function () {
    var schema = {
        type: "object",
        required: ["id", "message"],
        properties: {
            id: { type: "string", format: "uuid" },
            message: { type: "string" }
        }
    };
    pm.response.to.have.jsonSchema(schema);
});

// Verificar tempo de resposta
pm.test("Response time is less than 500ms", function () {
    pm.expect(pm.response.responseTime).to.be.below(500);
});

// Verificar headers
pm.test("Content-Type is JSON", function () {
    pm.response.to.have.header("Content-Type");
    pm.expect(pm.response.headers.get("Content-Type")).to.include("application/json");
});
```

---

## 🔄 Atualizar Collection (Quando API Mudar)

### Método Automático

Quando adicionar novos endpoints no .NET:

```bash
# 1. Rebuild da API
docker-compose build
docker-compose up -d

# 2. Regenerar collection
generate-postman-collection.bat
```

### Reimportar no Postman

1. **Import** → **File**
2. Selecione `postman/Evently-API.postman_collection.json`
3. Escolha **"Replace"** quando perguntado
4. Clique em **Import**

---

## 🎨 Organizar Collection

### Criar Pastas

1. Clique direito na Collection
2. **Add Folder**
3. Nomeie (ex: "Authentication", "Events", "Users")

### Ordenar Requests

Arraste e solte requests entre pastas.

### Adicionar Descrições

1. Clique no request
2. Vá para **Documentation** tab
3. Adicione descrição em Markdown:

```markdown
# Create Event

Creates a new event in the system.

## Request Body

- **title** (required): Event title
- **description**: Event description
- **location**: Event location
- **startsAtUtc** (required): Start date in UTC
- **endsAtUtc** (required): End date in UTC

## Response

Returns the created event ID and success message.
```

---

## 🔐 Adicionar Autenticação (Futuro)

Quando implementar autenticação JWT:

### 1. Adicionar Variável de Token

No Environment, adicione:
```json
{
  "key": "authToken",
  "value": "",
  "type": "secret"
}
```

### 2. Configurar Authorization na Collection

1. Clique na Collection → **Authorization**
2. Type: **Bearer Token**
3. Token: `{{authToken}}`

### 3. Request de Login

```javascript
// No request de login, adicione em Tests:
var jsonData = pm.response.json();
pm.environment.set("authToken", jsonData.token);
```

Agora todos os requests usarão o token automaticamente!

---

## 🤖 Automatizar com Newman (CLI)

### Instalar Newman

```bash
npm install -g newman
```

### Executar Collection via CLI

```bash
newman run postman/Evently-API.postman_collection.json \
  -e postman/Evently-Dev.postman_environment.json \
  --reporters cli,json
```

### Integrar no CI/CD

**GitHub Actions (`.github/workflows/api-tests.yml`):**

```yaml
name: API Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Start API
        run: docker-compose up -d
      
      - name: Wait for API
        run: sleep 15
      
      - name: Install Newman
        run: npm install -g newman
      
      - name: Run Postman Tests
        run: |
          newman run postman/Evently-API.postman_collection.json \
            -e postman/Evently-Dev.postman_environment.json \
            --reporters cli,junit \
            --reporter-junit-export results.xml
      
      - name: Publish Test Results
        uses: EnricoMi/publish-unit-test-result-action@v2
        if: always()
        with:
          files: results.xml
```

---

## 📊 Executar Collection Runner

### Via Interface

1. Clique na Collection → **Run**
2. Selecione requests para executar
3. Configure iterations (quantas vezes executar)
4. Clique em **Run Evently API**

### Com Dados Externos (CSV/JSON)

1. Crie arquivo `test-data.json`:

```json
[
  {
    "title": "Event 1",
    "location": "São Paulo",
    "startsAtUtc": "2024-12-01T10:00:00Z",
    "endsAtUtc": "2024-12-01T18:00:00Z"
  },
  {
    "title": "Event 2",
    "location": "Rio de Janeiro",
    "startsAtUtc": "2024-12-05T10:00:00Z",
    "endsAtUtc": "2024-12-05T18:00:00Z"
  }
]
```

2. No Runner: **Select File** → `test-data.json`
3. No request, use variáveis: `{{title}}`, `{{location}}`, etc.

---

## 🌐 Publicar Collection

### Postman Workspace (Público)

1. Clique na Collection → **Share**
2. **Get public link**
3. Copie o link
4. Adicione ao `README.md`:

```markdown
[![Run in Postman](https://run.pstmn.io/button.svg)](https://god.gw.postman.com/run-collection/your-collection-id)
```

### Publicar Documentação

1. Collection → **View Documentation**
2. **Publish**
3. Configure nome e descrição
4. **Publish Collection**

---

## 📚 Recursos Adicionais

### Links Úteis

- [Postman Learning Center](https://learning.postman.com/)
- [Writing Tests](https://learning.postman.com/docs/writing-scripts/test-scripts/)
- [Newman CLI](https://learning.postman.com/docs/running-collections/using-newman-cli/command-line-integration-with-newman/)
- [Postman API](https://www.postman.com/postman/workspace/postman-public-workspace/documentation/12959542-c8142d51-e97c-46b6-bd77-52bb66712c9a)

### Exemplos de Testes

```javascript
// Validar array
pm.test("Events array is not empty", function () {
    var jsonData = pm.response.json();
    pm.expect(jsonData).to.be.an('array');
    pm.expect(jsonData.length).to.be.above(0);
});

// Validar formato de data
pm.test("Date is in ISO 8601 format", function () {
    var jsonData = pm.response.json();
    pm.expect(jsonData.startsAtUtc).to.match(/^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(Z|[+-]\d{2}:\d{2})$/);
});

// Validar GUID
pm.test("ID is valid GUID", function () {
    var jsonData = pm.response.json();
    pm.expect(jsonData.id).to.match(/^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i);
});
```

---

## ✅ Checklist

- [ ] Postman instalado
- [ ] Collection importada
- [ ] Environment configurado e selecionado
- [ ] Teste manual de POST /events funcionando
- [ ] Testes automatizados adicionados
- [ ] Newman CLI instalado (opcional)
- [ ] Collection Runner testado (opcional)

---

## 🎉 Próximos Passos

1. **Adicionar mais testes** em cada endpoint
2. **Criar test suites** por funcionalidade
3. **Integrar no CI/CD** com Newman
4. **Publicar documentação** no Postman
5. **Compartilhar com equipe**

---

**Boa sorte testando a API! 🚀**
