using ULearn.Be.Models.ViewModels.Codetables;

namespace ULearn.Be.Repositories.Interfaces;

public interface IRUserSubjectLevelRepository
{
    Task<IEnumerable<CSubjectLevelVM>> GetByUserId(int userId);
    Task<ILookup<int, CSubjectLevelVM>> GetSubjectLevelsByUserId(IEnumerable<int> userIds);
}