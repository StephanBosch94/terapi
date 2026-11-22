using ULearn.Be.Models.ViewModels.Reservation;

namespace ULearn.Be.Models.ViewModels.AppUser;

public record AppShortUserInfoVM : AppUserBasicVM
{
    public AppShortUserInfoVM(AppUserBasicVM entity) : base(entity)
    {
    }

    public AppReservationVM RunningActivity { get; set; }
    public bool HasFutureBookings { get; set; }
    public bool HasBookedAnyLesson { get; set; }
}