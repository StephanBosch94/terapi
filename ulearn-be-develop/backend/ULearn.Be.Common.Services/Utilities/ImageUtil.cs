namespace ULearn.Be.Common.Services.Utilities;

public static class ImageUtil
{
    public static string GetFileUrl(string basePath, string filePath)
    {
        var baseWithSeparator = !string.IsNullOrEmpty(basePath) && basePath.Last() != '/' ? basePath + '/' : basePath;
        var encoded = Uri.EscapeDataString(filePath ?? "");
        return $"{baseWithSeparator}api/file/filepath/{encoded}";
    }
}