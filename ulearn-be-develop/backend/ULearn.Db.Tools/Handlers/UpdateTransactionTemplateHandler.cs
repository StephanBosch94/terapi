using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ULearn.Db.Tools.Models.Options;

namespace ULearn.Db.Tools.Handlers;

public class UpdateTransactionTemplateHandler
{
    private readonly ILogger<UpdateTransactionTemplateHandler> _logger;

    public UpdateTransactionTemplateHandler(ILogger<UpdateTransactionTemplateHandler> logger)
    {
        _logger = logger;
    }

    public async Task Run(UpdateTransactionTemplateOption obj)
    {
        try
        {
            var (_, stylesContent) = await GetTemplate(obj.StylesUrl, obj.SourceToken);
            var (filename, content) = await GetTemplate(obj.Url, obj.SourceToken);
            var resultContent = GetWithInlineStyles(stylesContent, content);
            await Send(obj.BeApiUrl, Path.GetFileNameWithoutExtension(filename), resultContent, obj.BearerToken);
        }
        catch (Exception e)
        {
            _logger.LogInformation("Failed to upload transaction template");
            _logger.LogError("{Error}", e.ToString());
        }
    }

    private static string GetWithInlineStyles(string stylesContent, string content)
    {
        const string placeholder = "<link rel=\"stylesheet\" href=\"styles.css\">";
        return content.Replace(placeholder, "<style>" + stylesContent + "</style>");
    }

    private async Task Send(string url, string transactionType, string content, string token)
    {
        using var httpClient = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) },
            RequestUri = new Uri(url),
            Content = new MultipartFormDataContent
            {
                {
                    new StringContent(content)
                    {
                        Headers = { ContentType = new MediaTypeHeaderValue("text/html") },
                    },
                    "files", transactionType
                },
                new StringContent(transactionType)
                {
                    Headers =
                    {
                        ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "transactionTypeKey"
                        }
                    }
                },
            }
        };
        _logger.LogInformation("Sending request to [{Url}] for transaction type [{TransactionType}]", url,
            transactionType);
        var response = await httpClient.SendAsync(request);
        try
        {
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("    Sent !");
        }
        catch (Exception e)
        {
            _logger.LogError("{Error}", e.ToString());
        }
    }

    private async Task<(string filename, string content)> GetTemplate(string url, string token)
    {
        _logger.LogInformation("Fetching [{Url}]", url);
        using var httpClient = new HttpClient();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            Headers = { { "PRIVATE-TOKEN", token } },
            RequestUri = new Uri(url)
        };

        try
        {
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Successfully fetched [{Source}]", url);
            var result = JsonConvert.DeserializeObject<GitFileResponse>(data);
            return (
                result.FileName,
                System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(result.Content))
            );
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to fetch a transaction templates from [{Source}]", url);
            _logger.LogError("{Error}", e.ToString());
            throw;
        }
    }

    private record GitFileResponse
    {
        [JsonProperty("file_name")] public string FileName { get; set; }
        [JsonProperty("content")] public string Content { get; set; }
    }
}