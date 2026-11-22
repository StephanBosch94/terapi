using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using ULearn.Db.Tools.Models.Options;

namespace ULearn.Db.Tools.Handlers;

public class DeleteAccountHandler
{
    private readonly ILogger<DeleteAccountHandler> _logger;

    public DeleteAccountHandler(ILogger<DeleteAccountHandler> logger)
    {
        _logger = logger;
    }

    public async Task Run(DeleteAccountOptions obj)
    {
        using var httpClient = new HttpClient();
        var url = GetUrl(obj);
        var emails = obj.Emails.ToList();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", obj.BearerToken) },
            RequestUri = new Uri(url),
            Content = JsonContent.Create(new AdminBatchDeleteIm
            {
                Emails = emails
            })
        };

        try
        {
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to hard delete users. Exception: {Exception}", e.Message);
            throw;
        }
        
        foreach (var objEmail in emails)
        {
            _logger.LogInformation("DELETE [{Api}]: [{User}]", url, objEmail);
        }
    }

    private static string GetUrl(DeleteAccountOptions obj)
    {
        var baseUrl = obj.BeApiUrl.LastOrDefault() == '/'
            ? obj.BeApiUrl
            : obj.BeApiUrl + '/';

        return baseUrl + "api/user/batch";
    }
    
    private class AdminBatchDeleteIm
    {
        public List<string> Emails { get; set; }
    }
}