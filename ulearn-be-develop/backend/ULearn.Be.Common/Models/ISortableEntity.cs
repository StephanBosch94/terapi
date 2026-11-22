namespace ULearn.Be.Common.Models;

public interface ISortableEntity
{
    string GetSortProperty(string filterProperty);
}