using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Repositories.Interfaces;

public interface IRUserInterestRepository
{
    Task<IEnumerable<CInterestVM>> GetByUserId(int userId);
    Task<ILookup<int, CInterestVM>> GetInterestByUserId(IEnumerable<int> userIds);
}