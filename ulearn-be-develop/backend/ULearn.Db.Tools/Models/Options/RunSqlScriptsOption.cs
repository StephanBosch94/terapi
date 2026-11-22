using CommandLine;

namespace ULearn.Db.Tools.Models.Options;

[Verb(
    "run-scripts",
    HelpText = @"Runs all SQL scripts in the specified folder"
)]
public class RunSqlScriptsOption: ConnectionStringBase
{
    [Option(
        'f',
        "folder",
        Required = true,
        HelpText = "Path to the folder with the sql scripts"
    )]
    public string PathToFolder { get; set; }
}