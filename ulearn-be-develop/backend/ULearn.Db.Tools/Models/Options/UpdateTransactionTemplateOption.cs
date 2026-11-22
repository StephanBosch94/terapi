using CommandLine;

namespace ULearn.Db.Tools.Models.Options;

[Verb(
    "upload-transaction-template",
    HelpText = @"Uploads a transaction template in PDF format"
)]
public class UpdateTransactionTemplateOption
{
    [Option(
        's',
        "sourceUrl",
        Required = true,
        HelpText = "URL to the folder with a transaction template"
    )]
    public string Url { get; set; }

    [Option(
        'l',
        "styles",
        Required = true,
        HelpText = "Styles"
    )]
    public string StylesUrl { get; set; }

    [Option(
        'c',
        "sourceToken",
        Required = true,
        HelpText = "Source token"
    )]
    public string SourceToken { get; set; }

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