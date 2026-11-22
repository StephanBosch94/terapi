namespace ULearn.Be.Common.Utils;

public static class UrlUtil
{
    public static string GetBaseUrl(string url)
    {
        return !url.EndsWith("/") ? $"{url}/" : url;
    }

    public static string GetTutorProfileRelativeUrl(int userId) => $"/tutor/{userId}";
}