using MessagingService;
using MessagingService.Data;
using MessagingService.Entities.Enums;
using MessagingService.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfiguredSerilog(builder.Configuration);

var dbOperator = new DbOperator<MessagingDbContext>();
dbOperator.AddDbContextWithSnakeNamingConvention(builder.Services, builder.Configuration,
    o => o.UseNpgsql(optionsBuilder => optionsBuilder.MapEnum<ActivityState>("activity_state")));

builder.Services.AddMessagingServices(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    await dbOperator.ApplyMigrations(app);
}

app.UseHttpsRedirection();

app.MapHub<ChatHub>("chat-hub");

app.Run();