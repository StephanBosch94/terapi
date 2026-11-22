using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CRoomStatisticType : CodetableBase
{
    public int RoomStatisticTypeId { get; set; }
    public virtual ICollection<RUserRoomStatistic> RUserRoomStatistics { get; set; }
}

public sealed class CRoomStatisticTypeConfiguration : IEntityTypeConfiguration<CRoomStatisticType>
{
    public void Configure(EntityTypeBuilder<CRoomStatisticType> entity)
    {
        entity.HasKey(e => e.RoomStatisticTypeId)
            .HasName("pk_room_statistic_type");

        entity.ToTable("c_room_statistic_type");

        entity.Property(e => e.RoomStatisticTypeId).HasColumnName("room_statistic_type_id");

        entity.HasData(CRoomStatisticTypeSeeder.Entities);
    }
}