<img src="https://github.com/geniusxp/backend-dotnet/blob/main/backend-.net/banner_geniusxp.png">

# Sobre a AMPZ

A Ampz é uma plataforma que combina IoT e educação infantil para promover hábitos sustentáveis. Utilizando dispositivos inteligentes nos quartos das crianças, monitora o consumo de energia em tempo real, incentivando o aprendizado por meio de desafios interativos e recompensas. Além de ensinar a importância da economia de energia, a Ampz conecta usuários e comunidades em uma rede engajada pela sustentabilidade.

# Design Pattern Aplicado

Na implementação da API do projeto AMPZ, decidimos utilizar o padrão de criação Builder. O padrão Builder é eficaz quando se deseja fornecer uma maneira flexível e controlada de construir objetos complexos, separando a criação de um objeto da sua representação final.

O uso do Builder traz benefícios claros:

- **Facilita a manutenção:** O código se torna mais modular e organizado, permitindo a criação de instâncias de classes de forma fluida e flexível.
- **Controle sobre a construção de objetos complexos:** Com o Builder, é possível gerenciar a criação de objetos que possuem diversos atributos opcionais ou diferentes configurações sem comprometer a clareza do código.
- **Encapsulamento de lógica complexa:** Ele permite que a complexidade da construção do objeto seja ocultada dentro do builder, deixando o código principal mais limpo e de fácil entendimento.

Assim, o padrão Builder foi implementado para otimizar a construção das classes da API, promovendo maior flexibilidade e manutenção eficiente.

# Princípios SOLID e Clean Code Aplicados

## 1. Single Responsibility Principle (SRP)
O SRP estabelece que uma classe deve ter uma única responsabilidade, ou seja, ela deve ter um único motivo para mudar. Isso torna o código mais fácil de manter e de entender.

As controllers do projeto estão focadas exclusivamente em gerenciar operações relacionadas aos seus respectivos domínios, como Usuário, Criança, Dispositivo e Comunidade. Isso significa que, se alguma mudança for necessária nas regras de negócio relacionadas a dispositivos, por exemplo, a modificação se limitará à classe DeviceController ou a outras diretamente relacionadas a essa responsabilidade. Isso está de acordo com o SRP, pois cada classe tem uma função bem definida e não está sobrecarregada com outras responsabilidades que não pertencem ao seu domínio, como lógica de previsão de consumo ou gestão de comunidades.


## 2. Dependency Injection (DI)
   
Dependency Injection é um padrão de design que permite que as dependências sejam fornecidas a uma classe, em vez de ela mesma instanciar essas dependências. Isso promove a inversão de controle e facilita a substituição e o teste de componentes.

Exemplo:
```csharp
    private readonly AppDbContext _context;

    public EnergyConsumptionController(AppDbContext context)
    {
        _context = context;

```
Como AppDbContext é injetado, a classe EnergyConsumptionController pode ser facilmente testada usando um mock ou uma instância alternativa de AppDbContext, sem a necessidade de uma instância concreta do banco de dados em si. Isso facilita a criação de testes unitários e de integração.

Se no futuro for necessário mudar a forma como os dados são armazenados ou introduzir um repositório, basta alterar a injeção de dependência. Isso torna a aplicação mais modular e mais fácil de modificar sem grandes alterações no código.

O controlador não está acoplado a uma instância específica de AppDbContext, o que significa que ele não se preocupa com a criação ou gerenciamento do ciclo de vida dessa dependência.

## 3. Clean Code
Os métodos e variáveis possuem nomes intuitivos, como AddKidToCommunity e GetDevicesByKidId, que descrevem claramente a intenção e o propósito do código. Isso facilita a compreensão, especialmente para outros desenvolvedores que possam trabalhar no código posteriormente.

Os métodos utilizam return NotFound(), return NoContent(), return BadRequest(), entre outros, que são boas práticas para indicar corretamente o status da operação HTTP. Isso melhora a comunicação com os consumidores da API, permitindo que entendam facilmente se a operação foi bem-sucedida ou se o recurso solicitado não foi encontrado.

# Testes Automatizados Desenvolvidos
Para garantir a qualidade do código desenvolvido, foram construídos os seguintes testes:

## UserController
A controller de usuários foi testada das seguintes maneiras:

### 1.1 CreateUser_ShouldReturnCreatedUser
Verifica se o método CreateUser cria um novo usuário corretamente e retorna o usuário criado.

### 1.2 GetAllUsers_ShouldReturnListOfUsers
Verifica se o método GetAllUsers retorna uma lista de usuários cadastrados.

