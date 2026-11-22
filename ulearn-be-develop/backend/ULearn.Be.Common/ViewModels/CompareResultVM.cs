namespace ULearn.Be.Common.ViewModels;

public class CompareResultVM<T>
{
    public IReadOnlyCollection<T> ToAdd { get; set; }
    public IReadOnlyCollection<T> ToRemove { get; set; }
    public IReadOnlyCollection<(T dbEntity, T newerEntity)> ToUpdate { get; set; }
}