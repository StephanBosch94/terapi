using ULearn.Be.Data.Enums;

namespace ULearn.Be.Models.InputModels;

public class UserFileInputVM: FileInputBaseVM
{
    public CFileTypeEnum FileTypeId { get; set; }
}