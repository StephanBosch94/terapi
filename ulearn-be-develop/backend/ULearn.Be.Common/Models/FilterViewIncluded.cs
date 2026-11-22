namespace ULearn.Be.Common.Models;

public class FilterViewIncluded<TFilter, TInclude> : FilterView<TFilter>
{
    public TInclude Include { get; set; }
}