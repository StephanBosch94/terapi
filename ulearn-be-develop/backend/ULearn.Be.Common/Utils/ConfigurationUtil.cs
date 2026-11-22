using Newtonsoft.Json;
using ULearn.Be.Common.Constants;
using ULearn.Be.Common.Exceptions;

namespace ULearn.Be.Common.Utils;

public static class ConfigurationUtil
{
    private const string BasePath = "../ULearn.Be.Api";

    public static string LoadConnectionString(string name)
    {
        
        var filePath = GetFilePath();
        var fileName = Path.GetFileName(filePath);
        if (!System.IO.File.Exists(filePath))
        {
            throw new NotFoundException($"Cannot find file {fileName}");
        }

        var lines = System.IO.File.ReadAllText(filePath);
        var configuration = JsonConvert.DeserializeObject<ConfigurationModel>(lines);
        if (configuration == null)
        {
            throw new ArgumentException($"File {fileName} is not valid json");
        }

        if (configuration.ConnectionStrings == null)
        {
            throw new NotFoundException($"File {fileName} does not have section ConnectionStrings");
        }

        if (!configuration.ConnectionStrings.ContainsKey(name))
        {
            throw new NotFoundException($"File {fileName} does not have section ConnectionStrings.{name}");
        }

        return configuration.ConnectionStrings[name];
    }

    private static string GetFilePath()
    {
        var environmentName = Environment.GetEnvironmentVariable(CommonConstants.EnvironmentNameVariable);
        const string appSetting = $"{BasePath}/appsettings.json";
        var envSpecificSetting = $"{BasePath}/appsettings.{environmentName}.json";

        return !System.IO.File.Exists(envSpecificSetting) ? appSetting : envSpecificSetting;
    }

    private class ConfigurationModel
    {
        public Dictionary<string, string> ConnectionStrings { get; set; }
    }
}