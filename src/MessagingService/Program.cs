using MessagingService.Common.Services;
using MessagingService.Data;
using MessagingService.Entities.Enums;
using MessagingService.Services;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfiguredSerilog(builder.Configuration);

var dbOperator = new DbOperator<MessagingDbContext>();
dbOperator.AddDbContextWithSnakeNamingConvention(builder.Services, builder.Configuration,
    o => o.UseNpgsql(optionsBuilder => optionsBuilder.MapEnum<ActivityState>("activity_state")));

builder.Services.AddScoped<IChatService, ChatsService>();
builder.Services.AddSignalR();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    dbOperator.ApplyMigrations(app);
}

app.UseHttpsRedirection();

app.MapHub<ChatHub>("chat-hub");

app.Run();