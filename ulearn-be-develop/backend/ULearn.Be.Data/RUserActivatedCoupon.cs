using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ULearn.Be.Data;

public class RUserActivatedCoupon
{
    [Column("user_activated_coupon_id")] public int UserActivatedCouponId { get; set; }
    [Column("user_id")] public int UserId { get; set; }
    [Column("coupon_id")] public int CouponId { get; set; }
    [Column("activated")] public DateTime Activated { get; set; }
    public AppUser AppUser { get; set; }
    public AppCoupon AppCoupon { get; set; }
}

public sealed class RUserActivatedCouponConfiguration : IEntityTypeConfiguration<RUserActivatedCoupon>
{
    public void Configure(EntityTypeBuilder<RUserActivatedCoupon> entity)
    {
        entity.HasKey(e => e.UserActivatedCouponId)
            .HasName("pk_r_user_activated_coupon");

        entity.ToTable("r_user_activated_coupon");

        entity.HasIndex(e => e.CouponId, "ix_r_user_activated_coupon_coupon_id");
        entity.HasIndex(e => e.UserId, "ix_r_user_activated_coupon_user_id");

        entity.HasOne(d => d.AppCoupon)
            .WithMany(p => p.RUserActivatedCoupons)
            .HasForeignKey(d => d.CouponId)
            .HasConstraintName("fk_r_user_activated_coupon_app_coupon_coupon_id");

        entity.HasOne(d => d.AppUser)
            .WithMany(p => p.RUserActivatedCoupons)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_r_user_activated_coupon_app_user_user_id");
    }
}