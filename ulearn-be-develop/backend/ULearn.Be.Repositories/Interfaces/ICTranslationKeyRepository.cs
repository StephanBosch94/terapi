using ULearn.Be.Models.InputModels;

namespace ULearn.Be.Repositories.Interfaces;

public interface ICTranslationKeyRepository
{
    Task<int> BatchUpsert(ICollection<AppTranslationIM> translations, int userId);
}