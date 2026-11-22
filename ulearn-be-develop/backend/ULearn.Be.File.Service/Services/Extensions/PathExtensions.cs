namespace ULearn.Be.File.Service.Services.Extensions;

public static class PathExtensions
{
    public static string NormalizeDirectoryPath(this string directoryPath)
    {
        if (directoryPath == null)
        {
            return "";
        }

        return !directoryPath.EndsWith("/") ? $"{directoryPath}/" : directoryPath;
    }
}
