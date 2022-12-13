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

app.Run(); //Iniciando aplicação
