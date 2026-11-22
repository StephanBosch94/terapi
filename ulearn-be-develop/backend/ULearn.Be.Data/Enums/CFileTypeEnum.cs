using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum CFileTypeEnum
{
    [EnumKey("profile_photo")] ProfilePhoto = 1,
    [EnumKey("profile_photo_thumbnail")] ProfilePhotoThumbnail = 2,
    [EnumKey("certificate")] Certificate = 3
}