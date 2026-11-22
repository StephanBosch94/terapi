using ULearn.Be.Data;

namespace ULearn.Be.Models.RepositoryExtensions;

public static class AppUserRepositoryExtension
{
    public static IQueryable<AppUser> FilterHasFutureReservations(this IQueryable<AppUser> dataCollection, DateTime now,
        bool isAdmin)
    {
        return dataCollection
            .Where(x => isAdmin || x.AppTimeslots.Any(y => y.FromTime >= now) || x.AppRecurrentTimeslots.Any());
    }
}