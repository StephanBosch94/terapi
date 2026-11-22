using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ULearn.Be.Data.Codetables.Base;

public class CodetableBase
{
    [Column("enum_key")] [MaxLength(100)] public string EnumKey { get; set; } = null!;
    [Column("name_key")] [MaxLength(100)] public string NameKey { get; set; } = null!;

    [Column("description_key")]
    [MaxLength(100)]
    public string DescriptionKey { get; set; } = null!;
}