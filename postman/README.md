# 📮 Postman Collections - Evently API

Este diretório contém as Postman Collections e Environments geradas automaticamente da API Evently.

---

## 🚀 Quick Start

### Método 1: Importar URL (Mais Rápido) ⭐

1. Inicie a API:
   ```sh
   docker-compose up -d
   ```

2. No Postman:
   - Clique em **Import**
   - Escolha **Link**
   - Cole: `http://localhost:5050/openapi/v1.json`
   - Clique em **Import**

### Método 2: Gerar Collection Automaticamente

```sh
# Na raiz do projeto
generate-postman-collection.bat
```

**Arquivos gerados:**
- ✅ `Evently-API.postman_collection.json` - Collection completa
- ✅ `Evently-Dev.postman_environment.json` - Environment de dev
- ✅ `evently-openapi.json` - OpenAPI source

### Método 3: Importar Arquivos Manualmente

1. No Postman: **Import** → **File**
2. Selecione os arquivos `.json` desta pasta
3. Clique em **Import**

---

## 📁 Arquivos

| Arquivo | Descrição |
|---------|-----------|
| `Evently-API.postman_collection.json` | Collection com todos os endpoints |
| `Evently-Dev.postman_environment.json` | Environment (variáveis de ambiente) |
| `evently-openapi.json` | OpenAPI spec baixado da API |

---

## ⚙️ Configurar Environment

Após importar:

1. Clique no **dropdown de environments** (canto superior direito)
2. Selecione **"Evently - Development"**
3. Verifique as variáveis:
   - `baseUrl`: `http://localhost:5050`
   - `baseUrlHttps`: `https://localhost:5051`
   - `apiVersion`: `v1`

---

## 🔄 Atualizar Collection (Quando API Mudar)

### Opção A - Automático (Recomendado)

```sh
generate-postman-collection.bat
```

Então no Postman: **Import** → **Replace existing**

### Opção B - Manual

1. Garantir API rodando: `docker-compose up -d`
2. Baixar OpenAPI:
   ```sh
   curl http://localhost:5050/openapi/v1.json -o postman/evently-openapi.json
   ```
3. Converter:
   ```sh
   npx openapi-to-postmanv2 -s postman/evently-openapi.json -o postman/Evently-API.postman_collection.json -p
   ```
4. No Postman: **Import** → Selecionar arquivo → **Replace**

---

## 📝 Usar a Collection

### Criar Evento (POST /events)

1. Expanda **Evently API** → **Events**
2. Clique em **Create Event**
3. Vá para **Body**
4. Edite o JSON:
   ```json
   {
     "title": "Meu Evento",
     "description": "Descrição do evento",
     "location": "Online",
     "startsAtUtc": "2024-12-31T18:00:00Z",
     "endsAtUtc": "2024-12-31T20:00:00Z"
   }
   ```
5. Clique em **Send**

### Variáveis de Ambiente

Use `{{baseUrl}}` nos requests:

```
GET {{baseUrl}}/events
POST {{baseUrl}}/events
```

---

## 🎨 Personalizar Collection

### Adicionar Testes Automatizados

No Postman, vá para **Tests** tab:

```javascript
// Verificar status code
pm.test("Status is 201 Created", function () {
    pm.response.to.have.status(201);
});

// Verificar response body
pm.test("Response has event ID", function () {
    var jsonData = pm.response.json();
    pm.expect(jsonData).to.have.property('id');
});

// Salvar ID para próximos requests
pm.environment.set("eventId", pm.response.json().id);
```

### Adicionar Pre-request Scripts

```javascript
// Gerar timestamp UTC
pm.environment.set("currentTimestamp", new Date().toISOString());
```

---

## 🔐 Autenticação (Futuro)

Quando adicionar autenticação à API:

1. Adicione variável ao environment:
   ```json
   {
     "key": "authToken",
     "value": "",
     "type": "secret"
   }
   ```

2. Configure no Collection:
   - Settings → Authorization
   - Type: Bearer Token
   - Token: `{{authToken}}`

---

## 🐛 Troubleshooting

### Collection vazia após import

**Solução:**
```sh
# Verificar se OpenAPI está acessível
curl http://localhost:5050/openapi/v1.json

# Se não, iniciar API
docker-compose up -d

# Aguardar 15 segundos e tentar novamente
```

### Erro "Cannot convert OpenAPI"

**Solução:**
```sh
# Instalar/atualizar conversor
npm install -g openapi-to-postmanv2

# Tentar novamente
generate-postman-collection.bat
```

### Requests retornam erro de conexão

**Solução:**
1. Verificar se API está rodando: `docker ps`
2. Verificar environment: `baseUrl` = `http://localhost:5050`
3. Testar no navegador: http://localhost:5050/scalar/v1

---

## 📚 Recursos Adicionais

### Postman

- [Documentação Postman](https://learning.postman.com/)
- [OpenAPI no Postman](https://learning.postman.com/docs/integrations/available-integrations/working-with-openAPI/)
- [Variáveis e Environments](https://learning.postman.com/docs/sending-requests/variables/)

### Ferramentas

- [openapi-to-postmanv2](https://github.com/postmanlabs/openapi-to-postman)
- [Postman CLI (newman)](https://learning.postman.com/docs/running-collections/using-newman-cli/command-line-integration-with-newman/)

---

## 🚀 Próximos Passos

1. **Automatizar Testes**
   - Adicionar scripts de teste em cada request
   - Criar test suites

2. **CI/CD Integration**
   - Usar Newman (Postman CLI) para rodar testes
   - Integrar no pipeline

3. **Documentação**
   - Adicionar descrições em cada request
   - Criar guias de uso

4. **Compartilhamento**
   - Publicar collection no Postman Workspace
   - Compartilhar com equipe

---

## 📞 Suporte

- **Issues:** [GitHub Issues](https://github.com/alexcanario/Evently/issues)
- **Docs da API:** http://localhost:3000 (Docusaurus)
- **Scalar UI:** http://localhost:5050/scalar/v1

---

**Última atualização:** Gerado automaticamente do OpenAPI
