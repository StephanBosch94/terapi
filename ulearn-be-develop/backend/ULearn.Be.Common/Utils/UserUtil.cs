namespace ULearn.Be.Common.Utils;

public static class UserUtil
{
    public static string GetUserName(string firstName, string lastName, string login = null)
    {
        return (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            ? $"{firstName} {lastName}"
            : login;
    }
}