using CommandLine;

namespace ULearn.Db.Tools.Models.Options;

[Verb(
    "generate-words",
    HelpText = @"Word generator"
)]
public class WordGeneratorOptions
{
    [Option(
        'w',
        "words",
        Required = true,
        HelpText = "Path to the folder with profile photos where each photo is labeled with email"
    )]
    public string PathToWordsFile { get; set; }
    
    [Option(
        's',
        "suffixes",
        Required = true,
        HelpText = "Path to the folder with profile photos where each photo is labeled with email"
    )]
    public string PathToSuffixesFile { get; set; }
    
    [Option(
        'o',
        "output",
        Required = true,
        HelpText = "Path to the folder with profile photos where each photo is labeled with email"
    )]
    public string OutputPath { get; set; }
}