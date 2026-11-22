using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Data.Codetables;

namespace ULearn.Be.Data;

public class AppMessage
{
    [Column("message_id")] public int MessageId { get; set; }
    [Column("conversation_id")] public int ConversationId { get; set; }
    [Column("sender_id")] public int SenderId { get; set; }
    [Column("receiver_id")] public int ReceiverId { get; set; }
    [Column("message_state_id")] public int MessageStateId { get; set; }
    [Column("text_value")] public string TextValue { get; set; }
    [Column("created")] public DateTime Created { get; set; }
    [Column("updated")] public DateTime Updated { get; set; }
    [Column("notified")] public DateTime? Notified { get; set; }
    public virtual AppConversation AppConversation { get; set; }
    public virtual AppUser Sender { get; set; }
    public virtual AppUser Receiver { get; set; }
    public virtual CMessageState CMessageState { get; set; }
    public virtual ICollection<AppMessageAttachment> AppMessageAttachments { get; set; }
}

public sealed class AppMessageConfiguration : IEntityTypeConfiguration<AppMessage>
{
    public void Configure(EntityTypeBuilder<AppMessage> entity)
    {
        entity.HasKey(e => e.MessageId)
            .HasName("pk_app_message_message_id");

        entity.ToTable("app_message");

        entity.HasIndex(e => e.SenderId, "ix_app_message_sender_id");
        entity.HasIndex(e => e.ReceiverId, "ix_app_message_receiver_id");
        entity.HasIndex(e => e.MessageStateId, "ix_app_message_message_state_id");
        entity.HasIndex(e => e.ConversationId, "ix_app_message_conversation_id");

        entity.HasOne(d => d.Sender)
            .WithMany(p => p.SentAppMessages)
            .HasForeignKey(d => d.SenderId)
            .HasConstraintName("fk_app_message_app_user_sender_id");

        entity.HasOne(d => d.Receiver)
            .WithMany(p => p.ReceivedAppMessages)
            .HasForeignKey(d => d.ReceiverId)
            .HasConstraintName("fk_app_message_app_user_receiver_id");

        entity.HasOne(d => d.CMessageState)
            .WithMany(p => p.AppMessages)
            .HasForeignKey(d => d.MessageStateId)
            .HasConstraintName("fk_app_message_c_message_state_message_state_id");

        entity.HasOne(d => d.AppConversation)
            .WithMany(p => p.AppMessages)
            .HasForeignKey(d => d.ConversationId)
            .HasConstraintName("fk_app_message_app_conversation_conversation_id");
    }
}