### 1.3 UpdateUser_ShouldReturnNoContent
Verifica se o método UpdateUser atualiza corretamente os dados de um usuário existente.

### 1.4 DeleteUser_ShouldReturnNoContent
Verifica se o método DeleteUser remove um usuário existente corretamente.

## DeviceController
A controller de dispositivos foi testada das seguintes maneiras:

### 2.1 GetDevicesByKidId_ShouldReturnDevices
Verifica se o método GetDevicesByKidId retorna os dispositivos associados a uma criança específica.

### 2.2 GetDevicesByKidId_ShouldReturnNotFoundForInvalidKid
Verifica se o método GetDevicesByKidId retorna NotFound quando uma criança inexistente é informada.

### 2.3 UpdateDevice_ShouldReturnNoContent
Verifica se o método UpdateDevice atualiza os dados de um dispositivo existente corretamente.

### 2.4 UpdateDevice_ShouldReturnNotFoundForInvalidDevice
Verifica se o método UpdateDevice retorna NotFound ao tentar atualizar um dispositivo inexistente.

### 2.5 DeleteDevice_ShouldReturnNoContent
Verifica se o método DeleteDevice remove um dispositivo existente corretamente.

### 2.6 DeleteDevice_ShouldReturnNotFoundForInvalidDevice
Verifica se o método DeleteDevice retorna NotFound ao tentar excluir um dispositivo inexistente.

## KidController
A controller de crianças foi testada das seguintes maneiras:

### 3.1 GetKidsByUserId_ShouldReturnKids
Verifica se o método GetKidsByUserId retorna as crianças associadas a um usuário específico.

### 3.2 GetKidsByUserId_ShouldReturnNotFoundForInvalidUser
Verifica se o método GetKidsByUserId retorna NotFound quando um usuário inexistente é informado.

### 3.3 UpdateKid_ShouldReturnNoContent
Verifica se o método UpdateKid atualiza os dados de uma criança existente corretamente.

### 3.4 UpdateKid_ShouldReturnNotFoundForInvalidKid
Verifica se o método UpdateKid retorna NotFound ao tentar atualizar uma criança inexistente.

### 3.5 DeleteKid_ShouldReturnNoContent
Verifica se o método DeleteKid remove uma criança existente corretamente.

### 3.6 DeleteKid_ShouldReturnNotFoundForInvalidKid
Verifica se o método DeleteKid retorna NotFound ao tentar excluir uma criança inexistente.

## CommunityController
A controller de comunidades foi testada das seguintes maneiras:

### 4.1 CreateCommunity_ShouldReturnCreatedCommunity
Verifica se o método CreateCommunity cria uma comunidade corretamente e retorna um OkObjectResult com os detalhes da comunidade criada.

### 4.2 GetAllCommunities_ShouldReturnListOfCommunities
Verifica se o método GetAllCommunities retorna uma lista de comunidades cadastradas com um status Ok.

### 4.3 AddKidToCommunity_ShouldAddKidAndReturnOk
Verifica se o método AddKidToCommunity adiciona corretamente uma criança a uma comunidade existente e retorna um OkObjectResult.

### 4.4 AddKidToCommunity_ShouldReturnNotFoundForInvalidCommunity
Verifica se o método AddKidToCommunity retorna NotFound ao tentar adicionar uma criança a uma comunidade inexistente.

### 4.5 AddKidToCommunity_ShouldReturnNotFoundForInvalidKid
Verifica se o método AddKidToCommunity retorna NotFound ao tentar adicionar uma criança inexistente a uma comunidade.

# Aplicação de Inteligência Artificial

A EnergyConsumptionController do Ampz utiliza inteligência artificial com ML.NET para estimar a economia de energia com base no uso de dispositivos. A solução prevê o impacto energético a partir das horas de uso fornecidas pelo usuário, retornando mensagens motivacionais e educativas. 

Com dados históricos, o modelo é treinado automaticamente, utilizando o algoritmo FastTree para calcular a economia e incentivar práticas sustentáveis. Além disso, o sistema recompensa crianças com pontos (TotalScore) e atualiza o total de energia economizada (TotalEnergySaved), tornando o aprendizado divertido e interativo.

# Instruções para executar o projeto (Local)

Para executar o projeto localmente, é necessário possuir alguns programas e ferramentas instaladas e seguir esses passos:

