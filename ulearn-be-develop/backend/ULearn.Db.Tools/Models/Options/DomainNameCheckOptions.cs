using CommandLine;

namespace ULearn.Db.Tools.Models.Options;

[Verb(
    "domain-name-check",
    HelpText = @"Word generator"
)]
public class DomainNameCheckOptions
{
    [Option(
        's',
        "source",
    Required = true,
    HelpText = "Source file with domains"
    )]
    public string Source { get; set; }
    
    [Option(
        'o',
        "output",
        Required = true,
        HelpText = "Output file with domains"
    )]
    public string Output { get; set; }
}