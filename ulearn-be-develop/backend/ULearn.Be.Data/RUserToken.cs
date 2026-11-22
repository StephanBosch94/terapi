using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data
{
    public partial class RUserToken
    {
        public int UserId { get; set; }
        public int TokenTypeId { get; set; }
        public int TokenProviderId { get; set; }
        public string TokenValue { get; set; }

        public virtual CTokenType TokenType { get; set; } = null!;
        public virtual CTokenProvider TokenProvider { get; set; } = null!;
        public virtual AppUser User { get; set; } = null!;
    }
}

public sealed class RUserTokenConfiguration : IEntityTypeConfiguration<RUserToken>
{
    public void Configure(EntityTypeBuilder<RUserToken> entity)
    {
        entity.HasKey(e => new { e.UserId, e.TokenTypeId })
            .HasName("pk_r_user_token");

        entity.ToTable("r_user_token");

        entity.HasIndex(e => e.TokenTypeId, "ix_r_user_token_token_type_id");

        entity.HasIndex(e => e.UserId, "ix_r_user_token_user_id");

        entity.HasIndex(e => e.TokenProviderId, "ix_c_token_type_token_provider_id");

        entity.Property(e => e.UserId).HasColumnName("user_id");

        entity.Property(e => e.TokenTypeId).HasColumnName("token_type_id");

        entity.Property(e => e.TokenProviderId).HasColumnName("token_provider_id");

        entity.Property(e => e.TokenValue).HasColumnName("token_value");

        entity.HasOne(d => d.TokenType)
            .WithMany(p => p.RUserTokens)
            .HasForeignKey(d => d.TokenTypeId)
            .HasConstraintName("fk_r_user_token_c_token_type_token_type_id");

        entity.HasOne(d => d.TokenProvider)
            .WithMany(p => p.RUserTokens)
            .HasForeignKey(d => d.TokenProviderId)
            .HasConstraintName("fk_r_user_token_c_token_provider_token_provider_id");

        entity.HasOne(d => d.User)
            .WithMany(p => p.RUserTokens)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_r_user_token_app_user_user_id");
    }
}