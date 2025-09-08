using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Yarp.ReverseProxy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:5000";
        options.Audience = "api";
        options.RequireHttpsMetadata = false;
    });

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy();

app.Run();
