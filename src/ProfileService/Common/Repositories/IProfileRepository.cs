using ProfileService.Entities;

namespace ProfileService.Common.Repositories;

public interface IProfileRepository
{
    Task InitUserProfileAsync(Guid userId, ICollection<string>? references);
    Task UpdateProfileAsync(Profile profile);
    Task<Profile?> GetProfileAsync(Guid userId);
    Task DeleteProfileAsync(Guid userId);
}