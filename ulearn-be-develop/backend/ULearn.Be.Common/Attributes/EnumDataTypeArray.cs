using System.ComponentModel.DataAnnotations;

namespace ULearn.Be.Common.Attributes;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property |
                AttributeTargets.Method)]
public sealed class EnumDataTypeArrayAttribute : DataTypeAttribute
{
    public EnumDataTypeArrayAttribute(Type enumType)
        : base("Enumeration")
    {
        if (enumType == null)
        {
            throw new InvalidOperationException("Type cannot be null");
        }

        if (!enumType.IsEnum)
        {
            throw new InvalidOperationException("Type must be an enum");
        }

        EnumType = enumType;
    }

    private Type EnumType { get; }

    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true;
        }

        var arrayType = value.GetType();
        if (!arrayType.IsArray)
        {
            return false;
        }

        if (EnumType != arrayType.GetElementType())
        {
            return false;
        }

        foreach (var v in (Array)value)
        {
            if (!Enum.IsDefined(EnumType, v))
            {
                return false;
            }
        }

        return true;
    }
}