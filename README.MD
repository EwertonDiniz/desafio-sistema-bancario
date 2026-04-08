## Como executar

### Pré-requisitos:
- .NET 8 SDK
- Docker + Docker Compose

```bash
# 1º Clone o repositório
git clone https://github.com/EwertonDiniz/desafio-sistema-bancario.git
cd desafio-sistema-bancario

# 2º Suba os containers
docker-compose up --build
```

# Acesse no navegador:

- Frontend: [login.html]
- Swagger ContaCorrenteAPI: [http://localhost:5001/swagger](http://localhost:5001/swagger)
- Swagger TransferenciaAPI: [http://localhost:5002/swagger](http://localhost:5002/swagger)

---

## 🧪 Rodando os testes

```bash
# Acesse a pasta do projeto
cd src/ContaCorrenteAPI.Tests
dotnet test

cd ../TransferenciaAPI.Tests
dotnet test
```