namespace ULearn.Be.Common.Attributes;

public class ProcedureAttribute : Attribute
{
    public ProcedureAttribute(string name, string schema = "dbo")
    {
        Name = name;
        Schema = schema;
    }

    public string Name { get; }
    public string Schema { get; set; }
}