using Microsoft.EntityFrameworkCore;
using ProfileService.Common.Repositories;
using ProfileService.Data;
using ProfileService.Entities;

namespace ProfileService.Repositories;

public class CountriesRepository(ProfilesDbContext context) : ICountriesRepository
{
    public async Task<List<Country>> GetAll()
    {
        return await context.Countries.ToListAsync();
    }
}