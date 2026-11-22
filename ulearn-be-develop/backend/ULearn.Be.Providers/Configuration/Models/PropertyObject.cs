using System.Reflection;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Providers.Configuration.Models;

public class PropertyObject
{
    public EnvConfigurationDataTypeEnum DataTypeEnum { get; set; }
    public PropertyInfo Property { get; set; }
    public object Value { get; set; }
    public string OriginalValue { get; set; }
}