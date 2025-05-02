using Asp.Versioning;
using Authentication.Configuration;
using Authentication.Configuration.Options;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Common.Repositories;
using ProfileService.Common.Services;
using ProfileService.Consumers;
using ProfileService.Data;
using ProfileService.Endpoints;
using ProfileService.Entities;
using ProfileService.Repositories;
using ProfileService.Services;
using Shared.Infrastructure;
using Shared.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfiguredSerilog(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbOperator = new DbOperator<ProfilesDbContext>();
dbOperator.AddDbContextWithSnakeNamingConvention(builder.Services, builder.Configuration);

builder.Services.AddS3Client(builder.Configuration);
builder.Services.AddScoped<IProfileImageService, ProfileImageService>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.Configure<ProfileImageStorageConfig>(builder.Configuration.GetSection("ProfileImageStorage"));

var rabbitMqConfig = new RabbitMqConfiguration();
builder.Configuration.GetSection(RabbitMqConfiguration.SectionName).Bind(rabbitMqConfig);
builder.Services.AddMassTransitConfigured(rabbitMqConfig,
    bus =>
    {
        bus.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("profile", false));
        bus.AddConsumer<UserRegisteredConsumer>();
    }
);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.AddJwtAuthentication();

builder.Services.AddConfiguredApiVersioning();

var app = builder.Build();

var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .ReportApiVersions()
    .Build();

app.MapGroup("api/v{version:apiVersion}/profiles")
    .MapProfilesEndpoints()
    .WithApiVersionSet(apiVersionSet)
    .HasApiVersion(1, 0);

app.MapGroup("api/v{version:apiVersion}/profiles/{userId:guid}/image")
    .MapProfilesImagesEndpoints()
    .WithApiVersionSet(apiVersionSet)
    .HasApiVersion(1, 0);

app.MapGet("api/v{version:apiVersion}/countries",
        async Task<Ok<List<Country>>> ([FromServices] ICountriesRepository countriesRepository) =>
        {
            var countries = await countriesRepository.GetAll();
            return TypedResults.Ok(countries);
        })
    .AllowAnonymous()
    .WithName("GetCountries")
    .WithOpenApi();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    dbOperator.ApplyMigrations(app);
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.Run();