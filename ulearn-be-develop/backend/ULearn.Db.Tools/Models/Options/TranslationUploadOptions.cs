using CommandLine;

namespace ULearn.Db.Tools.Models.Options;

[Verb(
    "upload-translations",
    HelpText = @"Uploads translations from a JSON file"
)]
public class TranslationUploadOptions
{
    [Option(
        'f',
        "files",
        Required = true,
        HelpText = "Path to JSON files with translations"
    )]
    public string PathsToJson { get; set; }

    [Option(
        't',
        "token",
        Required = true,
        HelpText = "Bearer admin token to BE.API"
    )]
    public string BearerToken { get; set; }
    
    
    [Option(
        'a',
        "api",
        Required = true,
        HelpText = "BE.API URL"
    )]
    public string BeApiUrl { get; set; }
}