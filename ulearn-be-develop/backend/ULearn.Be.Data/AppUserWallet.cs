using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data
{
    public partial class AppUserWallet
    {
        public AppUserWallet()
        {
            AppTransactions = new HashSet<AppTransaction>();
        }

        [Column("user_wallet_id")] public int UserWalletId { get; set; }
        [Column("user_id")] public int UserId { get; set; }
        [Column("currency_id")] public int CurrencyId { get; set; }
        [Column("amount")] public double Amount { get; set; }
        [Column("scheduled_amount")] public double ScheduledAmount { get; set; }
        [Column("total_revenue")] public double TotalRevenue { get; set; }
        [Column("updated")] public DateTime? Updated { get; set; }
        [Column("bank_account_number")] public string BankAccountNumber { get; set; }
        [Column("bank_code")] public string BankCode { get; set; }
        public virtual CCurrency Currency { get; set; } = null!;
        public virtual AppUser User { get; set; } = null!;
        public virtual ICollection<AppTransaction> AppTransactions { get; set; }
    }
}

public sealed class AppUserWalletConfiguration : IEntityTypeConfiguration<AppUserWallet>
{
    public void Configure(EntityTypeBuilder<AppUserWallet> entity)
    {
        entity.HasKey(e => e.UserWalletId)
            .HasName("pk_app_user_wallet");

        entity.ToTable("app_user_wallet");

        entity.HasIndex(e => e.UserId, "ix_app_user_wallet_user_id");

        entity.HasOne(d => d.Currency)
            .WithMany(p => p.AppUserWallets)
            .HasForeignKey(d => d.CurrencyId)
            .HasConstraintName("fk_app_user_wallet_c_currency_currency_id");

        entity.HasOne(d => d.User)
            .WithMany(p => p.AppUserWallets)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("fk_app_user_wallet_app_user_user_id");
    }
}