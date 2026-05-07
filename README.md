# FinTrack API

API REST de controle financeiro pessoal construída em .NET 9, aplicando Clean Architecture, CQRS e boas práticas de desenvolvimento de software.

---

## Tecnologias

- **.NET 9 / C# 13**
- **Clean Architecture + CQRS + MediatR**
- **PostgreSQL + Dapper** (queries) **+ EF Core** (contexto)
- **JWT Authentication + Refresh Tokens** com rotação automática
- **Argon2id** para hashing de senhas
- **FluentValidation + Pipeline Behaviors**
- **Serilog** para logging estruturado
- **Scalar** para documentação interativa da API
- **xUnit + Moq + FluentAssertions** para testes

---

## Estrutura do projeto

```
src/
├── 01-Domain/          # Entidades, Value Objects, Guard, Result pattern
├── 02-Application/     # Commands, Queries, Handlers, Validators, Behaviors
├── 03-Infrastructure/  # Repositórios, Dapper, JWT, Argon2, CurrentUserService
└── 04-Presentation/    # Controllers, GlobalExceptionHandler, Extensions

tests/
├── Domain.UnitTests/       # Testes de entidades, Value Objects e Result pattern
└── Application.UnitTests/  # Testes de handlers e behaviors com Moq
```

---

## Funcionalidades

**Autenticação**
- Registro e login de usuários
- JWT com expiração configurável
- Refresh token com rotação automática (token antigo é revogado a cada novo login)
- Rehash automático de senha quando os parâmetros de segurança mudam
- Logout com revogação de todos os tokens do usuário

**Contas** (`finance.accounts`)
- CRUD completo de contas financeiras
- Soft delete (desativa a conta sem remover do banco)
- Saldo inicial e saldo atual separados

**Categorias** (`catalog.categories`)
- CRUD com suporte a PATCH parcial
- Vinculadas ao usuário autenticado

**Transações** (`finance.transactions`)
- Criação, listagem e PATCH de transações
- Soft delete
- Sumário financeiro por período (total de receitas, despesas e saldo)

**Transações Recorrentes** (`finance.recurring_transactions`)
- CRUD completo com PATCH parcial
- Controle de frequência, data de início, fim e próxima ocorrência

---

## Decisões de design

**Result pattern** — toda operação de negócio retorna `Result` ou `Result<T>` em vez de lançar exceções. Erros de domínio são valores, não exceções.

**Guard clauses** — validações de domínio centralizadas com `Guard.AgainstNullOrWhiteSpace` e `Guard.AgainstOutOfRange`, encadeadas via `Bind`.

**Value Objects** — `Email` e `Price` encapsulam validação e normalização. Dapper lida com eles via `TypeHandler` customizado (`EmailTypeHandler`, `PriceTypeHandler`).

**CQRS com separação real** — repositórios de escrita (`IRepository`) separados de queries de leitura (`IQueries`). Queries retornam DTOs leves (`ListItem`), repositórios retornam entidades completas.

**Pipeline Behaviors** — `ValidationBehavior` executa FluentValidation antes de cada handler. `LoggingBehavior` loga entrada, saída, tempo de execução e erros de domínio automaticamente para todos os requests.

**Argon2id** para senhas — parâmetros configuráveis (iterações, memória, paralelismo) com detecção automática de necessidade de rehash.

**Soft delete** — contas e transações recorrentes não são removidas fisicamente, apenas desativadas com `is_active = false`.

---

## Como rodar

> **Pré-requisitos:** .NET SDK 9, PostgreSQL rodando localmente.

**1. Clone o repositório:**
```bash
git clone https://github.com/hifye/FinTrack.git
cd FinTrack
```

**2. Configure o banco de dados:**

Execute os scripts SQL para criar os schemas e tabelas:

