# Para construir o projeto, foi utilizando o comando com .NET CLI, disponível a partir da instalação do SDK do .NET 8.0:
### O comando a seguuir usa -controllers para adicionar arquitetura usando controllers. Se retirar, gera arquitetura minimal.
### O comando -o seguido do nome do arquivo resulta na criação de nova pasta
```
dotnet new webapi -controllers -o ApiCatalogo
```

# Instalações realizadas para utilizar o entity framework e fazer migrations:
```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design  
dotnet tool install --global dotnet-ef                 
```

# Migrations

### Migrations - o recurso EF Core Migrations oferece uma maneira de atualizar de forma incremental o esquema do banco de dados para mantê-lo em sincronia com o modelo de dados do aplicativo, preservando os dados existentes no banco de dados. 
### Sempre que alterar as classes do modelo domínio, deve-se executar o Migrations para manter o esquema do banco de dados atualizado. 
### Modelo de entidades do domínio (cria)-> Modelo do EF Core (Migrations)-> Banco de dados

### Cria o script de Migration:
```
dotnet ef migrations add 'nome'
```

### Remove o script de Migration:
```
dotnet ef migrations remove 'nome'
```

### Gera o DB e as tabelas com base no script:
```
dotnet ef database update
```

# Data Anottations e FluentValidation API

### Aplicaremos Data Anottations para sobrescrever as convenções do EF Core 
### Os Data Annotations possíveis são: Key, Table("nome"), Column, DataType, Foreign Key, NotMapped, StringLength, Required. Ainda pode-se adicionar uma ErrorMessage.
### A desvantagem de utilizar o Data Annotation é que ele polui o código da classe. Fluent Validation API nesse caso pode ser utilizado

# Para popular as databases, usaremos migrations vazias(Ou seja, cria-se migrações sem alterar as classes do domínio) com códigos SQL de "insert into" nas voids Up e Down

# Controllers

### Controllers podem ser criados usando o template padrão do projeto, são classes que derivam da classe ControllerBase, e o nome é formado pelo nome do controlador seguido do sufixo Controller. Em uma minimal API, a estrutura é diferente.  Utilizam métodos herdados da classe ControllerBase para retornar as responses. Possuem uma estrutura baseada em decorators.

### O decorator [ApiController] permite, por exemplo, respostas http 400 automáticas, requisito de roteamento de atributo, inferência de parâmetro de origem, inferência de solicitação de dados de várias partes, e uso de Problem Details para códigos de status de erro

### Em Program.cs, adicionar o AddControllers() aos serviços, e condigurar app.MapControllerrs(). O controlador acessa o EF Core(AppDbContext) que acessa o DB. Usaremos injeção de dependencia no controlador. Por isso adicionamos nos serviços.

### Para criar um controlador, no Visual Studio, criar "API Controller with actions using Entity Framework"

```
dotnet run
```

### Se tiver referências cíclicas na aplicação, configurar para o serializer Json ignorar estas referências no Program.cs.
### Se quiser ignorar propriedades individuais na hora de fazer put e post, configurar usando decorator no model ou no Program.cs. Esse tipo de problema ocorre devido a serialização/desserialização da resposta SQL do banco.
### Quando consultamos entidades usando o EF, ele armazena as entidades no contexto para realizar o tracking das entidades para acompanhar o estado. Este recurso adiciona uma sobrecarga que afeta o desempenho das consultas rastreadas. Para resolver, pode-se adicionar o método AsNoTracking nas consultas de somente leitura. Utilizar quando o resultado da consulta não precisar ser alterado. 

### Além disso, nunca retornar todos registros de um conulta -> adicionar método Take(10) por exemplo
### Também nunca retornar objetos relacionados sem aplicar filtro. 

### Para lidar com o tratamento de erros em ambiente de produção, podemos configurar uma página de tratamento para estes erros personalizada para o ambiente de produção, usando  middleware UseExcpetionHandler. Captura e registra requisições não tratadas. Além disso, usamos Try Catch e a lib StatusCode.

## Roteamento -> importante para clareza dos endpoints e não haver ambiguidade.
### Pode-se configurar parâmetros, roteamento, e restrição de rotas(isso não deve ser a validação do parametro, mas deve ser utilizado apenas para diferenciar a rota)

### Sobre tipos de retorno, ActionResult implementa a interface abstrata IActionResult. O segundo é vantajoso de se utilizar com ActionResult<T> de forma que se pode retornar também um tipo T, além da Action (Com IAction, teria-se que criar New ObjectResult(T)). No entanto, pode-se utilizar como preferencia interface nos demais casos.

## Métodos Actions Síncronos: Quando uma request chega, uma thread do poll da aplicação é designada para processá-la e ficará bloqueada até o fim do request.
## Métodos Actions Assíncrons: A thread é encarregada de processar a requisição, mas é devolvida ao pool enquanto a operação é feira. Quando ela acaba, a thread é avisada e retoma o controle. 
### Para fazer uso disso, usa-se async, await e o tipo Task como tipo para a action. Regras:
### 1. A assinatura do método deve incluir o modificador async
### 2. O método deve ter um tipo de retorno Task<TResult>, Task ou void
### 3. As declarações devem incluir pelo menos uma expressão await - diz ao compilador que o método precisa ser suspenso enquanto a operação estiver ocupada
### 4. O nome do método deve ter o sufico Async por convenção

## O assincronismo melhora a experiência do usuário e o ganho é atender mais requisições em paralelo. Podem, há perda de desempenho e a requisição específica não ficará mais rápida em hipótese alguma. Por exemplo, é interessante utilizar nos métodos Get que estão acessando o DB.





# Para executar a API, usamos:

```
dotnet run
```

