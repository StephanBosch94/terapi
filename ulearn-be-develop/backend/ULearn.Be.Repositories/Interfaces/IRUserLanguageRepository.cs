using ULearn.Be.Models.ViewModels.AppUser.Language;

namespace ULearn.Be.Repositories.Interfaces;

public interface IRUserLanguageRepository
{
    Task<IEnumerable<AppUserLanguageVM>> GetByUserId(int userId);
    Task<ILookup<int, AppUserLanguageVM>> GetLanguagesByUserId(IEnumerable<int> userIds);
}