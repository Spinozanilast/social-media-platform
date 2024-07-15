using Microsoft.EntityFrameworkCore;

namespace IdentityService.Data;

public static class DbInjector
{
    private const string DbName = "UsersDbConnection";
    public static void AddUsersDbContext(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddDbContext<UsersDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString(DbName)));
    }
}
