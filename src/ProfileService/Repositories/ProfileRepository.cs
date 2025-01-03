using Microsoft.EntityFrameworkCore;
using ProfileService.Common.Repositories;
using ProfileService.Data;
using ProfileService.Entities;

namespace ProfileService.Repositories;

public class ProfileRepository(ProfileDbContext context) : IProfileRepository
{
    public async Task InitUserProfileAsync(Guid userId)
    {
        var profile = new Profile(userId);
        context.Profiles.Add(profile);
        await context.SaveChangesAsync();
    }

    public async Task SaveProfileAsync(Profile profile)
    {
        context.Profiles.Update(profile);
        await context.SaveChangesAsync();
    }

    public async Task<Profile?> GetProfileAsync(Guid userId)
    {
        return await context
            .Profiles
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task DeleteProfileAsync(Guid userId)
    {
        var profile = await GetProfileAsync(userId);
        if (profile is not null)
        {
            context.Profiles.Remove(profile);
            await context.SaveChangesAsync();
        }
    }
}