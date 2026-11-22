using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Repositories.Interfaces;

public interface IMasterHashRepository
{
    Task<int> Update(ICollection<string> enumKeys, Guid? newGuid = null);
    Task<ICollection<CMasterHash>> GetByEnumKeyPrefix(string enumKeyPrefix);
    Task<DateTime> GetLastUpdate(CMasterHashEnum masterHash);
}