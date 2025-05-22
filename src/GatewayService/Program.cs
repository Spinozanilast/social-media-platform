using Authentication.Configuration;
using Authentication.Configuration.Options;
using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfiguredSerilog(builder.Configuration);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.AddJwtAuthentication();

builder.Services.AddCorsPolicy("NextFrontend", builder.Configuration["ClientApp"]);

var app = builder.Build();
app.UseCors("NextFrontend");
app.MapReverseProxy();
app.UseAuthentication();
app.UseAuthorization();
app.Run();