using DnsClient;
using Microsoft.Extensions.Logging;
using ULearn.Db.Tools.Models.Options;

namespace ULearn.Db.Tools.Handlers;

public class DomainNameCheckHandler
{
    private readonly ILogger<DomainNameCheckHandler> _logger;

    public DomainNameCheckHandler(ILogger<DomainNameCheckHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task Run(DomainNameCheckOptions obj)
    {
        _logger.LogInformation("Input domains are loaded from [{Path}]", obj.Source);
        
        var lookup = new LookupClient();
        var domains = await File.ReadAllLinesAsync(obj.Source);
        var res = new List<string>();
        var count = domains.Length;
        foreach (var (domain, idx) in domains.Select((x, idx) => (x, idx)))
        {
            var isAvailable = !(await lookup.QueryAsync(domain, QueryType.NS)).Answers.Any();
            var availableSign = isAvailable ? "✓" : "x";
            if (isAvailable)
            {
                res.Add(domain);
            }
            _logger.LogInformation("Processed {Progress}% - [{Domain}] {AvailableSign}", GetProgress(idx, count), domain, availableSign);
        }
        
        _logger.LogInformation("---Available Domains----");
        foreach (var re in res)
        {
            _logger.LogInformation("{Res}", re);
        }
        
        await File.WriteAllLinesAsync(obj.Output, res);
        _logger.LogInformation("File saved to [{ObjOutputPath}]", obj.Output);
    }

    private static int GetProgress(int idx, int count)
    {
        return (idx + 1) * 100 / count;
    }
}