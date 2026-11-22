using ULearn.Be.Data.Enums;

namespace ULearn.Be.Models.InputModels;

public class AppReservationIM
{
    public int ReservationPackageId { get; set; }
    public int StudentId { get; set; }
    public int TutorId { get; set; }
    public int SubjectId { get; set; }
    public double Price { get; set; }
    public double PriceWithoutSales { get; set; }
    public int RoomId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime ExpectedEndTime { get; set; }
    public string HostEventIanaTimezone { get; set; }
    public CReservationStateEnum ReservationStateEnum { get; set; } = CReservationStateEnum.Created;
    public int? SourceReservationId { get; set; }
    public CClassDurationEnum ClassDurationEnum { get; set; }
    public int? CouponId { get; set; }
    public double? CouponSaleAmount { get; set; }
}