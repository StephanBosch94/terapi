namespace ULearn.Be.Common.Attributes;

public class IconNameAttribute : Attribute
{
    public IconNameAttribute(string icon)
    {
        Icon = icon;
    }

    public string Icon { get; }
}