using Asp.Versioning;
using Authentication.Configuration;
using Authentication.Configuration.Options;
using AuthorizationService;
using AuthorizationService.Data;
using AuthorizationService.Entities;
using AuthorizationService.Extensions;
using Scalar.AspNetCore;
using Shared.Infrastructure;
using Shared.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddConfiguredSerilog(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();

var dbOperator = new DbOperator<IdentityAppContext>();
dbOperator.AddDbContextWithSnakeNamingConvention(builder.Services, builder.Configuration);

builder.Services.AddAuthorizationServices(builder.Configuration);

builder.Services
    .AddIdentity<User, Role>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
    })
    .AddEntityFrameworkStores<IdentityAppContext>();

var rabbitMqConfig = new RabbitMqConfiguration();
builder.Configuration.GetSection(RabbitMqConfiguration.SectionName).Bind(rabbitMqConfig);
builder.Services.AddMassTransitConfigured(rabbitMqConfig);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.AddJwtAuthentication().AddConfiguredGithub(builder.Configuration);

builder.Services.AddConfiguredApiVersioning();

builder.Services.AddCorsPolicy("NextFrontend", builder.Configuration["ClientApp"]);
builder.Services.AddAuthorization();

var app = builder.Build();

var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .ReportApiVersions()
    .Build();

app.MapGroup("/api/v{version:apiVersion}/auth")
    .MapAuthEndpoints()
    .WithApiVersionSet(apiVersionSet)
    .HasApiVersion(new ApiVersion(1, 0));

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    await dbOperator.ApplyMigrations(app);
    await app.Services.SeedRoles();
}

app.UseCors("NextFrontend");
app.UseRefreshTokenMiddleware();
app.UseAuthentication();
app.UseAuthorization();

app.Run();