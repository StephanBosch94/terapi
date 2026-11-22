namespace ULearn.Be.Repositories.Interfaces;

public interface ICTranslationLanguageRepository
{
    Task<int> GetTranslationLanguageId(string languageCode, bool throwIfNotFound = true);
}