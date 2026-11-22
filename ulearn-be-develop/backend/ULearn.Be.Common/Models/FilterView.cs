using ULearn.Be.Common.Constants;

namespace ULearn.Be.Common.Models;

public class FilterView<T>
{
    private const int DefaultSkip = CommonConstants.FilterViewDefaultSkip;
    private const int DefaultTake = CommonConstants.FilterViewDefaultTake;

    public FilterView(bool getAll)
    {
        if (getAll)
        {
            Take = CommonConstants.FilterViewMaxTake;
        }

        Sort = new SortView();
    }

    public FilterView()
    {
        Sort = new SortView();
    }

    public int? Skip { get; set; } = DefaultSkip;
    public int? Take { get; set; } = DefaultTake;

    public SortView Sort { get; set; }

    public T Filter { get; set; }
}