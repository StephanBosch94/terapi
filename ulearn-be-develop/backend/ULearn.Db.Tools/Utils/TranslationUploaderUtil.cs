using System.Net.Http.Headers;
using Newtonsoft.Json;
using ULearn.Db.Tools.Models;

namespace ULearn.Db.Tools.Utils;

public static class TranslationUploaderUtil
{
    public static async Task<HttpResponseMessage> Upload(string api, string token, IEnumerable<TranslationVM> translations)
    {
        var serialized = JsonConvert.SerializeObject(translations);
        
        using var httpClient = new HttpClient();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) },
            RequestUri = new Uri(api),
            Content = new StringContent(serialized) {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            }
        };

        var response = await httpClient.SendAsync(request);
        return response;
    }
}