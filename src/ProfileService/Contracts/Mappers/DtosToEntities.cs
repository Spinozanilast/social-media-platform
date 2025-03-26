using ProfileService.Entities;
using Profile = ProfileService.Entities.Profile;

namespace ProfileService.Contracts.Mappers;

public static class DtosToEntities
{
    public static Profile ToProfile(this SaveProfileDto saveProfileDto, Guid userId)
    {
        var updatedProfile = new Profile(userId)
        {
            About = saveProfileDto.About,
            Country = new Country
            {
                Id = saveProfileDto.Country.Id,
                IsoCode = saveProfileDto.Country.IsoCode,
                Name = saveProfileDto.Country.Name,
            },
            Interests = saveProfileDto.Interests,
            References = saveProfileDto.References
        };

        return updatedProfile;
    }
}