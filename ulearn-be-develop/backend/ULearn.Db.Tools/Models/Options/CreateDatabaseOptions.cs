using CommandLine;

namespace ULearn.Db.Tools.Models.Options;

[Verb(
    "create-database",
    HelpText = @"Creates a database"
)]
public class CreateDatabaseOptions: ConnectionStringBase
{
    
}