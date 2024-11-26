using System.Globalization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProfileService.Entities;

namespace ProfileService.Data.Helpers;

public static class CountriesSeedExtensions
{
    public static void SeedCountriesTableWithData(this EntityTypeBuilder<Country> builder)
    {
        var cultureInfos = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

        var countriesInfo = new SortedSet<(string countryName, string twoLetterIso)>();

        foreach (var cultureInfo in cultureInfos)
        {
            var regionInfo = new RegionInfo(cultureInfo.Name);
            if (regionInfo.TwoLetterISORegionName.Length == 2)
            {
                countriesInfo.Add((regionInfo.EnglishName, regionInfo.TwoLetterISORegionName));
            }
        }

        var countryIndex = 0;
        builder.HasData(countriesInfo.Select(countryInfo =>
            new Country
            {
                Id = ++countryIndex, IsoCode = countryInfo.twoLetterIso,
                Name = countryInfo.countryName
            }));
    }
}