using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.Common;

namespace ULearn.Be.Repositories.Interfaces;

public interface IAppReservationPackageRepository
{
    Task<int> Create(int tutorId, int studentId, double totalPrice, CClassDurationEnum durationEnum);
    Task Cancel(int packageId, string reasonText);
    Task<double> GetTotalPrice(int reservationPackageId);
    Task<AppReservationPackage> GetRawDbEntity(int reservationPackageId);

    Task<IReadOnlyDictionary<int, AppUserCommonVM>> GetOpponentByPackageId(
        IReadOnlyCollection<int> reservationPackageIds, int userId);
}