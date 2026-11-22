using Microsoft.Extensions.Logging;
using Npgsql;
using ULearn.Db.Tools.Models;
using ULearn.Db.Tools.Models.Options;
using ULearn.Db.Tools.Utils;

namespace ULearn.Db.Tools.Handlers;

public class DropDatabaseHandler
{
    private const string DefaultDbName = "template1";
    
    private readonly ILogger<DropDatabaseHandler> _logger;

    public DropDatabaseHandler(ILogger<DropDatabaseHandler> logger)
    {
        _logger = logger;
    }

    public async Task Run(DropDatabaseOptions obj)
    {
        _logger.LogInformation("Closing connections");
        await using var dataSource = NpgsqlDataSource.Create(
            ConnectionStringUtil.GetConnectionString(obj)
        );
        {
            await using var closeCommand = dataSource.CreateCommand(GetStopProcessedQuery(obj.DatabaseName));
            {
                await closeCommand.ExecuteNonQueryAsync();
                _logger.LogInformation("Closed all of the opened DB connections");
            }
            await dataSource.DisposeAsync();
        }

        await using var dataSource2 = NpgsqlDataSource.Create(
            ConnectionStringUtil.GetConnectionString(obj.ServerName, obj.Port, DefaultDbName, obj.UserName, obj.Password)
        );
        {
            await using var dropCommand = dataSource2.CreateCommand($"DROP DATABASE {obj.DatabaseName}");
            {
                await dropCommand.ExecuteNonQueryAsync();
                _logger.LogInformation("Drop local database");
            }    
        }
    }

    private static string GetStopProcessedQuery(string dbName)
    {
        return @$"SELECT 
    pg_terminate_backend(pid) 
FROM 
    pg_stat_activity 
WHERE 
    -- don't kill my own connection!
    pid <> pg_backend_pid()
    -- don't kill the connections to other databases
    AND datname = '{dbName}'
    ";
    }
}

