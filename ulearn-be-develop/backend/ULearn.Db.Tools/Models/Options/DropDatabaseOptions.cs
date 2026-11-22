using CommandLine;

namespace ULearn.Db.Tools.Models.Options;

[Verb(
    "drop-database",
    HelpText = @"Drops the database"
)]
public class DropDatabaseOptions: ConnectionStringBase
{
    
}