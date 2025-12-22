# 📮 Postman + Evently API - Quick Guide

## 🎯 3 Formas de Usar Postman

### 1️⃣ Import Direto (30 segundos) ⭐

```bash
# Iniciar API
docker-compose up -d
```

**No Postman:**
1. Import → Link
2. Cole: `http://localhost:5050/openapi/v1.json`
3. Import

**Pronto!** ✅

---

### 2️⃣ Gerar Collection Automaticamente (2 minutos)

```bash
# Na raiz do projeto
generate-postman-collection.bat
```

**No Postman:**
1. Import → Files
2. Selecione `postman/Evently-API.postman_collection.json`
3. Selecione `postman/Evently-Dev.postman_environment.json`
4. Import

---

### 3️⃣ Usar Arquivos Pré-gerados

Os arquivos já estão em `postman/`:
- ✅ Collection: `Evently-API.postman_collection.json`
- ✅ Environment: `Evently-Dev.postman_environment.json`

Basta importar no Postman!

---

## 🚀 Teste Rápido

### Criar Evento

1. **Selecione Environment:** `Evently - Development` (dropdown superior direito)

2. **Abra Request:**
   - `Evently API` → `Events` → `Create Event`

3. **Body (JSON):**
   ```json
   {
     "title": "Meu Primeiro Evento",
     "description": "Testando Postman",
     "location": "Virtual",
     "startsAtUtc": "2024-12-31T18:00:00Z",
     "endsAtUtc": "2024-12-31T20:00:00Z"
   }
   ```

4. **Send** ✅

**Response esperado:**
```json
{
  "id": "guid-aqui",
  "message": "Event created successfully!"
}
```

---

## 🔄 Atualizar Collection

Quando adicionar novos endpoints:

```bash
# 1. Rebuild API
docker-compose build
docker-compose up -d

# 2. Regenerar
generate-postman-collection.bat

# 3. No Postman: Import → Replace
```

---

## 📁 Arquivos

```
postman/
├── Evently-API.postman_collection.json     ← Collection
├── Evently-Dev.postman_environment.json    ← Environment
├── evently-openapi.json                    ← OpenAPI (gerado)
└── README.md                               ← Guia completo
```

---

## 🧪 Adicionar Testes

Na aba **Tests**:

```javascript
pm.test("Status is 201", () => {
    pm.response.to.have.status(201);
});

pm.test("Has event ID", () => {
    pm.expect(pm.response.json()).to.have.property('id');
});
```

---

## 🤖 CLI (Newman)

```bash
# Instalar
npm install -g newman

# Executar testes
newman run postman/Evently-API.postman_collection.json \
  -e postman/Evently-Dev.postman_environment.json
```

---

## 📚 Guias Completos

- **[Postman Collections](docs/docs/guides/postman-collections.md)** - Guia detalhado
- **[postman/README.md](postman/README.md)** - Documentação técnica
- **[Docusaurus Docs](http://localhost:3000/docs/guides/postman-collections)** - UI interativa

---

## ✅ Checklist

- [ ] Postman instalado
- [ ] API rodando (`docker-compose up -d`)
- [ ] Collection importada
- [ ] Environment selecionado
- [ ] Teste POST /events funcionando

---

**Simples assim! 🎉**
