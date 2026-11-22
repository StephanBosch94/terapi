namespace ULearn.Be.Models.InputModels;

public class FileInputBaseVM
{
    public string Directory { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string FilePath => $"{Directory}/{FileName}";
}