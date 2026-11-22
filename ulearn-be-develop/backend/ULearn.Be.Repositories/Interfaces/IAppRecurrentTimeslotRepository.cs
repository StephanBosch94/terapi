using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.AppRecurrentTimeslot;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppRecurrentTimeslotRepository
{
    Task<ICollection<AppRecurrentTimeslot>> GetAllRaw(int userId, int? isoWeekday = null);
    Task Add(AppRecurrentTimeslotInputVM input, int userId);
    Task Delete(int recurrentTimeslotId, int userId);
    Task<AppRecurrentTimeslot> Get(int recurrentTimeslotId, int userId);
    Task<bool> Exists(int userId);
}