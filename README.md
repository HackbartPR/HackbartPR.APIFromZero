# API do Zero

Toda vez que precisamos criar uma API, acabamos implementando novamente diversas funcionalidades básicas, como padronização de respostas, idempotência, versionamento, autenticação, documentação, conexão com banco de dados, migrations, entre outras.

Este projeto possui o intuito de centralizar todas essas implementações para os devs .NET, servindo como um projeto base para a criação de qualquer API profissional.

O objetivo é disponibilizar um repositório que possa ser utilizado como ponto de partida (startup) para o desenvolvimento de APIs, evitando a necessidade de recriar essa estrutura a cada novo projeto.

Cada commit corresponde a um post da série, permitindo acompanhar a evolução do projeto passo a passo por meio do histórico de commits.

## Conteúdo da Série

### 1º Post — Estrutura Inicial da API
- ✅ Criando a Base da API **[Camada API]**
- ✅ Criando um Base Response simples **[Camada API]**
- ✅ Criando um Base Controller **[Camada API]**
- ✅ Versionando a API **[Camada API]**
- ✅ Configurando Scalar na API **[Camada API]**

---

### 2º Post — Configurações e Infraestrutura
- ⏳ Trabalhando com padrão **IOptions** para ler variáveis de ambiente e Secrets **[Camada API]**
- ⏳ Criando um Middleware para tratamento de Exceptions **[Camada API]**
- ⏳ Conectando a um servidor de Cache (Redis) **[Camada Infrastructure]**
- ⏳ Criando um Middleware de Idempotência **[Camada API]**

---

### 3º Post — Banco de Dados
- ⏳ Conectando ao Entity Framework Core **[Camada Infrastructure]**
- ⏳ Health Check do Servidor/Banco de Dados **[Camada API]**

---

### 4º Post — Migrations
- ⏳ Serviço separado para execução das Migrations

---

### 5º Post — Identity
- ⏳ Iniciando com Identity para gerenciamento dos usuários **[Camada API]**
- ⏳ Personalizando a entidade User do Identity **[Camada Domain] [Camada Infrastructure]**
- ⏳ Seeds para usuários iniciais **[Camada Infrastructure]**

---

### 6º Post — Autenticação
- ⏳ Criar endpoint de Login separado do Identity **[Camada API]**
- ⏳ Criar endpoint de Logout separado do Identity **[Camada API]**
- ⏳ Gerar Authentication Token e Refresh Token com JWT **[Camada API]**
- ⏳ Criar Cookie HttpOnly para armazenar o JWT **[Camada API]**
- ⏳ Criar endpoint de Refresh Token separado do Identity **[Camada API]**

---

### 7º Post — Cadastro de Usuários
- ⏳ Criar endpoint de Register separado do Identity **[Camada API]**
- ⏳ Conectar a um servidor SMTP **[Camada Infrastructure]**
- ⏳ Enviar e-mail de confirmação no registro do usuário **[Camada API]**
- ⏳ Criar endpoint de Change Password **[Camada API]**

---

### 8º Post — Paginação e Exemplo de Caso de Uso
- ⏳ Criar Base Response Paginado **[Camada API]**
- ⏳ Exemplo de Caso de Uso de Paginação **[Camada Application] [Camada Domain]**
- ⏳ Exemplo de um cadastro completo **[Camada Application] [Camada Domain]**


**Com todas as features citadas acima, teremos uma base sólida para iniciar a construção das regras de negócio e colocar a API em produção.**

---

# Próximas Features (Extras)

Estas funcionalidades serão adicionadas futuramente:

- Trabalhando com Repositories
- Trabalhando com Unit of Work
- Trabalhando com Dapper