using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables
{
    public partial class CUserRole : CodetableBase
    {
        public CUserRole()
        {
            AppUsers = new HashSet<AppUser>();
        }

        public int UserRoleId { get; set; }
        public virtual ICollection<AppUser> AppUsers { get; set; }
        public virtual ICollection<RNotificationTypeUserRole> RNotificationTypeUserRoles { get; set; } = new List<RNotificationTypeUserRole>();
    }
}

public sealed class CUserRoleConfiguration : IEntityTypeConfiguration<CUserRole>
{
    public void Configure(EntityTypeBuilder<CUserRole> entity)
    {
        entity.HasKey(e => e.UserRoleId)
            .HasName("pk_c_user_role");

        entity.ToTable("c_user_role");

        entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");

        entity.HasIndex(e => e.EnumKey, "ix_c_user_profile_enum_key").IsUnique();

        entity.HasData(CUserRoleSeeder.Entities);
    }
}