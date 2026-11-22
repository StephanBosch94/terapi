using ULearn.Be.Models.InputModels;

namespace ULearn.Be.Repositories.Interfaces;

public interface IRUserTokenRepository
{
    Task Upsert(int userId, IEnumerable<UserTokenIM> tokenIMs);
}