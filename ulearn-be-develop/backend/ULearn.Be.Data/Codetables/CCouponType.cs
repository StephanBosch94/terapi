using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CCouponType : CodetableBase
{
    [Column("coupon_type_id")] public int CouponTypeId { get; set; }
    public ICollection<AppCoupon> AppCoupons { get; set; } = new List<AppCoupon>();
}

public sealed class CCouponTypeConfiguration : IEntityTypeConfiguration<CCouponType>
{
    public void Configure(EntityTypeBuilder<CCouponType> entity)
    {
        entity.HasKey(e => e.CouponTypeId)
            .HasName("pk_c_coupon_type");

        entity.ToTable("c_coupon_type");

        entity.HasIndex(e => e.EnumKey, "ix_c_coupon_type_enum_key").IsUnique();

        entity.HasData(CCouponTypeSeeder.Entities);
    }
}