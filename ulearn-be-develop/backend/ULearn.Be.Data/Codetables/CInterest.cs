using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables;

public class CInterest : CodetableBase
{
    [Column("interest_id")] public int InterestId { get; set; }
    [Column("icon_name")] public string IconName { get; set; }
    public virtual ICollection<RUserInterest> RUserInterests { get; set; }
}

public sealed class CInterestConfiguration : IEntityTypeConfiguration<CInterest>
{
    public void Configure(EntityTypeBuilder<CInterest> entity)
    {
        entity.HasKey(e => e.InterestId)
            .HasName("pk_c_interest");

        entity.ToTable("c_interest");

        entity.HasData(CInterestSeeder.Entities);
    }
}