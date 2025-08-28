# README — API de Moderação de Texto

Sistema ASP.NET Core para análise e registro de textos potencialmente ilícitos/violentos. Oferece endpoints para verificar um texto via IA (Gemini), armazenar logs em SQL Server e consultar os registros.

>> site do projeto: que esta na digital ocean: https://www.edinaelson.xyz/swagger/index.html

>> NameCheap: https://www.namecheap.com/domains/registration/results/?domain=edinaelson.xyz

## Tecnologias

- .NET 8 (C# 12)
- ASP.NET Core (Web API)
- Entity Framework Core (SQL Server)
- Swagger/Swashbuckle (documentação e testes dos endpoints)
- Docker e Docker Compose
- SQL Server 2022 (container oficial Microsoft)

## ntegração com Gemini (Google)
- Site oficial (docs): [https://ai.google.dev/](https://ai.google.dev/)
- Documentação da Gemini API: [https://ai.google.dev/gemini-api/docs](https://ai.google.dev/gemini-api/docs)
- Obter chave de API (AI Studio): [https://aistudio.google.com/app/apikey](https://aistudio.google.com/app/apikey)
- Modelos disponíveis: [https://ai.google.dev/gemini-api/docs/models](https://ai.google.dev/gemini-api/docs/models)
- Políticas e segurança (Safety): [https://ai.google.dev/gemini-api/docs/safety](https://ai.google.dev/gemini-api/docs/safety)
- Políticas de uso: [https://ai.google.dev/policies](https://ai.google.dev/policies)
- Termos de uso: [https://ai.google.dev/terms](https://ai.google.dev/terms)
- Preços: [https://ai.google.dev/pricing](https://ai.google.dev/pricing)
- Cotas e limites: [https://ai.google.dev/gemini-api/docs/quotas](https://ai.google.dev/gemini-api/docs/quotas)

Observações:
- Defina a variável de ambiente Gemini:GOOGLE_API_KEY. Sem ela, a API não inicia.
- Em produção, armazene a chave de forma segura (secret manager, variáveis de ambiente, vault).

## Arquitetura (visão geral)

- API exposta em HTTP com endpoints REST.
- Camada de persistência com EF Core, banco SQL Server.
- Cliente de moderação baseado no serviço Gemini (Google) via HttpClient.
- Documentação interativa via Swagger UI.

## Endpoints principais

Base: /api/Moderation

- POST /api/Moderation
    - Body: string (texto a ser analisado)
    - Retorna: objeto com o texto e o resultado da moderação (true/false)
    - Também registra o log no banco.

- GET /api/Moderation/true
    - Retorna todos os logs marcados como verdadeiros (ilícito/violento).

- GET /api/Moderation/false
    - Retorna todos os logs marcados como falsos (não ilícito/violento).

- GET /api/Moderation/byday?date=YYYY-MM-DD
    - Filtra logs por dia (data em formato ISO, ex.: 2025-08-27).

- GET /api/Moderation/raw-count
    - Retorna o total bruto de registros na tabela de logs.

A documentação interativa (Swagger UI) fica em:
- Ambiente local: https://localhost:PORT/swagger
- Docker: http://localhost:7007/swagger

## Pré-requisitos

- Docker e Docker Compose
- (Opcional, para rodar localmente sem Docker) .NET SDK 8.0

## Variáveis de ambiente (importante)

- Gemini: GOOGLE_API_KEY
    - Chave obrigatória para o cliente de moderação. A API não sobe sem essa variável.
- ConnectionStrings__DefaultConnection
    - String de conexão SQL Server usada pela aplicação.

Exemplo de string para desenvolvimento com Docker (sem criptografia):
- Server=sqlserver,1433;Database=ModerationDb;User Id=sa;Password=SUA_SENHA;Encrypt=False;

Exemplo com criptografia habilitada:
- Server=sqlserver,1433;Database=ModerationDb;User Id=sa;Password=SUA_SENHA;Encrypt=True;TrustServerCertificate=True;

Atenção à grafia: o parâmetro correto é TrustServerCertificate (sem “ed”). Grafias incorretas causam erro de runtime.

## Como executar com Docker Compose (recomendado)

1) Configure as variáveis necessárias
- Ajuste a senha do SA do SQL Server e a connection string no serviço da API, se preciso.
- Garanta que a variável de ambiente Gemini: GOOGLE_API_KEY esteja disponível para a API (por exemplo, exportando-a antes do up ou adicionando ao compose com segurança).

2) Suba os serviços
- docker compose up -d --build

3) Verifique
- API: http://localhost:7007
- Swagger: http://localhost:7007/swagger
- SQL Server: localhost:1433 (usuário: sa, senha conforme definida)

4) Teste rápido (exemplos)
- POST de análise:
  curl -X POST "http://localhost:7007/api/Moderation" -H "Content-Type: application/json" -d "\"Texto para analisar\""
- GET logs verdadeiros:
  curl "http://localhost:7007/api/Moderation/true"
- GET por dia:
  curl "http://localhost:7007/api/Moderation/byday?date=2025-08-27"

## Como executar localmente (sem Docker)

1) Configure a connection string
- Defina ConnectionStrings:DefaultConnection (em appsettings.Development.json, user-secrets ou variáveis de ambiente).
- Para desenvolvimento local com SQL Server local/containers, você pode usar:
    - Encrypt=False; (mais simples para dev)
    - ou Encrypt=True;TrustServerCertificate=True; (se quiser tráfego criptografado com certificado não confiável)

2) Configure a chave do Gemini
- Defina a variável Gemini:GOOGLE_API_KEY via user-secrets ou variáveis de ambiente.

3) Restaure, compile e execute
- dotnet restore
- dotnet build
- dotnet run

4) Acesse
- Swagger: https://localhost:PORT/swagger

## Banco de dados e migrações

- Se você usa migrações do EF Core, aplique-as antes de rodar:
    - dotnet ef database update
- Caso ainda não tenha migrações:
    - dotnet ef migrations add InitialCreate
    - dotnet ef database update
- Ajuste conforme sua convenção de pastas e projeto.

## Boas práticas e segurança

- Não registre secrets em logs em produção (ex.: connection strings e chaves de API).
- Use usuários e permissões mínimos no SQL Server para produção.
- Padronize a configuração entre ambientes (appsettings, variáveis de ambiente e compose) para evitar conflitos.
- Se optar por criptografia na conexão, use certificados apropriados em produção (idealmente certificados válidos, não apenas TrustServerCertificate=True).

## Solução de problemas (FAQ)

- Erro “Keyword not supported: 'trustedservercertificate'”:
    - Verifique se a connection string tem a chave correta: TrustServerCertificate (sem “ed”).
    - Alternativamente, em desenvolvimento, use Encrypt=False; para evitar o parâmetro.
    - Confirme qual fonte de configuração está prevalecendo (variáveis de ambiente, appsettings, user-secrets).

- Não consigo acessar o Swagger:
    - Confira a porta mapeada. No Docker, a API está em http://localhost:7007.
    - Verifique se o container está em “healthy” e os logs do container da API.

- Falha ao conectar no SQL Server:
    - Aguarde alguns segundos após subir o compose (SQL Server leva um tempo para ficar pronto).
    - Verifique credenciais e porta (1433).
    - Teste conexão com uma ferramenta cliente (ex.: Azure Data Studio).

