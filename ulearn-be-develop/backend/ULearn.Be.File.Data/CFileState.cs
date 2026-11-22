using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ULearn.Be.Common.Constants;
using ULearn.Be.File.Data.Seeders;

namespace ULearn.Be.File.Data;

public class CFileState
{
    [Column("file_state_Id")] public int FileStateId { get; set; }
    [Column("enum_key2")] public string EnumKey { get; set; }
    [Column("name_key")] public string NameKey { get; set; }
    [Column("description_Key")] public string DescriptionKey { get; set; }
    public virtual ICollection<AppFile> AppFiles { get; set; }
}

public sealed class CFileStateConfiguration : IEntityTypeConfiguration<CFileState>
{
    public void Configure(EntityTypeBuilder<CFileState> entity)
    {
        entity.HasKey(e => e.FileStateId).HasName("pk_c_file_state");

        entity.ToTable("c_file_state", DatabaseSchemaConstants.Sfile);
        
        entity.HasData(CFileStateSeeder.Entities);
    }
}