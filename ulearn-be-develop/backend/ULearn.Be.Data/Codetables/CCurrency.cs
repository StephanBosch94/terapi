using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables
{
    public partial class CCurrency : CodetableBase
    {
        public CCurrency()
        {
            AppUserWallets = new HashSet<AppUserWallet>();
            AppUsers = new HashSet<AppUser>();
        }

        [Column("currency_id")] public int CurrencyId { get; set; }
        [Column("is_base")] public bool IsBase { get; set; }
        [Column("symbol")] public string Symbol { get; set; }
        [Column("rate")] public double Rate { get; set; }
        [Column("last_sync")] public DateTime? LastSync { get; set; }
        public virtual ICollection<AppUserWallet> AppUserWallets { get; set; }
        public virtual ICollection<AppUser> AppUsers { get; set; }
        public virtual ICollection<AppTransaction> AppTransactions { get; set; }
        public virtual ICollection<AppReservation> AppReservations { get; set; }
        public virtual ICollection<AppReservationPackage> AppReservationPackages { get; set; }
    }
}

public sealed class CCurrencyConfiguration : IEntityTypeConfiguration<CCurrency>
{
    public void Configure(EntityTypeBuilder<CCurrency> entity)
    {
        entity.HasKey(e => e.CurrencyId)
            .HasName("pk_c_currency");

        entity.ToTable("c_currency");

        entity.HasIndex(e => e.EnumKey, "ix_c_currency_enum_key").IsUnique();

        entity.HasData(CCurrencySeeder.Entities);
    }
}