using System.Globalization;
using CsvHelper;
using Microsoft.Extensions.Logging;
using ULearn.Db.Tools.Models;
using ULearn.Db.Tools.Models.Options;
using ULearn.Db.Tools.Utils;

namespace ULearn.Db.Tools.Handlers;

public class TsvTranslationUploadHandler
{
    private readonly ILogger<TsvTranslationUploadHandler> _logger;

    public TsvTranslationUploadHandler(ILogger<TsvTranslationUploadHandler> logger)
    {
        _logger = logger;
    }

    public async Task Run(TsvTranslationUploadOptions obj)
    {
        try
        {
            var rawTranslations = await GetRawTranslations(obj.Url);
            var translations = Parse(rawTranslations);
            var response = await TranslationUploaderUtil.Upload(obj.BeApiUrl, obj.BearerToken, translations);
            try
            {
                response.EnsureSuccessStatusCode();
                _logger.LogInformation("[{NumberOfTranslations}] translations are uploaded", translations.Count);
            }
            catch (Exception)
            {
                _logger.LogError("BE error response: [{Response}]", await response!.Content!.ReadAsStringAsync());
            }
        }
        catch (Exception e)
        {
            _logger.LogInformation("Failed to upload translations");
            _logger.LogError("{Error}", e.ToString());
        }
    }

    private async Task<string> GetRawTranslations(string url)
    {
        _logger.LogInformation("Fetching translations from [{Url}]", url);
        using var httpClient = new HttpClient();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url)
        };

        try
        {
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Successfully fetched translations from [{Source}]", url);
            return data;
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to fetch translations from [{Source}]", url);
            _logger.LogError("{Error}", e.ToString());
            throw;
        }
    }

    private IReadOnlyCollection<TranslationVM> Parse(string translations)
    {
        _logger.LogInformation("Parsing tsv data");
        var badRecords = new List<string>();
        var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = "\t",
            BadDataFound = context => badRecords.Add($"Invalid value [{context.Field}] in [{context.RawRecord}]"),
            Mode = CsvMode.NoEscape
        };
        using var reader = new StringReader(translations);
        using var csvReader = new CsvReader(reader, config);
        var records = csvReader.GetRecords<TranslationsFlatVM>().ToList();
        _logger.LogInformation("Successfully parsed [{TranslationsNumber}] with [{WarningsNumber}] warnings",
            records.Count, badRecords.Count);
        if (badRecords.Any())
        {
            _logger.LogInformation("Found bad data");
            foreach (var badRecord in badRecords)
            {
                _logger.LogWarning("{Data}", badRecord);
            }
        }

        return records
            .Where(x => !string.IsNullOrEmpty(x.TranslationKey))
            .Select(x => new TranslationVM
            {
                TranslationKey = x.TranslationKey,
                Values = new List<TranslationValueVM>
                {
                    new()
                    {
                        LanguageCode = "en",
                        Value = x.EnglishTranslation
                    },
                    new()
                    {
                        LanguageCode = "cs",
                        Value = x.CzechTranslation
                    },
                    new()
                    {
                        LanguageCode = "ru",
                        Value = x.RussianTranslation
                    },
                    new()
                    {
                        LanguageCode = "uk",
                        Value = x.UkrainianTranslation
                    },
                    new()
                    {
                        LanguageCode = "sr",
                        Value = x.SerbianTranslation
                    }
                }.Where(y => !string.IsNullOrEmpty(y.Value)).ToList()
            }).ToList();
    }
}