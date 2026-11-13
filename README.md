# Verificador de idade

Aplicação de exercício criada com Angular 19 e TypeScript.

## Executar

```bash
npm install
npm start
```

## Funcionamento

Informe uma idade no campo da aplicação. O componente recebe esse valor
por meio de um `input` signal, iniciado em `0`, e usa o bloco `@if` do
template `src/app/verificador-idade/verificador-idade.component.html` para
exibir o resultado:

- Para idade maior ou igual a 18, exibe **Você é maior de idade.**
- Para idade menor que 18, exibe **Você é menor de idade.**

## Caso de uso: verificar idade com o back-end

### Definições técnicas

- **Framework:** ASP.NET Core MVC.
- **Linguagem:** C# 14.
- **SDK previsto:** .NET 10 SDK, versão 10.0.403.
- **SDK usado nesta etapa:** .NET 10 SDK, versão 10.0.402 (instalado no
  ambiente). O projeto tem como alvo `net10.0`; esta diferença de patch foi
  registrada e não altera o framework de destino.

### Comunicação e contrato da API

A aplicação Angular e o back-end trocarão dados por uma API HTTP que utiliza
JSON. O Angular enviará a idade para processamento, e o back-end devolverá
uma resposta JSON com a mensagem. Para esta operação será utilizado o método
`POST`, pois o cliente está enviando um dado para ser processado.

#### Requisição

- **Método:** `POST`
- **Rota:** `/api/verificacao-idade`
- **Cabeçalho:** `Content-Type: application/json`
- **Corpo:**

```json
{
  "idade": 18
}
```

#### Resposta de sucesso

O back-end responderá com o status HTTP `200 OK` e um objeto JSON contendo a
mensagem correspondente à idade recebida.

Para idade maior ou igual a 18:

```json
{
  "mensagem": "Você é maior de idade."
}
```

Para idade menor que 18:

```json
{
  "mensagem": "Você é menor de idade."
}
```

#### Responsabilidades na comunicação

- **Angular:** coleta a idade, envia o `POST` e apresenta a mensagem recebida.
- **Back-end:** recebe a idade, aplica a regra de maioridade e retorna a
  resposta JSON.
- **HTTP/JSON:** define o protocolo e o formato compartilhado para a troca
  de dados entre as aplicações.

#### Execução em desenvolvimento

Angular e back-end podem ser executados em origens diferentes, por exemplo,
Angular em `http://localhost:4200` e back-end em `http://localhost:5187`.
Como o navegador restringe requisições entre origens distintas, o back-end
está configurado para permitir a origem local do Angular (`http://localhost:4200`)
em desenvolvimento. A configuração para outros ambientes está descrita na
Task 5.

### Objetivo

Evoluir o exercício para praticar a integração entre Angular e um back-end
simples em ASP.NET Core MVC com C# 14 e .NET 10. O endpoint do back-end aplica
a regra de maioridade; na integração, o Angular deve enviar a idade e exibir a
resposta, sem duplicar essa regra de negócio.

### Atores

- **Usuário:** informa a idade e consulta o resultado.
- **Aplicação Angular:** envia a idade ao back-end e apresenta o resultado
  recebido.
- **Back-end ASP.NET Core MVC:** valida a idade e devolve a mensagem
  correspondente.

### Fluxo principal

1. O usuário informa uma idade na aplicação Angular.
2. O Angular envia a idade ao back-end ASP.NET Core MVC.
3. O back-end avalia a idade recebida:
   - Se a idade for maior ou igual a 18, responde **Você é maior de idade.**
   - Se a idade for menor que 18, responde **Você é menor de idade.**
4. O Angular apresenta ao usuário a resposta recebida, sem repetir a regra
   de maioridade no front-end.

### Resultado esperado

A regra de negócio para determinar a maioridade fica centralizada no
back-end em C#. A aplicação Angular fica responsável por coletar a idade,
solicitar a verificação e exibir o resultado.

## Tasks de implementação do back-end

As tasks abaixo organizam o trabalho de construção do back-end. As Tasks 1 a 7
foram concluídas.

### Task 1 — Criar o projeto ASP.NET Core MVC (concluída)

