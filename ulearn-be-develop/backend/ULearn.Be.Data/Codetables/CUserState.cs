using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables
{
    public partial class CUserState : CodetableBase
    {
        public CUserState()
        {
            AppUsers = new HashSet<AppUser>();
        }

        [Column("user_state_id")] public int UserStateId { get; set; }
        [Column("company_admin_name_key")] public string CompanyAdminNameKey { get; set; }
        public virtual ICollection<AppUser> AppUsers { get; set; }
    }
}

public sealed class CUserStateConfiguration : IEntityTypeConfiguration<CUserState>
{
    public void Configure(EntityTypeBuilder<CUserState> entity)
    {
        entity.HasKey(e => e.UserStateId)
            .HasName("pk_c_user_state");

        entity.ToTable("c_user_state");

        entity.HasIndex(e => e.EnumKey, "ix_c_user_state_enum_key").IsUnique();

        entity.HasData(CUserStateSeeder.Entities);
    }
}