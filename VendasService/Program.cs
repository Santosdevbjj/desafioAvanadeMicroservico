using Microsoft.EntityFrameworkCore;
using VendasService.Data;
using VendasService.Repositories;
using VendasService.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "server=mysql;database=vendasdb;user=root;password=secret";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Injeção de dependência
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddSingleton<RabbitMqPublisher>();

// Controllers + Swagger
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
