using System.ComponentModel;
using ULearn.Be.Common.Attributes;

namespace ULearn.Be.File.Data.Enums;

public enum CCompressionTypeEnum
{
    [EnumKey("none")][Description("none")] None = 1,
    [EnumKey("zip")][Description("zip")] Zip = 2
}
