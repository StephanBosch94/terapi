namespace ULearn.Be.Models.ViewModels.Reservation;

public class AppReservationForPaymentVM
{
    public int ReservationId { get; set; }
    public int ReservationPackageId { get; set; }
    public int SubjectId { get; set; }
    public int HostId { get; set; }
    public int AttenderId { get; set; }
    public int ReservationStateId { get; set; }
    public double Price { get; set; }
    public int CurrencyId { get; set; }
    public double PriceWithoutSales { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public int RoomId { get; set; }
    public string HostEventIanaTimezone { get; set; }
    public int? SourceReservationId { get; set; }
    public bool HasApprovedRefundRequest { get; set; }
}