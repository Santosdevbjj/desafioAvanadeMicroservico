using Microsoft.EntityFrameworkCore;
using EstoqueService.Data;
using EstoqueService.Repositories;
using EstoqueService.Services;
using EstoqueService.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Configura MySQL
builder.Services.AddDbContext<EstoqueContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySQL"),
    new MySqlServerVersion(new Version(8, 0, 32))));

// Injeta dependências
builder.Services.AddScoped<ProdutoRepository>();
builder.Services.AddScoped<ProdutoService>();

// Adiciona Consumer RabbitMQ
builder.Services.AddHostedService<RabbitMqConsumer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
