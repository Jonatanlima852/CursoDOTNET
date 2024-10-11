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

# Para executar a API, usamos:

```
dotnet run
```

