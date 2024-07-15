using IdentityService.Data;
using IdentityService.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddUsersDbContext(builder);
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<IdentityDbContext>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();