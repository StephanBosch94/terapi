using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables
{
    public partial class CTokenProvider : CodetableBase
    {
        public CTokenProvider()
        {
            RUserTokens = new HashSet<RUserToken>();
        }

        public int TokenProviderId { get; set; }

        public virtual ICollection<RUserToken> RUserTokens { get; set; }
    }
}

public sealed class CTokenProviderConfiguration : IEntityTypeConfiguration<CTokenProvider>
{
    public void Configure(EntityTypeBuilder<CTokenProvider> entity)
    {
        entity.HasKey(e => e.TokenProviderId)
            .HasName("pk_c_token_provider");

        entity.ToTable("c_token_provider");

        entity.Property(e => e.TokenProviderId).HasColumnName("token_provider_id");

        entity.HasIndex(e => e.EnumKey, "ix_c_token_provider_enum_key").IsUnique();

        entity.HasData(CTokenProviderSeeder.Entities);
    }
}