using ProfileService.Entities;
using Profile = ProfileService.Entities.Profile;

namespace ProfileService.Contracts.Mappers;

public static class DtosToEntities
{
    public static Profile ToProfile(this SaveProfileDto saveProfileDto, Guid userId)
    {
        var updatedProfile = new Profile(userId)
        {
            About = saveProfileDto.About ?? "",
            Interests = saveProfileDto.Interests,
            References = saveProfileDto.References
        };

        if (saveProfileDto.Country is not null)
        {
            updatedProfile.Country = new Country
            {
                Id = saveProfileDto.Country.Id,
                Name = saveProfileDto.Country.Name,
                IsoCode = saveProfileDto.Country.IsoCode
            };
        }

        return updatedProfile;
    }
}