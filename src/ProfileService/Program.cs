using Authentication.Configuration;
using MassTransit;
using ProfileService.Common.Repositories;
using ProfileService.Common.Services;
using ProfileService.Consumers;
using ProfileService.Data;
using ProfileService.Repositories;
using ProfileService.Services;
using Shared.Infrastructure;
using Shared.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfiguredSerilog(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProfileDbContext(builder.Configuration);
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

builder.Services.AddJwtConfiguration();
builder.Services.AddAuthentication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();