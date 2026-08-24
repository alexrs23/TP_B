# TP_B - Gestão de Loja (Blazor + API + Backend)

Repositório com aplicação Blazor, controlo de identidade e uma API REST.

## Visão geral

Projetos principais:
- GestaoLoja: aplicação principal (Blazor / Identity) com configuração de autofalsificação e inicialização de dados.
- BlazorApp: exemplo/cliente Blazor (componentes e páginas).
- APIRestFull: projeto API REST (controllers e configuração para EF/Identity).
- RCLComum e RCLProdutos: bibliotecas Razor (component libraries) usadas pela UI.

O projeto usa Entity Framework Core para persistência e ASP.NET Core Identity para autenticação/roles.

## Pré-requisitos

- .NET 8 SDK
- Visual Studio 2022/2026 ou Visual Studio Code
- SQL Server ou outro provedor (configurar ConnectionString)

## Configuração

1. Definir a connection string no ficheiro de configuração do projeto `GestaoLoja` (appsettings.json) ou por User Secrets (o projeto já tem `UserSecretsId`).
   - Chave esperada: `ConnectionStrings:DefaultConnection`

2. Ajustar a base de dados conforme desejar (SQL Server/Sqlite/etc.).

3. (Opcional) Atualizar outras configurações em `AppConfig` via appsettings.

## Base de dados e migrações

Usando a CLI do dotnet/EF:

1. Adicionar migração (se necessário):

   dotnet ef migrations add InitialCreate --project GestaoLoja --startup-project GestaoLoja

2. Aplicar migrações:

   dotnet ef database update --project GestaoLoja --startup-project GestaoLoja

Observação: os comandos acima assumem que o projeto `GestaoLoja` é o projeto que contém o DbContext e deve ser o startup project.

## Executar a aplicação

Via Visual Studio:
- Abrir a solução `TP_B.sln` e definir `GestaoLoja` (ou outro projeto desejado) como startup project.
- Executar (F5 / Ctrl+F5).

Via CLI:

dotnet build
dotnet run --project GestaoLoja

O `Program.cs` já invoca `Inicializacao.CriaDadosIniciais` durante a inicialização para criar roles e um usuário administrador padrão.

## Estrutura do repositório

- /GestaoLoja - Aplicação principal (Blazor + Identity)
- /BlazorApp - Cliente/Exemplo Blazor
- /APIRestFull - API REST
- /RCLComum - Biblioteca de componentes comuns
- /RCLProdutos - Biblioteca de componentes de produtos
