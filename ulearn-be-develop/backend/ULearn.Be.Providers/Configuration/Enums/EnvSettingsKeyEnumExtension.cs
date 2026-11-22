using System.Reflection;
using ULearn.Be.Common.Attributes;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Providers.Configuration.Enums;

public static class EnvSettingsKeyEnumExtension
{
    public static string GetSettingsObjectName(this EnvSettingsKeyEnum value)
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        return type.GetField(name)
            .GetCustomAttribute<SettingsObjectNameAttribute>()
            ?.Name;
    }

    public static bool IsOptionalSettings(this EnvSettingsKeyEnum value)
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        var optional = type.GetField(name)
            .GetCustomAttribute<OptionalSettingsAttribute>();
        return optional != null;
    }
}