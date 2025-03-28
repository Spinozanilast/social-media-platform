using Authentication.Configuration;
using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfiguredSerilog(builder.Configuration);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddJwtConfiguration();

builder.Services.AddCors(options =>
{
    options.AddPolicy("NextFrontend", b =>
    {
        b
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins(builder.Configuration["ClientApp"]!);
    });
});

var app = builder.Build();
app.UseCors("NextFrontend");
app.MapReverseProxy();
app.UseAuthentication();
app.UseAuthorization();
app.Run();