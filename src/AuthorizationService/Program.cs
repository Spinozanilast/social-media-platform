using Asp.Versioning;
using Authentication.Configuration;
using Authentication.Configuration.Options;
using AuthorizationService;
using AuthorizationService.Common.Services;
using AuthorizationService.Data;
using AuthorizationService.Entities;
using AuthorizationService.Extensions;
using AuthorizationService.Services;
using Microsoft.OpenApi.Models;
using Shared.Infrastructure;
using Shared.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfiguredSerilog(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity Api", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});

var dbOperator = new DbOperator<IdentityAppContext>();
dbOperator.AddDbContextWithSnakeNamingConvention(builder.Services, builder.Configuration);
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddTransient<ICookieManager, CookieManager>();

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
    app.UseSwagger();
    app.UseSwaggerUI();
    dbOperator.ApplyMigrations(app);
    await app.Services.SeedRoles();
}

app.UseAuthentication();
app.UseAuthorization();

app.Run();