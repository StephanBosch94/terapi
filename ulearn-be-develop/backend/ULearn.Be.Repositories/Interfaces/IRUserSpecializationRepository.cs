using ULearn.Be.Models.ViewModels.AppUser.Specialization;

namespace ULearn.Be.Repositories.Interfaces;

public interface IRUserSpecializationRepository
{
    Task<IEnumerable<AppUserSpecializationVM>> GetByUserId(int userId);
    Task<ILookup<int, AppUserSpecializationVM>> GetSpecializationsByUserId(IEnumerable<int> userIds);
}