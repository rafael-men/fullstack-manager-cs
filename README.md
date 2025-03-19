# Sistema de Gerenciamento de Processos Jurídicos

Este projeto foi desenvolvido com o objetivo de criar uma aplicação web completa para gerenciamento de processos jurídicos, com foco em agilizar o trabalho dos servidores públicos.

## Funcionalidades

O sistema permite:

- **Gerenciamento de Processos Jurídicos:** Cadastro, edição, exclusão e consulta de processos.
- **Controle de Prazos:** Acompanhamento de prazos importantes para cada processo.
- **Armazenamento de Documentos:** Vinculação de documentos essenciais aos processos.
- **Gerenciamento de Pessoas Envolvidas:** Cadastro e vinculação de procuradores e clientes aos processos.
- **Distribuição de Processos:** Possibilidade de redistribuir processos entre procuradores.

## Tecnologias Utilizadas

- **Back-end:** C# e ASP.NET 9
- **Front-end:** React.js + Vite
- **Banco de Dados:** SQL Server
- **Autenticação:** JWT (JSON Web Tokens)
- **Documentação da API:** Swagger


## Estrutura do Projeto

O projeto está dividido em três partes principais:

1. **Back-end:** API RESTful para gerenciamento de processos, procuradores, clientes e documentos.
2. **Front-end:** Interface web para interação com o sistema.
3. **Banco de Dados:** Modelo relacional para armazenamento de dados.

## Como Executar o Projeto

### Pré-requisitos

- .Net Framework instalado
- Sql Server instalado 
- Git instalado

### Passos para Execução

1. **Clone o repositório:**

   ```bash
   git clone https://github.com/seu-usuario/nome-do-projeto.git
   ```
## Backend

3.1 - Inicie o serviço do Sql Server a sua máquina.

3.2 - Rode os comandos

```bash
cd backend
cd main
dotnet run
```

3.3 - Execute as migrações ao banco (no Visual Studio vá em Ferramentas / Console do Ger. de Pacotes do Nuget)

```bash
add-migration Migração-para-o-Banco
update-database
```

3.3 - A aplicação iniciará em **http://localhost:5250/swagger/index.html**

## Frontend

4. Rodando a aplicação no docker

   ```bash
   docker compose up
   ```
5. Executando Localmente

5.1 - Execute os comandos 

```bash
cd frontend
npm i
npm run dev
```
5.2 - A Aplicação iniciará em **http://localhost:5173**

# Diagrama ER do Banco de Dados da aplicação abaixo 



