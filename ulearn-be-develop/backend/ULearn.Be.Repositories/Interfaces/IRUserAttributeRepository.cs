using ULearn.Be.Models.ViewModels.AppUser.Attribute;

namespace ULearn.Be.Repositories.Interfaces;

public interface IRUserAttributeRepository
{
    Task<IEnumerable<UserAttributeVM>> GetByUserId(int userId);
    Task<ILookup<int, UserAttributeVM>> GetAttributesByUserId(IEnumerable<int> userIds);
}