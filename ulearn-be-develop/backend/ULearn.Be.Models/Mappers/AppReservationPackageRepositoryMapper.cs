using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ULearn.Be.Data;
using ULearn.Be.Models.ViewModels.ReservationPackage;

namespace ULearn.Be.Models.Mappers;

public static class AppReservationPackageRepositoryMapper
{
    private static readonly Expression<Func<AppReservationPackage, AppReservationPackageOpponentVM>> AsOpponentVM =
        dataItem => new AppReservationPackageOpponentVM
        {
            ReservationPackageId = dataItem.ReservationPackageId,
            Host = AppUserMapper.ToCommonVm(dataItem.Host),
            Attender = AppUserMapper.ToCommonVm(dataItem.Attender),
        };

    public static IQueryable<AppReservationPackageOpponentVM> ToOpponentList(
        this IQueryable<AppReservationPackage> dataCollection)
    {
        return dataCollection
            .Include(x => x.Attender)
            .ThenInclude(x => x.RUserFiles)
            .Include(x => x.Host)
            .ThenInclude(x => x.RUserFiles)
            .Select(AsOpponentVM);
    }
}