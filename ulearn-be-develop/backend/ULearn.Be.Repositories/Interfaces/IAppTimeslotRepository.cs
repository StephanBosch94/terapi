using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.AppTimeslot;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppTimeslotRepository
{
    Task<ICollection<AppTimeslot>> GetAllRaw(int userId);
    Task Add(AppTimeslotInputVM input, int userId);
    Task Delete(int timeslotId, int userId);
    Task<IEnumerable<AppTimeslotBaseVM>> GetBaseVMs(int userId);
    Task CreateRange(IEnumerable<AppTimeslot> appTimeslots);
    Task<bool> Exists(int userId);
}