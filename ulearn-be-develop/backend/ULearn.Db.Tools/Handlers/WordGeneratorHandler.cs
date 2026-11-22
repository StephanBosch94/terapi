using DnsClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ULearn.Db.Tools.Models.Options;

namespace ULearn.Db.Tools.Handlers;

public class WordGeneratorHandler
{
    private readonly ILogger<WordGeneratorHandler> _logger;

    public WordGeneratorHandler(ILogger<WordGeneratorHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task Run(WordGeneratorOptions obj)
    {
        var words = await File.ReadAllLinesAsync(obj.PathToWordsFile);
        var suffixes = await File.ReadAllLinesAsync(obj.PathToSuffixesFile);
        _logger.LogInformation("Input texts are loaded");
        
        var results = new List<string>();
        foreach (var word in words)
        {
            foreach (var suffix in suffixes)
            {
                var res = $"{word}{suffix}.com";
                results.Add(res);
            }
        }
        
        await File.WriteAllLinesAsync(obj.OutputPath, results);
        _logger.LogInformation("File saved to [{ObjOutputPath}]", obj.OutputPath);
    }
}