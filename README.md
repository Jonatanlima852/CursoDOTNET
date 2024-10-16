# Para construir o projeto, foi utilizando o comando com .NET CLI, disponível a partir da instalação do SDK do .NET 8.0:
O comando a seguuir usa -controllers para adicionar arquitetura usando controllers. Se retirar, gera arquitetura minimal.
O comando -o seguido do nome do arquivo resulta na criação de nova pasta
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

Migrations - o recurso EF Core Migrations oferece uma maneira de atualizar de forma incremental o esquema do banco de dados para mantê-lo em sincronia com o modelo de dados do aplicativo, preservando os dados existentes no banco de dados. 
Sempre que alterar as classes do modelo domínio, deve-se executar o Migrations para manter o esquema do banco de dados atualizado. 
Modelo de entidades do domínio (cria)-> Modelo do EF Core (Migrations)-> Banco de dados

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

Aplicaremos Data Anottations para sobrescrever as convenções do EF Core Os Data Annotations possíveis são: [Key], [Table("nome")], [Column], [DataType], [ForeignKey], [NotMapped], [MaxLength], [Required]. Ainda pode-se adicionar uma ErrorMessage. 

Data Anottations também podem ser utilizados na validação, invés de somente para sobrescrever as convenções do EF Core ao fazer mapeamento.

Validações interessante são: 

1. [RegularExpression("regex", ErrorMessage="erro")]
2. [Range(18, 65, ErrorMessage="idade deve estar entre 18 e 65")]
3. [CreditCard], [Url], [Phone], [Compare("Senha")] (este permite comprar duas propriedades)
4. [StringLength(10, ErrorMessage="erro", MinimumLength = 5)]

O atributo [ApiController] faz com que as validações sejam feitas automaticamente, verificando se o ModelState é válido ou não. A validação ocorre após o Model Binding. A validação manual utilizaria TryValidate(model). 

Pode-se criar atributos personalizados para fazer a validação ou impelementar IValidatableObject no seu modelo para acessar todas propriedades e fazer uma validação mais complexa. (Ver pasta Validation)

Vantagem: Simplicidade e centralização da validação de modelo

A desvantagem de utilizar o Data Annotation é que ele polui o código da classe. Fluent Validation API nesse caso pode ser utilizado

# População de DBs

Para popular as databases, usaremos migrations vazias(Ou seja, cria-se migrações sem alterar as classes do domínio) com códigos SQL de "insert into" nas voids Up e Down

# Controllers

Controllers podem ser criados usando o template padrão do projeto, são classes que derivam da classe ControllerBase, e o nome é formado pelo nome do controlador seguido do sufixo Controller. Em uma minimal API, a estrutura é diferente.  Utilizam métodos herdados da classe ControllerBase para retornar as responses. Possuem uma estrutura baseada em decorators.

O decorator [ApiController] permite, por exemplo, respostas http 400 automáticas, requisito de roteamento de atributo, inferência de parâmetro de origem, inferência de solicitação de dados de várias partes, e uso de Problem Details para códigos de status de erro

Em Program.cs, adicionar o AddControllers() aos serviços, e condigurar app.MapControllerrs(). O controlador acessa o EF Core(AppDbContext) que acessa o DB. Usaremos injeção de dependencia no controlador. Por isso adicionamos nos serviços.

Para criar um controlador, no Visual Studio, criar "API Controller with actions using Entity Framework"

```
dotnet run
```

# Otimizações e Ajustes
Se tiver referências cíclicas na aplicação, configurar para o serializer Json ignorar estas referências no Program.cs.

Se quiser ignorar propriedades individuais na hora de fazer put e post, configurar usando decorator no model ou no Program.cs. Esse tipo de problema ocorre devido a serialização/desserialização da resposta SQL do banco.

Quando consultamos entidades usando o EF, ele armazena as entidades no contexto para realizar o tracking das entidades para acompanhar o estado. Este recurso adiciona uma sobrecarga que afeta o desempenho das consultas rastreadas. Para resolver, pode-se adicionar o método AsNoTracking nas consultas de somente leitura. Utilizar quando o resultado da consulta não precisar ser alterado. 

Além disso, nunca retornar todos registros de um conulta -> adicionar método Take(10) por exemplo

Também nunca retornar objetos relacionados sem aplicar filtro. 

Para lidar com o tratamento de erros em ambiente de produção, podemos configurar uma página de tratamento para estes erros personalizada para o ambiente de produção, usando  middleware UseExcpetionHandler. Captura e registra requisições não tratadas. Além disso, usamos Try Catch e a lib StatusCode.

# Roteamento 

Roteamento é importante para clareza dos endpoints e não haver ambiguidade.

Pode-se configurar parâmetros, roteamento, e restrição de rotas(isso não deve ser a validação do parametro, mas deve ser utilizado apenas para diferenciar a rota)

Sobre tipos de retornonnas funções do controller, ActionResult implementa a interface abstrata IActionResult. O segundo é vantajoso de se utilizar com ActionResult<T> de forma que se pode retornar também um tipo T, além da Action (Com IAction, teria-se que criar New ObjectResult(T)). No entanto, pode-se utilizar como preferencia interface nos demais casos.

# Métodos Assíncronos

Métodos Actions Síncronos: Quando uma request chega, uma thread do poll da aplicação é designada para processá-la e ficará bloqueada até o fim do request.

Métodos Actions Assíncrons: A thread é encarregada de processar a requisição, mas é devolvida ao pool enquanto a operação é feira. Quando ela acaba, a thread é avisada e retoma o controle. 

Para fazer uso disso, usa-se async, await e o tipo Task como tipo para a action. Regras:
1. A assinatura do método deve incluir o modificador async
2. O método deve ter um tipo de retorno Task<TResult>, Task ou void
3. As declarações devem incluir pelo menos uma expressão await - diz ao compilador que o método precisa ser suspenso enquanto a operação estiver ocupada
4. O nome do método deve ter o sufico Async por convenção

### O assincronismo melhora a experiência do usuário e o ganho é atender mais requisições em paralelo. Podem, há perda de desempenho e a requisição específica não ficará mais rápida em hipótese alguma. Por exemplo, é interessante utilizar nos métodos Get que estão acessando o DB.


# Model Binding

Determina qual o método Actions será executado com base na definição da rota e vincula os valores da requiisção HTTP aos parâmetros deste método Action.

Extrai primeiro valores do body(POST e PUT), em seguida verifica parametros pelo roteamento, e por fim procura dados nas QueryStrings. Ex: ?nome=Suco&ativo=true (a queryString começa na interrogação).

Parâmetros para definir se o model binding ocorrerá ou não:
1. [BindRequired] -> adiciona um erro ao ModelState se a vinculação dos parâmetros não puder ocorrer. Utilizado no controller (Bom para exigir query strings)
2. [BindNever] -> Informa para não vincular informação ao parametro. Utilizado no Model

Atributos que indicam a fonte de dados dos parâmetros: FromForm - (somente dados do formulário enviado), FromRoute - (vincula apenas dados oriundos da rota de dados), FromQuery, FromHeader, FromBody, FromServices - (vicula o valor à implementação que foi configurada no seu conteiner DI) 
Atenção especial ao [FromServices]: Permite que que vc utilize um service registrado sem configurá-lo no construtor. Por padrão é feita essa inferêcia de onde utilizar services, mas pode-se desabilitá-la e fazê-la usando o atributo. É bacana quando não se quer que toda a classe tenha acesso ao service. Pode inclusive registrar o service como transiente ou persistente, etc. 
Os atributos são interessantes para segurança e personalização das rotas.










# Para executar a API, usamos:

```
dotnet run
```

