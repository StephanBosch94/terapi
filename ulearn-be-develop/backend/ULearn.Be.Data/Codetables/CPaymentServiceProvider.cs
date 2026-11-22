using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;
using ULearn.Be.Data.Codetables.Base;
using ULearn.Be.Data.Seeders;

namespace ULearn.Be.Data.Codetables
{
    public partial class CPaymentServiceProvider : CodetableBase
    {
        public CPaymentServiceProvider()
        {
            AppTransactions = new HashSet<AppTransaction>();
        }

        public int PaymentServiceProviderId { get; set; }
        public virtual ICollection<AppTransaction> AppTransactions { get; set; }
    }
}

public sealed class CPaymentServiceProviderConfiguration : IEntityTypeConfiguration<CPaymentServiceProvider>
{
    public void Configure(EntityTypeBuilder<CPaymentServiceProvider> entity)
    {
        entity.HasKey(e => e.PaymentServiceProviderId)
            .HasName("pk_c_payment_service_provider");

        entity.ToTable("c_payment_service_provider");

        entity.Property(e => e.PaymentServiceProviderId).HasColumnName("payment_service_provider_id");

        entity.HasIndex(e => e.EnumKey, "ix_c_payment_service_provider_enum_key").IsUnique();

        entity.HasData(CPaymentServiceProviderSeeder.Entities);
    }
}