using ULearn.Be.Common.Utils;
using ULearn.Be.Data.Enums;
using ULearn.Be.Models.ViewModels.File;

namespace ULearn.Be.Models.ViewModels.Common;

public record AppUserBaseVM
{
    public int UserId { get; set; }
    public int UserRoleId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Timezone { get; set; }
    public string Ico { get; set; }
    public string Dic { get; set; }
    public bool IsVatPayer { get; set; }
    public int? TranslationLanguageId { get; set; }
    public bool CertificationsVerified { get; set; }
    public IEnumerable<UserFileVM> Files { get; set; }

    public string GetUserName() => UserUtil.GetUserName(FirstName, LastName);

    public UserFileVM GetThumbnail() => Files
        .FirstOrDefault(x => x.FileTypeId == (int)CFileTypeEnum.ProfilePhotoThumbnail);

    public string GetProfileThumbnailUrl(string baseUrl)
    {
        var thumbnail = GetThumbnail();
        return thumbnail != null
            ? $"{UrlUtil.GetBaseUrl(baseUrl)}api/file/filepath/{GetThumbnail().FilePath}"
            : string.Empty;
    }
}