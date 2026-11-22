using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ULearn.Db.Tools.Models;
using ULearn.Db.Tools.Models.Options;
using ULearn.Db.Tools.Utils;

namespace ULearn.Db.Tools.Handlers;

public class TranslationUploaderHandler
{
    private readonly ILogger<TranslationUploaderHandler> _logger;

    public TranslationUploaderHandler(ILogger<TranslationUploaderHandler> logger)
    {
        _logger = logger;
    }

    public async Task Run(TranslationUploadOptions obj)
    {
        var translations = Directory.EnumerateFiles(obj.PathsToJson, "*.json")
            .SelectMany(Parse);
        
        try
        {
            var response = await TranslationUploaderUtil.Upload(obj.BeApiUrl, obj.BearerToken, translations);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            _logger.LogError("{Error}", e.ToString());
        }
        _logger.LogInformation("Translations are uploaded");
    }
    
    private IEnumerable<TranslationVM> Parse(string pathToJson)
    {
        _logger.LogInformation("Reading and parsing \'{PathToJson}\'", pathToJson);
        return JsonConvert.DeserializeObject<List<TranslationVM>>(
            File.ReadAllText(pathToJson)
        );
    }
}