```sql
-- Schemas
CREATE SCHEMA IF NOT EXISTS auth;
CREATE SCHEMA IF NOT EXISTS catalog;
CREATE SCHEMA IF NOT EXISTS finance;

-- auth.users
CREATE TABLE auth.users (
    id            UUID        PRIMARY KEY DEFAULT gen_random_uuid(),
    name          VARCHAR(150) NOT NULL,
    email         VARCHAR(100) NOT NULL UNIQUE,
    password_hash TEXT        NOT NULL,
    created_at    TIMESTAMP   NOT NULL
);

-- auth.refresh_tokens
CREATE TABLE auth.refresh_tokens (
    id         UUID        PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id    UUID        NOT NULL REFERENCES auth.users(id) ON DELETE CASCADE,
    token      TEXT        NOT NULL UNIQUE,
    expires_at TIMESTAMPTZ NOT NULL,
    is_revoked BOOLEAN     NOT NULL DEFAULT FALSE,
    created_at TIMESTAMPTZ NOT NULL
);

-- catalog.categories
CREATE TABLE catalog.categories (
    id         UUID         PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id    UUID         NOT NULL REFERENCES auth.users(id) ON DELETE CASCADE,
    name       VARCHAR(100) NOT NULL,
    type       VARCHAR(100) NOT NULL,
    is_active  BOOLEAN      NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP    NOT NULL
);

-- finance.accounts
CREATE TABLE finance.accounts (
    id              UUID          PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID          NOT NULL REFERENCES auth.users(id) ON DELETE CASCADE,
    name            VARCHAR(50)   NOT NULL,
    type            VARCHAR(20)   NOT NULL,
    initial_balance NUMERIC(18,2) NOT NULL,
    current_balance NUMERIC(18,2) NOT NULL,
    is_active       BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at      TIMESTAMP     NOT NULL
);

-- finance.transactions
CREATE TABLE finance.transactions (
    id               UUID          PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id          UUID          NOT NULL REFERENCES auth.users(id) ON DELETE CASCADE,
    account_id       UUID          NOT NULL REFERENCES finance.accounts(id) ON DELETE RESTRICT,
    category_id      UUID          NOT NULL REFERENCES catalog.categories(id) ON DELETE RESTRICT,
    recurring_id     UUID,
    amount           NUMERIC(18,2) NOT NULL,
    type             VARCHAR(100)  NOT NULL,
    description      VARCHAR(250),
    transaction_date TIMESTAMP     NOT NULL,
    created_at       TIMESTAMP     NOT NULL,
    updated_at       TIMESTAMP
);

-- finance.recurring_transactions
CREATE TABLE finance.recurring_transactions (
    id              UUID          PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id         UUID          NOT NULL REFERENCES auth.users(id) ON DELETE CASCADE,
    account_id      UUID          NOT NULL REFERENCES finance.accounts(id) ON DELETE RESTRICT,
    category_id     UUID          NOT NULL REFERENCES catalog.categories(id) ON DELETE RESTRICT,
    amount          NUMERIC(18,2) NOT NULL,
    type            VARCHAR(100)  NOT NULL,
    description     VARCHAR(250),
    frequency       VARCHAR(50)   NOT NULL,
    start_date      DATE          NOT NULL,
    end_date        DATE          NOT NULL,
    next_occurrence DATE          NOT NULL,
    is_active       BOOLEAN       NOT NULL DEFAULT TRUE,
    created_at      TIMESTAMP     NOT NULL
);
```

