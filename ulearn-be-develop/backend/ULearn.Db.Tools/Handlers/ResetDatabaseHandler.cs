using Microsoft.Extensions.Logging;
using Npgsql;
using ULearn.Db.Tools.Models.Options;
using ULearn.Db.Tools.Utils;

namespace ULearn.Db.Tools.Handlers;

public class ResetDatabaseHandler
{
    private readonly ILogger<ResetDatabaseHandler> _logger;

    public ResetDatabaseHandler(ILogger<ResetDatabaseHandler> logger)
    {
        _logger = logger;
    }

    public async Task Run(ResetDatabaseOptions obj)
    {
        var schemas = new List<string>{
            "public",
            "sf"
        };
        await using var dataSource = NpgsqlDataSource.Create(ConnectionStringUtil.GetConnectionString(obj));
        {
            foreach (var schema in schemas)
            {
                await using var closeCommand = dataSource.CreateCommand(GetResetDbQuery(obj.UserName, schema));
                {
                    await closeCommand.ExecuteNonQueryAsync();
                }   
            }
            await dataSource.DisposeAsync();
        }
        _logger.LogInformation("DB [{DatabaseName}] was successfully reset", obj.DatabaseName);
    }

    private static string GetResetDbQuery(string userName, string schema)
    {
        return @$"
DROP SCHEMA {schema} CASCADE;
CREATE SCHEMA {schema};
GRANT ALL ON SCHEMA {schema} TO {userName};
    ";
    }
}