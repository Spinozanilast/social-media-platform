using ProfileService.Entities;
using ProfileService.Models;

namespace ProfileService.Common.Extensions;

public static class ProfileExtensions
{
    public static ProfileWithStringifiedInterests GetProfileWithStringifiedInterests(this Profile profile)
    {
        return new ProfileWithStringifiedInterests
        {
            About = profile.About,
            Anything = profile.Anything,
            BirthDate = profile.BirthDate,
            Country = profile.Country,
            Interests = profile.Interests.Select(i => i.Name).ToArray(),
            Refs = profile.References.ToArray()
        };
    }
}