namespace ULearn.Be.Common.Models;

public class SortView
{
    public SortView()
    {
        Reverse = false;
    }

    public string Predicate { get; set; }
    public bool Reverse { get; set; }
}