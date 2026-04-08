## 🐳 Como executar

### Pré-requisitos:
- .NET 8 SDK
- Docker + Docker Compose

```bash
# Clone o repositório
git clone https://github.com/renanmunizdev/BankMore.git
cd BankMore

# Suba os containers
docker-compose up --build
```

### Acesse no navegador:


- ContaCorrenteAPI: [http://localhost:5001/swagger](http://localhost:5001/swagger)
- TransferenciaAPI: [http://localhost:5002/swagger](http://localhost:5002/swagger)

---

## 🧪 Rodando os testes

```bash
# Acesse a pasta do projeto
cd src/ContaCorrenteAPI.Tests
dotnet test

cd ../TransferenciaAPI.Tests
dotnet test
```

---

## 🔄 CI/CD

Você pode configurar um pipeline de CI/CD com GitHub Actions, GitLab CI ou Azure Pipelines. Sugestões:

- Build da solution
- Execução dos testes
- Docker build + push (ex: Docker Hub)
- Deploy (Kubernetes, Azure Web App, etc.)

---

## ⚠️ Observações

- A implementação de mensageria com Kafka foi planejada, mas não concluída por limite de tempo. O projeto já está preparado para integração futura com KafkaFlow.
- A solução está em SQLite para facilitar testes, mas é facilmente adaptável para SQL Server ou PostgreSQL.

---

## 👨‍💻 Autor

Desenvolvido por **Renan Muniz**  
Contato: [LinkedIn](https://www.linkedin.com/in/renanmuniz86) · [GitHub](https://github.com/renanmunizdev)

---

## 📄 Licença

Este projeto está sob a licença MIT. Sinta-se à vontade para usar e contribuir!
