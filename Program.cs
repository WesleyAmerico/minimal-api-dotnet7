using Microsoft.AspNetCore.Mvc;
using Minimal_API_Desafio.DTOs;
using Minimal_API_Desafio.Models;
using Minimal_API_Desafio.ModelViews;

var builder = WebApplication.CreateBuilder(args); //iniciando a criação do servidor virtual

// Add services to the container.

builder.Services.AddControllers(); // criandop os controladores e adicionando que temos a pasta controllers
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); //criando as rotas
builder.Services.AddSwaggerGen();

var app = builder.Build(); // criando a app para lançar a VPS

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); //falando para utilizar sempre o https

app.UseAuthorization();

app.MapControllers(); //mapeando os contoles, ligando os dados com a pasta controllers

MapRoutes(app);
MapRoutesClientes(app);

#region Rotas utilizando Minimal API

void MapRoutesClientes(WebApplication app)
{
 app.MapGet("/clientes", () => 
    {
         var clientes = new List<Cliente>();
         return Results.Ok(StatusCodes.Status200OK);
    })
    .Produces<Cliente>(StatusCodes.Status200OK)
    .WithName("GetCliente")
    .WithTags("Clientes");

    app.MapPost("/clientes", ([FromBody] ClienteDTO clienteDTO) => 
    {
        var cliente = new Cliente
        {
           Nome = clienteDTO.Nome,
           Telefone =  clienteDTO.Telefone,
           Email = clienteDTO.Email,
        };
         return Results.Created($"/clientes/{cliente.Id}",cliente);
    })
    .Produces<Cliente>(StatusCodes.Status200OK)
    .Produces<Error>(StatusCodes.Status400BadRequest)
    .WithName("PostCliente")
    .WithTags("Clientes");

    app.MapPut("/cliente/{id}", ([FromRoute] int id, [FromBody] ClienteDTO clienteDTO) => 
    {
        if(string.IsNullOrEmpty(clienteDTO.Nome))
            return Results.BadRequest(new Error 
            {
                Codigo = 29012901, 
                Mensagem = "Você passou um cliente inexistente"
            });
        /*
           var cliente = ClienteService.BuscarPorId(id);
           if(cliente == null)
            return Results.NotFound(new Error {Codigo = 29012901, Mensagem = Você passou um cliente inexistente});
           Nome = clienteDTO.Nome,
           Telefone =  clienteDTO.Telefone,
           Email = clienteDTO.Email,
        */
        var cliente = new Cliente();
        
         return Results.Ok(cliente);
    })
    .Produces<Cliente>(StatusCodes.Status200OK)
    .Produces<Error>(StatusCodes.Status400BadRequest)
    .Produces<Error>(StatusCodes.Status404NotFound)
    .WithName("PutCliente")
    .WithTags("Clientes");
}

void MapRoutes(WebApplication app)
{
    // duas formas de fazer a mesma coisa a primeira utilizando arrow function
    //app.MapGet("/", () => new {Mensagem = "Bem vindo a API"});
    app.MapGet("/", () =>  {
        var retorno = new {Mensagem = "Bem vindo a API"};
        return retorno;
    })
    .Produces<dynamic>(StatusCodes.Status200OK)
    .WithName("Home")
    .WithTags("Teste");
    //fim das formas a seguir utilizarei apenas arrow function
    app.MapGet("/recebe-parametro", (string? nome) => 
    {
        //response.StatusCode = 201;
        //request.
        if(string.IsNullOrEmpty(nome))
        {
            return Results.BadRequest(new {
                Mensagem = "Olha você não mandou uma informação importante, o nome é obrigatorio"
            });
        }

        nome = $"""
        Alterando parametro recebido {nome}
        """;
        var objetoDeRetorno = new
        {
            ParametroPassado = nome,
            Mensagem = "Muito bem alunos passamos um parametro por querystring"
        };
        return Results.Created($"/recebe-parametro/{objetoDeRetorno.ParametroPassado}", objetoDeRetorno);
    })//abaixo ão os decorates para ter uma melhor documentação para quem acessar a api -- super importante
    .Produces<dynamic>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("TesteRecebeParametro")
    .WithTags("Teste");
}

#endregion

app.Run(); //Iniciando aplicação
