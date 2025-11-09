# 🚀 Projeto Challenger API (.NET 8)

Este repositório contém a implementação de uma **API desenvolvida em .NET 8**, estruturada em camadas de acordo com princípios de **Domain-Driven Design (DDD)**.
O projeto foi criado como parte da Sprint 4 com objetivo de aplicar boas práticas de desenvolvimento, versionamento, arquitetura de software e pipelines e Releases DevOps.

---

## 🎯 Objetivos do Projeto

* Implementar uma API REST em **.NET** com arquitetura organizada em camadas.
* Aplicar conceitos de **Domain-Driven Design (DDD) e SOLID**.
* Estruturar as camadas **Domain, Application, Infrastructure e API**.
* Automatizar build, restore e deploy usando **Azure DevOps Pipelines**.
* Permitir fácil execução local e testes da aplicação.

---

## 🛠️ Estrutura do Projeto

```
NET-MOTTU-main/
│
├── Challenger.API/            # Camada de apresentação (Controllers e Startup)
├── Challenger.Application/    # Casos de uso e regras de aplicação
├── Challenger.Domain/         # Entidades e regras de negócio
├── Challenger.Infrastructure/ # Persistência e integrações externas
│
├── Challenger.sln             # Arquivo da solução .NET
├── global.json
└── .gitignore
```

---

## ▶️ Executar Localmente

### 📌 Pré-requisitos

* [.NET SDK 8+](https://dotnet.microsoft.com/en-us/download)
* [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)
* Banco de dados SQL Server / MySQL disponível

### 📥 Clonar o repositório

```bash
git clone https://github.com/Challenger-MOTTU/Devops-Mottu.git
cd NET-MOTTU-main
```

### ⚙️ Restaurar dependências

```bash
dotnet restore
```

### Configure a string de conexão no `appsettings.json`

```json
"ConnectionStrings": {
  "MotoGridDB": "Server=tcp:devpos.database.windows.net,1433;Initial Catalog=devops;Persist Security Info=False;User ID=admin_fiap;Password={sua senha};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
}
```

### ▶️ Executar a API

```bash
cd Challenger.API
dotnet run
```

A API ficará disponível por padrão em:

```
https://localhost:5001
http://localhost:5000
```

### ✅ Testar a aplicação

* [Postman](https://www.postman.com/)
* `curl` no terminal
* Navegador para endpoints GET

### 📦 Exemplos de Requisição

**POST /api/patios**

```json
{
  "name": "Pátio Centro",
  "cidade": "São Paulo",
  "capacidade": 50
}
```

**GET /api/patios/cidade/São Paulo**

**POST /api/motos**

```json
{
  "placa": "XYZ-1234",
  "modelo": 1,
  "status": 0,
  "patioId": "GUID_DO_PATIO"
}
```

**GET /api/motos/placa/XYZ-1234**

**POST /api/users**

```json
{
  "username": "DustSams",
  "email" : "victorhugo@gmail.com",
  "senha" : "Fiapm1234"
}
```

---

## ▶️ Azure DevOps Pipeline – Passo a Passo

### 1️⃣ Pré-requisitos

* Projeto .NET 8 configurado (`global.json` ou SDK 8 instalado)
* Azure DevOps Account + Pipeline criado
* App Service no Azure criado (B1 Basic OK) com **.NET 8 (LTS)**
* Banco de dados SQL Server / Azure SQL configurado

### 2️⃣ Pipeline Básico

**Tarefas sugeridas:**

1. **NuGet Restore**

   * Use task **NuGet** ou `dotnet restore`

2. **Build**

```yaml
- task: DotNetCoreCLI@2
  inputs:
    command: 'build'
    projects: '**/*.sln'
    arguments: '--configuration Release'
```

3. **Run Migrations (opcional)**

```bash
dotnet ef database update --project Challenger.Infrastructure/Challenger.Infrastructure.csproj --startup-project Challenger.API/Challenger.API.csproj
```

4. **Publish**

```yaml
- task: DotNetCoreCLI@2
  inputs:
    command: 'publish'
    publishWebProjects: true
    arguments: '--configuration Release --output $(Build.ArtifactStagingDirectory)'
    zipAfterPublish: true
```

5. **Publish Artifacts**

```yaml
- task: PublishBuildArtifacts@1
  inputs:
    pathToPublish: '$(Build.ArtifactStagingDirectory)'
    artifactName: 'drop'
```

6. **Deploy – Azure Web App**

```yaml
- task: AzureWebApp@1
  inputs:
    azureSubscription: '<service-connection>'
    appName: '<nome-do-appservice>'
    package: '$(Build.ArtifactStagingDirectory)/**/*.zip'
```

### 3️⃣ Configuração do App Service

* **Runtime Stack:** .NET 8 (LTS)
* **Deployment Mode:** ZIP Deploy
* **Application Settings:** `ConnectionStrings:MotoGridDB`
* Logs podem ser acessados via **Kudu** (`https://<appservice>.scm.azurewebsites.net/DebugConsole`)

### 4️⃣ Possíveis Erros e Soluções

| Erro                   | Causa                           | Solução                                           |
| ---------------------- | ------------------------------- | ------------------------------------------------- |
| `MSB4236`              | MSBuild incompatível            | Atualizar Visual Studio ou global.json            |
| `NuGet config invalid` | NuGet.config incorreto          | Corrigir path ou autenticação                     |
| `ZIP Deploy failed`    | Runtime ou package incompatível | Verificar runtime no App Service, refazer publish |
| 404 API endpoints      | Rotas incorretas                | Checar `[Route]` e launchSettings.json            |

---

## 👥 Integrantes

* **Gabriel Gomes Mancera** - RM:555427
* **Juliana de Andrade Sousa** - RM:558834
* **Victor Hugo Carvalho Pereira** - RM:558550
