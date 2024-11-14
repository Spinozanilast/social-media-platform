using MassTransit;
using ProfileService.Common.Repositories;
using ProfileService.Common.Services;
using ProfileService.Consumers;
using ProfileService.Data;
using ProfileService.Repositories;
using ProfileService.Services;
using Serilog;
using Shared.Infrastructure;
using Shared.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfiguredSerilog(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddS3Client();
builder.Services.AddScoped<IProfileImageService, ProfileImageService>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.Configure<ProfileImageStorageConfig>(builder.Configuration.GetSection("ProfileImageStorage"));

var rabbitMqConfig = new RabbitMqConfiguration();
builder.Configuration.GetSection("RabbitMq").Bind(rabbitMqConfig);
builder.Services.AddMassTransitConfigured(rabbitMqConfig,
    bus =>
    {
        bus.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("profile", false));
        bus.AddConsumer<UserRegisteredConsumer>();
    },
    (context, configurator) =>
    {
        configurator.ReceiveEndpoint("profile-user-registered", e =>
        {
            e.UseMessageRetry(r => r.Interval(5, 50));
            e.ConfigureConsumer<UserRegisteredConsumer>(context);
        });
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();