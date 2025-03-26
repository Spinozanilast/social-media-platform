using Authentication.Configuration;
using FluentValidation;
using Shared.Infrastructure;
using StoriesService.Common;
using StoriesService.Data;
using StoriesService.Entities;
using StoriesService.Repositories;
using StoriesService.Services;
using StoriesService.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfiguredSerilog(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddUsersDbContext(builder.Configuration);
builder.Services.AddScoped<IStoryRepository, StoryRepository>();
builder.Services.AddTransient<IValidator<Story>, StoryValidator>();
builder.Services.AddScoped<IStoryService, StoryService>();

builder.Services.AddJwtConfiguration();
builder.Services.AddAuthentication();
builder.Services.AddApiVersioning();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseCors(corsBuilder => corsBuilder.AllowAnyOrigin());
app.MapControllers();

app.Run();