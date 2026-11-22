namespace ULearn.Be.File.Service.ViewModels;

public class AppFileRaw
{
    public int FileId { get; set; }
    public string UniqueKey { get; set; }
    public string ContentType { get; set; }
    public string Path { get; set; }
    public string FileName { get; set; }
    public string Environment { get; set; }
    public int CreatorId { get; set; }
    public int UpdaterId { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public int FileStateId { get; set; }
    public int CompressionTypeId { get; set; }
}
