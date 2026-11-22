namespace ULearn.Be.Common.Utils;

public static class EnvConfigUtils
{
    public static readonly bool IsDevelopment =
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
}