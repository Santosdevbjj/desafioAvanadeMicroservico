using Microsoft.EntityFrameworkCore;
using VendasService.Services;
using VendasService.Messaging;
using VendasService.Data;

var builder = WebApplication.CreateBuilder(args);

// Banco MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlConnection"),
        new MySqlServerVersion(new Version(8, 0, 29))
    )
);

// Injeção de dependências
builder.Services.AddSingleton<RabbitMqPublisher>();
builder.Services.AddScoped<PedidoService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

