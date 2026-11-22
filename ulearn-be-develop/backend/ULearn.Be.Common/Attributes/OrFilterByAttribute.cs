namespace ULearn.Be.Common.Attributes;

public class OrFilterByAttribute : Attribute, IAttributeWithName
{
    public OrFilterByAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}