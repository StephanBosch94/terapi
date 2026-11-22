using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data;

public class AppCoupon
{
    [Column("coupon_id")] public int CouponId { get; set; }
    [Column("coupon_type_id")] public int CouponTypeId { get; set; }
    [Column("discount_percentage")] public int DiscountPercentage { get; set; }
    [Column("created")] public DateTime Created { get; set; }
    [Column("valid_from")] public DateTime ValidFrom { get; set; }
    [Column("valid_to")] public DateTime ValidTo { get; set; }
    [Column("user_id")] public int? UserId { get; set; }
    [Column("code")] public string Code { get; set; }

    public CCouponType CCouponType { get; set; }
    public AppUser AppUser { get; set; }
    public ICollection<RUserActivatedCoupon> RUserActivatedCoupons { get; set; } = new List<RUserActivatedCoupon>();
    public ICollection<AppReservation> AppReservations { get; set; } = new List<AppReservation>();
    public ICollection<AppTransaction> AppTransactions { get; set; } = new List<AppTransaction>();
}

public sealed class AppCouponConfiguration : IEntityTypeConfiguration<AppCoupon>
{
    public void Configure(EntityTypeBuilder<AppCoupon> entity)
    {
        entity.HasKey(e => e.CouponId)
            .HasName("pk_app_coupon");

        entity.ToTable("app_coupon");

        entity.HasIndex(e => e.CouponId, "ix_app_coupon_coupon_type_id");
        entity.HasIndex(e => e.UserId, "ix_app_coupon_user_id");

        entity.HasOne(d => d.CCouponType)
            .WithMany(p => p.AppCoupons)
            .HasForeignKey(d => d.CouponTypeId)
            .HasConstraintName("fk_app_coupon_c_coupon_type_coupon_type_id");

        entity.HasOne(d => d.AppUser)
            .WithMany(p => p.AppCoupons)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_app_coupon_app_user_user_id");
    }
}