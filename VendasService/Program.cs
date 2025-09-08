using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Auth;
using VendasService.Data;
using VendasService.Repositories;
using VendasService.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados (MySQL)
builder.Services.AddDbContext<VendasContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// Repositórios e serviços
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<PedidoService>();

// JWT Token Service
builder.Services.AddScoped<JwtTokenService>();

// Configuração de autenticação JWT
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false, // sem validação de Audience nesse exemplo
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger somente em dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

// Middleware de autenticação/autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
