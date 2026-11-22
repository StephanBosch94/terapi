using Newtonsoft.Json;

namespace ULearn.Be.Models.ViewModels.File;

public class UserFileVM
{
    [JsonIgnore] public int UserId { get; set; }
    public int FileTypeId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string ContentType { get; set; }
}