using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfiguredSerilog(builder.Configuration);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("customPolicy", b =>
    {
        b.AllowAnyHeader()
            .AllowAnyMethod().AllowCredentials().WithOrigins(builder.Configuration["ClientApp"]!);
    });
});

var app = builder.Build();
app.UseCors();
app.MapReverseProxy();
app.Run();