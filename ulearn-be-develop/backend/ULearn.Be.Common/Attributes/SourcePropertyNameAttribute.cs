namespace ULearn.Be.Common.Attributes;

/// <summary>
///     zajistuje mapovani property mezi Filterem a Viewmodelem pro automaticke filtrovani
/// </summary>
public class SourcePropertyNameAttribute : Attribute, IAttributeWithName
{
    public SourcePropertyNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}