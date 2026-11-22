using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables
{
    public partial class CTokenType : CodetableBase
    {
        public CTokenType()
        {
            RUserTokens = new HashSet<RUserToken>();
        }

        [Column("token_type_id")] public int TokenTypeId { get; set; }
        public virtual ICollection<RUserToken> RUserTokens { get; set; }
    }
}

public sealed class CTokenTypeConfiguration : IEntityTypeConfiguration<CTokenType>
{
    public void Configure(EntityTypeBuilder<CTokenType> entity)
    {
        entity.HasKey(e => e.TokenTypeId)
            .HasName("pk_c_token_type");

        entity.ToTable("c_token_type");
        
        entity.HasIndex(e => e.EnumKey, "ix_c_token_type_enum_key").IsUnique();

        entity.HasData(CTokenTypeSeeder.Entities);
    }
}