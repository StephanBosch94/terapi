using ULearn.Be.Data;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppRecurrentTimeslotBlacklistRepository
{
    Task<ICollection<AppRecurrentTimeslotBlacklist>> GetAllRaw(int userId);
    Task Add(int recurrentTimeslotId, int localYear, int localMonth, int localDay);
    Task Add(AppRecurrentTimeslot appRecurrentTimeslot, int localYear, int localMonth, int localDay);
    Task DeleteByRecurrentTimeslotId(int recurrentTimeslotId);
}