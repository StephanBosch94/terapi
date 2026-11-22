using System.ComponentModel.DataAnnotations;

namespace ULearn.Be.Common.Models;

public class PagedVM<T>
{
    public PagedVM() : this(0, new List<T>())
    {
    }

    public PagedVM(T data) : this(new List<T> { data })
    {
    }

    public PagedVM(IEnumerable<T> data) : this(data.ToList())
    {
    }

    public PagedVM(ICollection<T> data) : this(data.Count, data)
    {
    }

    public PagedVM(int size, Task<ICollection<T>> data) : this(size, data.Result)
    {
    }

    public PagedVM(int size, IEnumerable<T> data) : this(size, data.ToList())
    {
    }

    public PagedVM(int size, ICollection<T> data, string status = "ok")
    {
        Size = size;
        Data = data ?? new List<T>();
        Status = status;
    }

    public int Size { get; set; }
    [Required] public ICollection<T> Data { get; set; }
    public string Status { get; set; }
}