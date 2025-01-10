using Amazon;
using ProfileService.Entities;
using Profile = ProfileService.Entities.Profile;

namespace ProfileService.Contracts.Mappers;

public static class DtosToEntities
{
    public static Profile ToProfile(this SaveProfileDto saveProfileDto)
    {
        var updatedProfile = new Profile(saveProfileDto.UserId);

        updatedProfile.About = saveProfileDto.About;
        updatedProfile.Anything = saveProfileDto.Anything;
        updatedProfile.Country = new Country
        {
            Id = saveProfileDto.Country.Id,
            IsoCode = saveProfileDto.Country.IsoCode,
            Name = saveProfileDto.Country.Name,
        };
        updatedProfile.Interests = saveProfileDto.Interests;
        updatedProfile.References = saveProfileDto.References;

        return updatedProfile;
    }
}