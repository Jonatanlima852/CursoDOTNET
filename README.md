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

### Aplicaremos Data Anottations para sobrescrever as convenções do EF Core 
### Os Data Annotations possíveis são: Key, Table("nome"), Column, DataType, Foreign Key, NotMapped, StringLength, Required. Ainda pode-se adicionar uma ErrorMessage.