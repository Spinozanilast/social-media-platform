using Authentication.Extensions;
using IdentityService;
using IdentityService.Data;
using IdentityService.Entities;
using IdentityService.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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
            new string[] { }
        }
    });
});

builder.Services.AddS3Client();
builder.Services.AddScoped<TokenService>();
builder.Services.AddUsersDbContext(builder);
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<IdentityDbContext>();
builder.Services.AddScoped<IProfileImageService, ProfileImageService>();
builder.Services.Configure<ProfileImageStorageConfig>(builder.Configuration.GetSection("ProfileImageStorage"));

builder.Services.AddJwtConfiguration();

builder.Services.AddAuthentication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();