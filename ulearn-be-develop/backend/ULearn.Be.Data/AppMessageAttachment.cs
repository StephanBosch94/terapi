using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ULearn.Be.Data;

public class AppMessageAttachment
{
    public int MessageAttachmentId { get; set; }
    public string Name { get; set; }
    public int MessageId { get; set; }
    public string ContentType { get; set; }
    public string FilePath { get; set; }
    public byte[] Content { get; set; }
    public virtual AppMessage AppMessage { get; set; }
}

public sealed class AppMessageAttachmentConfiguration : IEntityTypeConfiguration<AppMessageAttachment>
{
    public void Configure(EntityTypeBuilder<AppMessageAttachment> entity)
    {
        entity.HasKey(e => e.MessageAttachmentId)
            .HasName("pk_app_message_attachment");

        entity.ToTable("app_message_attachment");

        entity.Property(e => e.MessageAttachmentId).HasColumnName("message_attachment_id");
        entity.Property(e => e.MessageId).HasColumnName("message_id");
        entity.Property(e => e.FilePath).HasColumnName("file_path").HasMaxLength(256);
        entity.Property(e => e.ContentType).HasColumnName("content_type").HasMaxLength(512);
        entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(512);
        entity.Property(e => e.Content).HasColumnName("content");

        entity.HasIndex(e => e.MessageId, "ix_app_message_attachment_message_id");

        entity.HasOne(d => d.AppMessage)
            .WithMany(p => p.AppMessageAttachments)
            .HasForeignKey(d => d.MessageId)
            .HasConstraintName("fk_app_message_attachment_app_message_message_id");
    }
}