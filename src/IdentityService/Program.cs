using Amazon.Runtime;
using Authentication.Extensions;
using AwsConfigurators;
using IdentityService;
using IdentityService.Data;
using IdentityService.Entities;
using IdentityService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();