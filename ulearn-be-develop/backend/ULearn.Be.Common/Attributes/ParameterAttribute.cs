namespace ULearn.Be.Common.Attributes;

public class ParameterAttribute : Attribute
{
    public ParameterAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}