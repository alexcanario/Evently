---
sidebar_position: 2
---

# Quick Start

Get Evently running in under 5 minutes!

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Visual Studio 2025](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

## Running with Docker Compose

1. **Clone the repository**
   ```bash
   git clone https://github.com/alexcanario/Evently.git
   cd Evently
   ```

2. **Start the containers**
   ```bash
   docker-compose up -d
   ```

3. **Access the API**
   - **Scalar UI**: http://localhost:5050/scalar/v1
   - **OpenAPI Spec**: http://localhost:5050/openapi/v1.json
   - **API Base**: http://localhost:5050

## Create Your First Event

Using the Scalar UI or curl:

```bash
curl -X POST http://localhost:5050/events \
  -H "Content-Type: application/json" \
  -d '{
    "title": "My First Event",
    "description": "Testing Evently API",
    "location": "Virtual",
    "startsAtUtc": "2024-12-31T18:00:00Z",
    "endsAtUtc": "2024-12-31T20:00:00Z"
  }'
```

## Next Steps

- [Docker Setup Guide](./guides/docker-setup.md)
- [Database Configuration](./guides/database-setup.md)
- [API Reference](/docs/api)
