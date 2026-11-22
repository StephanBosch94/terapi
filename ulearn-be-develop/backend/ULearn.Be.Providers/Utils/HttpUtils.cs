using System.Text;
using Newtonsoft.Json;

namespace ULearn.Be.Providers.Utils;

public static class HttpUtils
{
    public static StringContent CreateContent(object obj)
    {
        return new StringContent(
            JsonConvert.SerializeObject(obj),
            Encoding.UTF8,
            "application/json"
        );
    }
}