**3. Configure o `appsettings.json`:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=fintrack;Username=postgres;Password=sua_senha"
  },
  "JWT": {
    "Key": "sua_chave_secreta_com_no_minimo_32_caracteres",
    "Issuer": "FinTrackApi",
    "Audience": "FinTrackClient",
    "ExpirationInDays": 1,
    "RefreshTokenExpirationInDays": 7
  }
}
```

**4. Rode a API:**
```bash
cd src/04-Presentation/FinTrack
dotnet run
```

Scalar UI disponível em: **https://localhost:{porta}/scalar/v1**

---

## Endpoints

### Auth — `/api/Auth`

| Método | Rota | Descrição | Auth |
|--------|------|-----------|------|
| POST | `/register` | Registra novo usuário | ❌ |
| POST | `/login` | Autentica e retorna JWT + refresh token | ❌ |
| POST | `/refresh-token` | Renova o access token | ✅ |
| POST | `/logout` | Revoga todos os tokens do usuário | ✅ |
| PATCH | `/password` | Atualiza senha do usuário autenticado | ✅ |

### Catalog — `/api/Catalog`

| Método | Rota | Descrição | Auth |
|--------|------|-----------|------|
| GET | `/category/{id}` | Detalhes de uma categoria | ✅ |
| GET | `/categories` | Lista todas as categorias do usuário | ✅ |
| POST | `/category` | Cria uma nova categoria | ✅ |
| PATCH | `/categories/{id}` | Atualiza parcialmente uma categoria | ✅ |
| DELETE | `/category/{id}` | Remove uma categoria | ✅ |

### Finance — `/api/Finance`

| Método | Rota | Descrição | Auth |
|--------|------|-----------|------|
| GET | `/account/{id}` | Detalhes de uma conta | ✅ |
| GET | `/accounts` | Lista todas as contas do usuário | ✅ |
| POST | `/account` | Cria uma nova conta | ✅ |
| PATCH | `/accounts/{id}` | Atualiza parcialmente uma conta | ✅ |
| DELETE | `/account/{id}` | Desativa uma conta (soft delete) | ✅ |
| GET | `/transaction/{id}` | Detalhes de uma transação | ✅ |
| GET | `/transactions` | Lista todas as transações do usuário | ✅ |
| GET | `/transaction-summary` | Sumário financeiro por período | ✅ |
| POST | `/transaction` | Cria uma nova transação | ✅ |
| PATCH | `/transactions/{id}` | Atualiza parcialmente uma transação | ✅ |
| DELETE | `/transaction/{id}` | Remove uma transação | ✅ |
| GET | `/recurring-transaction/{id}` | Detalhes de uma transação recorrente | ✅ |
| GET | `/recurring-transactions` | Lista todas as transações recorrentes | ✅ |
| POST | `/recurring-transaction` | Cria uma transação recorrente | ✅ |
| PATCH | `/recurring-transactions/{id}` | Atualiza parcialmente uma transação recorrente | ✅ |
| DELETE | `/recurring-transactions/{id}` | Desativa uma transação recorrente | ✅ |

---

## Testes

```bash
# Testes de domínio
cd tests/Domain.UnitTests
dotnet test

# Testes de aplicação
cd tests/Application.UnitTests
dotnet test
```

**Domain.UnitTests** cobre: `Result` pattern, `Guard`, Value Objects (`Email`, `Price`), entidades (`User`, `Category`, `Account`, `Transaction`).

**Application.UnitTests** cobre: handlers de Auth (Register, Login), handlers de Catalog (CreateCategory, DeleteCategory, GetCategoriesByUserId), handlers de Finance (CreateAccount, PatchAccount, CreateTransaction, GetTransactionDetails) e `LoggingBehavior`.

---

## Fluxo de autenticação

```
1. POST /register      → cria usuário com Argon2id hash
2. POST /login         → valida credenciais, revoga tokens antigos, retorna JWT + RefreshToken
3. GET  /accounts      → Authorization: Bearer {accessToken}
4. POST /refresh-token → quando o JWT expirar, usa o RefreshToken para obter novos tokens
5. POST /logout        → revoga todos os RefreshTokens do usuário
```

---

## Logging

Serilog configurado com três outputs:
- **Console** — todos os níveis em desenvolvimento
- **logs/log-{data}.log** — rolling diário, retém 7 arquivos, limite de 10MB por arquivo
- **logs/errors-{data}.log** — somente erros, rolling diário

O `LoggingBehavior` loga automaticamente cada request MediatR com: nome do comando/query, payload, tempo de execução, resultado (sucesso ou erro com código) e aviso quando a execução ultrapassa 500ms.
