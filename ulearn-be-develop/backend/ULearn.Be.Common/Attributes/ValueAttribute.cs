namespace ULearn.Be.Common.Attributes;

public class ValueAttribute : Attribute
{
    public ValueAttribute(string textValue)
    {
        TextValue = textValue;
    }

    public string TextValue { get; set; }
}