using EstoqueService.Data;
using EstoqueService.Repositories;
using EstoqueService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<EstoqueContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// Add Repository
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

// Add RabbitMQ Consumer
builder.Services.AddHostedService<RabbitMqConsumer>();

// Add Controllers
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
