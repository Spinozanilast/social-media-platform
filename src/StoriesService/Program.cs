using Asp.Versioning;
using Authentication.Configuration;
using Authentication.Configuration.Options;
using Scalar.AspNetCore;
using Shared.Infrastructure;
using StoriesService;
using StoriesService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfiguredSerilog(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var dbOperator = new DbOperator<StoriesDbContext>();
dbOperator.AddDbContextWithSnakeNamingConvention(builder.Services, builder.Configuration);

builder.Services.AddStoriesServices(builder.Configuration);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.AddJwtAuthentication();

builder.Services.AddAuthentication();
builder.Services.AddApiVersioning();

var app = builder.Build();

var versionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .ReportApiVersions()
    .Build();

app.MapGroup("/api/v{version:apiVersion}/stories")
    .MapStoriesEndpoints()
    .WithApiVersionSet(versionSet)
    .HasApiVersion(new ApiVersion(1, 0));

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    await dbOperator.ApplyMigrations(app);
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseCors(corsBuilder => corsBuilder.AllowAnyOrigin());
app.MapControllers();

app.Run();