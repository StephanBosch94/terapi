namespace ULearn.Be.Common.Attributes;

public class ComparisonAttribute : Attribute, IAttributeWithName
{
    public ComparisonAttribute(string name = "")
    {
        Name = name;
    }

    public string Name { get; }
}