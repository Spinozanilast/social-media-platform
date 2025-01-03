using Microsoft.EntityFrameworkCore;
using ProfileService.Common.Repositories;
using ProfileService.Data;
using ProfileService.Entities;

namespace ProfileService.Repositories;

public class CountriesRepository(ProfileDbContext context) : ICountriesRepository
{
    public Task<List<Country>> GetAll()
    {
        return context.Countries.ToListAsync();
    }
}