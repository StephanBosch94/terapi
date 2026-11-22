using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ULearn.Be.Data;

public class AppConversation
{
    public int ConversationId { get; set; }
    public int InitiatorId { get; set; }
    public int OpponentId { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }

    public virtual AppUser Initiator { get; set; }
    public virtual AppUser Opponent { get; set; }
    public virtual ICollection<AppMessage> AppMessages { get; set; }
}

public sealed class AppConversationConfiguration : IEntityTypeConfiguration<AppConversation>
{
    public void Configure(EntityTypeBuilder<AppConversation> entity)
    {
        entity.HasKey(e => e.ConversationId)
            .HasName("pk_app_conversation");

        entity.ToTable("app_conversation");

        entity.HasIndex(e => e.InitiatorId, "ix_app_conversation_initiator_id");
        entity.HasIndex(e => e.OpponentId, "ix_app_conversation_opponent_id");

        entity.Property(e => e.ConversationId).HasColumnName("conversation_id");
        entity.Property(e => e.InitiatorId).HasColumnName("initiator_id");
        entity.Property(e => e.OpponentId).HasColumnName("opponent_id");
        entity.Property(e => e.Created).HasColumnName("created");
        entity.Property(e => e.Updated).HasColumnName("updated");

        entity.HasOne(d => d.Initiator)
            .WithMany(p => p.InitiatorAppConversations)
            .HasForeignKey(d => d.InitiatorId)
            .HasConstraintName("fk_app_conversation_app_user_initiator_id");

        entity.HasOne(d => d.Opponent)
            .WithMany(p => p.OpponentAppConversations)
            .HasForeignKey(d => d.OpponentId)
            .HasConstraintName("fk_app_conversation_app_user_opponent_id");
    }
}