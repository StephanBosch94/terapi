using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum EnvConfigurationDataTypeEnum
{
    [EnumKey("string")] String = 1,
    [EnumKey("string_array")] StringArray = 2,
    [EnumKey("integer")] Integer = 3,
    [EnumKey("object")] Object = 4,
    [EnumKey("object_array")] ObjectArray = 5,
    [EnumKey("boolean")] Boolean = 6,
    [EnumKey("object_map")] ObjectMap = 7,
    [EnumKey("integer_array")] IntegerArray = 8,
    [EnumKey("password")] Password = 9,
    [EnumKey("long")] Long = 10
}