using Newtonsoft.Json.Serialization;

namespace ULearn.Be.Common.JsonConverters;

public class LowercaseContractResolver : DefaultContractResolver
{
    public LowercaseContractResolver()
    {
        NamingStrategy = new CamelCaseNamingStrategy
        {
            ProcessDictionaryKeys = false,
            OverrideSpecifiedNames = true
        };
    }
}