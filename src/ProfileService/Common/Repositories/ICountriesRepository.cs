using ProfileService.Entities;

namespace ProfileService.Common.Repositories;

public interface ICountriesRepository
{
    Task<List<Country>> GetAll();
}