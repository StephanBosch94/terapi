using CommandLine;

namespace ULearn.Db.Tools.Models.Options;

[Verb(
    "reset-database",
    HelpText = @"Resets a database"
)]
public class ResetDatabaseOptions: ConnectionStringBase
{
    
}