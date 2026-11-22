namespace ULearn.Be.Common.Attributes;

public class DefaultSortColumnAttribute : Attribute
{
    public DefaultSortColumnAttribute(int orderNumber, bool reversed = false)
    {
        OrderNumber = orderNumber;
        Reversed = reversed;
    }

    public int OrderNumber { get; }
    public bool Reversed { get; }
}