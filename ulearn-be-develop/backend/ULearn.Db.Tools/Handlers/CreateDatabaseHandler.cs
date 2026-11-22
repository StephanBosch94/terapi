using Microsoft.Extensions.Logging;
using Npgsql;
using ULearn.Db.Tools.Models;
using ULearn.Db.Tools.Models.Options;
using ULearn.Db.Tools.Utils;

namespace ULearn.Db.Tools.Handlers;

public class CreateDatabaseHandler
{
    private const string DefaultDbName = "template1";
    
    private readonly ILogger<CreateDatabaseHandler> _logger;

    public CreateDatabaseHandler(ILogger<CreateDatabaseHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task Run(CreateDatabaseOptions obj)
    {
        await using var dataSource = NpgsqlDataSource.Create(
            ConnectionStringUtil.GetConnectionString(obj.ServerName, obj.Port, DefaultDbName, obj.UserName, obj.Password)
        );
        {
            await using var closeCommand = dataSource.CreateCommand($"CREATE DATABASE {obj.DatabaseName} OWNER {obj.UserName}");
            {
                await closeCommand.ExecuteNonQueryAsync();
                _logger.LogInformation("Created database '{DB}' with owner '{Owner}'", obj.DatabaseName, obj.UserName);
            }
        }
    }
}