## Pré-requisitos
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) com o workload de desenvolvimento para ASP.NET Core
- [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet) (versão compatível com o projeto (.NET 8.0))
- Git instalado ([Baixar Git](https://git-scm.com/))
- Banco de Dados Oracle

## Passo 1: Clonar o Repositório
Abra o terminal ou o Git Bash e execute o seguinte comando para clonar o repositório do projeto:

```bash
git clone https:
```

## Passo 2: Navegar até a Pasta do Projeto
Depois de clonar o repositório, navegue até a pasta raiz do projeto:

```bash
cd backend-dotnet
```

## Passo 3: Restaurar as Dependências
Restaure todas as dependências necessárias para o projeto executando o seguinte comando:

```bash
dotnet restore
```
## Passo 4: Configurar String de Conexão
Edite o arquivo appsettings.json e configure credenciais corretas para acessar seu banco de dados Oracle:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=oracle.fiap.com.br:1521/orcl; User Id={Seu usuário}; Password={Sua senha}"
}
```

## Passo 5: Executar as migrations do projeto
Aplique as migrations executando o seguinte comando para criar as tabelas em seu banco de dados:

```bash
dotnet ef database update
```

## Passo 6: Executar o projeto
Para rodar o projeto, utilize o seguinte comando:

```bash
dotnet run
```
Isso irá iniciar a API localmente. A saída no terminal indicará o endereço no qual a API está sendo executada, geralmente http://localhost:5130

## Passo 7: Acessando documentação e testando o projeto com Swagger
No navegador, acesse o seguinte endereço para visualizar a documentação da API gerada pelo Swagger:

```bash
https://localhost:5130/swagger/index.html
```
Através da interface do Swagger, você pode testar os endpoints diretamente, enviando requisições e recebendo as respostas em tempo real. Isso facilita a verificação da funcionalidade da API sem a necessidade de ferramentas externas.

# Instruções para executar o projeto (Nuvem)
Para executar e testar a API do Ampz, siga os passos abaixo:

1. Acesse o seguinte endereço para iniciar o projeto:
```
http://ampzbackenddotnet.brazilsouth.azurecontainer.io:8080
```

2. Para visualizar a documentação e testar os endpoints da API, acesse o Swagger no seguinte link:
```
http://ampzbackenddotnet.brazilsouth.azurecontainer.io:8080/swagger/index.html
```
O Swagger fornece uma interface interativa para explorar os recursos da API diretamente no navegador.

# Endpoints da API
Abaixo estão os principais endpoints disponíveis na API AMPZ, acessíveis via o Swagger:

### Comunidade
- [POST] /api/community
Corpo da requisição:
```json
{
  "name": "string",
  "description": "string"
}
```
- [GET] /api/community
- [POST] /api/community/{communityId}/add-kid/{kidId}

### Dispositivo
- [GET] /api/device/kid/{kidId}
- [PUT] /api/device/{id}
Corpo da requisição:
```json
{
  "name": "string",
  "type": "string",
  "operatingSystem": "string"
}
```
- [DELETE] /api/device/{id}
  
### Criança
- [GET] /api/kid/user/{userId}
- [PUT] /api/kid/{id}
Corpo da requisição:
```json
{
  "name": "string",
  "birthdate": "2024-01-01",
  "totalScore": 0,
  "totalEnergySaved": 0.0
}
```
- [DELETE] /api/kid/{id}

### Usuário
- [POST] /api/user
Corpo da requisição:
```json
{
  "name": "string",
  "email": "string",
  "password": "string",
  "birthdate": "2024-01-01",
  "city": "string",
  "state": "string",
  "kids": [
    {
      "name": "string",
      "birthdate": "2024-01-01",
      "devices": [
        {
          "name": "string",
          "type": "string",
          "operatingSystem": "string"
        }
      ]
    }
  ]
}
```
- [GET] /api/user
- [GET] /api/user/{id}
- [PUT] /api/user/{id}
Corpo da requisição:
```json
{
  "name": "string",
  "email": "string",
  "password": "string",
  "birthdate": "2024-01-01",
  "city": "string",
  "state": "string"
}
```
- [DELETE] /api/user/{id}
- 
### Consumo de Energia
- [POST] /api/energyconsumption/predict
Corpo da requisição:
```json
{
  "deviceId": 0,
  "hoursUsed": 0.0
}
```

# Vídeo Demonstração
Para ver o vídeo de demonstração da aplicação, acesse: 

# Equipe Ampz
- RM99565 - Erick Nathan Capito Pereira
- RM550841 - Lucas Araujo Oliveira Silva
- RM99409 - Michael José Bernardi Da Silva
- RM99577 - Guilherme Dias Gomes
- RM550889 - Hemily Nara da Silva
