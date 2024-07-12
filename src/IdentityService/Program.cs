using IdentityService;
using IdentityService.Data;
using IdentityService.Entities;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddUsersDbContext(builder);
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<UsersDbContext>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();