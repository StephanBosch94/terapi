namespace ULearn.Be.Common.Attributes;

public class SettingsObjectNameAttribute : Attribute
{
    public SettingsObjectNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}