O projeto ASP.NET Core MVC foi criado em
`backend/VerificadorIdade.Api/`, separado da aplicação Angular, com alvo
`net10.0`. Como o ambiente disponibiliza o SDK `10.0.402`, ele foi usado para
gerar, compilar e executar o projeto; a versão prevista `10.0.403` ainda não
foi usada.

#### Estrutura criada

- `VerificadorIdade.Api.csproj`: projeto web ASP.NET Core com alvo `net10.0`,
  nullable reference types e implicit usings habilitados.
- `Program.cs`: inicializa a aplicação, registra MVC com
  `AddControllersWithViews`, configura o pipeline HTTP e o roteamento MVC.
- `Controllers/HomeController.cs` e `Models/ErrorViewModel.cs`: controller
  e modelo de exemplo gerados pelo template MVC.
- `Views/`: páginas Razor iniciais (Home, Privacy, Error e layout
  compartilhado).
- `wwwroot/`: arquivos estáticos e bibliotecas do template (Bootstrap,
  jQuery e validação).
- `appsettings.json` e `appsettings.Development.json`: configuração inicial
  de logging e ambiente.
- `Properties/launchSettings.json`: perfil `http` para execução local em
  `http://localhost:5187`.

O template MVC disponibiliza o suporte a controllers e serialização JSON da
plataforma. Os modelos, endpoint e validação da API estão descritos nas Tasks
2 a 4.

#### Executar e validar

Na raiz do repositório, use:

```powershell
dotnet build .\backend\VerificadorIdade.Api\VerificadorIdade.Api.csproj
dotnet run --project .\backend\VerificadorIdade.Api\VerificadorIdade.Api.csproj
```

O perfil local inicia a aplicação em `http://localhost:5187`. Nesta etapa, a
rota raiz apresenta a página MVC inicial. A compilação foi validada com
sucesso (zero avisos e zero erros), e uma requisição HTTP à rota raiz
retornou `200 OK`.

#### Critérios de aceite

- Projeto MVC independente do Angular, criado e compilado como `net10.0`.
- Aplicação inicia em uma URL local conhecida.
- Pipeline MVC pronto para receber controllers e respostas JSON nas próximas
  tasks.

### Task 2 — Definir os modelos da requisição e da resposta (concluída)

- Criar um modelo de requisição com a propriedade numérica `idade`.
- Criar um modelo de resposta com a propriedade textual `mensagem`.
- Garantir que a serialização JSON use os nomes de propriedades definidos no
  contrato (`idade` e `mensagem`).

Os modelos foram criados em `backend/VerificadorIdade.Api/Models/` com a
serialização explícita dos nomes de contrato.

**Critérios de aceite**

- O back-end aceita uma requisição JSON no formato documentado.
- Uma resposta de sucesso contém `mensagem` com o texto correspondente.

### Task 3 — Implementar o endpoint de verificação (concluída)

- Criar um endpoint MVC de API com método `POST` e rota
  `/api/verificacao-idade`.
- Receber a idade a partir do corpo JSON.
- Aplicar no back-end a regra: idade maior ou igual a 18 é maioridade; idade
  menor que 18 é menoridade.
- Retornar `200 OK` com a mensagem prevista no contrato.

O endpoint foi implementado em `backend/VerificadorIdade.Api/Controllers/VerificacaoIdadeController.cs` e usa os modelos de requisição e resposta definidos na Task 2.

**Critérios de aceite**

- Para idade `18` ou superior, a resposta é **Você é maior de idade.**
- Para idade abaixo de `18`, a resposta é **Você é menor de idade.**
- A regra não é duplicada como regra de negócio no Angular.

### Task 4 — Validar os dados de entrada (concluída)

- Rejeitar idade ausente, negativa ou que não seja um número inteiro.
- Retornar `400 Bad Request` para entradas inválidas, sem executar a regra de
  maioridade.
- Usar uma resposta de erro consistente do ASP.NET Core, preferencialmente
  `ProblemDetails`, sem alterar o formato das respostas de sucesso.

A validação foi implementada com `int?` + `Required` + `Range` no modelo de
requisição e com checagens em `ModelState`/`ValidationProblem` no controller. As
respostas de sucesso continuam no mesmo formato do contrato da API.

**Critérios de aceite**

- Idades inteiras não negativas são processadas.
- Entradas ausentes ou inválidas recebem status `400 Bad Request`.

