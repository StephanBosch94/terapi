namespace ULearn.Be.Common.Attributes;

public class ObjectNameAttribute : Attribute
{
    public ObjectNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}