### Task 5 — Configurar CORS para desenvolvimento (concluída)

- Permitir a origem local usada pelo Angular (por exemplo,
  `http://localhost:4200`).
- Aplicar a política CORS ao endpoint da API.
- Evitar liberar origens indiscriminadamente; a origem permitida deverá ser
  configurável para outros ambientes.

Foi criada a política `AngularOrigin`, aplicada ao controller da API. Em
desenvolvimento, `http://localhost:4200` é a origem permitida, definida em
`appsettings.Development.json`. A lista base em `appsettings.json` fica vazia;
outros ambientes devem configurar `Cors:AllowedOrigins` com as origens
autorizadas, por exemplo pela variável de ambiente
`Cors__AllowedOrigins__0`.

**Critérios de aceite**

- O navegador permite que o Angular local chame o endpoint.
- Uma origem não configurada não recebe permissão CORS.

### Task 6 — Testar o contrato e as regras (concluída)

- Criar testes para os casos de idade `17`, `18` e uma idade acima de `18`.
- Criar testes para requisição ausente ou inválida e verificar o status
  `400`.
- Verificar método, rota, status HTTP e formato JSON das respostas.

Foi criado o projeto xUnit
`backend/VerificadorIdade.Api.Tests/`, com testes de integração usando o
`WebApplicationFactory`. Os testes cobrem as idades `17`, `18` e `21`, corpo
ausente, idade ausente, negativa, não inteira e de tipo inválido, além de
verificar o contrato JSON, a rota e o método HTTP. Para executar:

```powershell
dotnet test .\backend\VerificadorIdade.Api.Tests\VerificadorIdade.Api.Tests.csproj
```

**Critérios de aceite**

- Os testes cobrem os dois resultados da regra de negócio e as entradas
  inválidas.
- As respostas de sucesso correspondem ao contrato documentado neste README.

### Task 7 — Documentar como executar e testar o back-end (concluída)

- Documentar pré-requisitos, comando para iniciar o back-end e URL local.
- Documentar exemplos de requisição e como conferir as respostas da API.
- Registrar como executar os testes automatizados.

#### Pré-requisitos

- .NET 10 SDK. O projeto foi compilado e validado com a versão `10.0.402`.
- PowerShell para os comandos abaixo, executados na raiz do repositório.

Confira a versão instalada:

```powershell
dotnet --version
```

#### Iniciar o back-end

Restaure as dependências, compile e inicie a API:

```powershell
dotnet restore .\backend\VerificadorIdade.Api\VerificadorIdade.Api.csproj
dotnet build .\backend\VerificadorIdade.Api\VerificadorIdade.Api.csproj
dotnet run --project .\backend\VerificadorIdade.Api\VerificadorIdade.Api.csproj
```

O perfil `http` do projeto inicia a API em `http://localhost:5187`. Mantenha
esse terminal aberto enquanto fizer chamadas. A configuração de desenvolvimento
permite CORS para `http://localhost:4200`. Para outro ambiente, configure
`Cors__AllowedOrigins__0` com a origem autorizada antes de iniciar a aplicação.

#### Enviar requisições para a API

Com o back-end em execução, envie uma idade pelo endpoint
`POST http://localhost:5187/api/verificacao-idade`:

```powershell
$uri = "http://localhost:5187/api/verificacao-idade"
$body = @{ idade = 18 } | ConvertTo-Json
Invoke-RestMethod -Method Post -Uri $uri -ContentType "application/json" -Body $body
```

Para idade `18`, a resposta HTTP é `200 OK` e o corpo é:

```json
{
  "mensagem": "Você é maior de idade."
}
```

Para conferir o caso abaixo de 18, altere o valor de `idade` para `17`; a
mensagem será `"Você é menor de idade."`. Idades ausentes ou inválidas, como
`-1`, recebem `400 Bad Request` com uma resposta `ProblemDetails`.

#### Executar os testes automatizados

Os testes de integração usam `WebApplicationFactory` e iniciam a API em memória,
portanto não é necessário deixar o servidor local em execução:

```powershell
dotnet test .\backend\VerificadorIdade.Api.Tests\VerificadorIdade.Api.Tests.csproj
```

**Critérios de aceite**

- Pré-requisitos, inicialização, URL, chamadas de exemplo e execução dos testes
  estão documentados neste README.
