using Microsoft.Extensions.Logging;
using Npgsql;
using ULearn.Db.Tools.Models;
using ULearn.Db.Tools.Models.Options;
using ULearn.Db.Tools.Utils;

namespace ULearn.Db.Tools.Handlers;

public class RunSqlScriptsHandler
{
    private readonly ILogger<RunSqlScriptsHandler> _logger;
    private const string CurrentDirectoryPlaceholder = "{{CURRENT_DIR}}";

    public RunSqlScriptsHandler(ILogger<RunSqlScriptsHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task Run(RunSqlScriptsOption obj)
    {
        await using var dataSource = NpgsqlDataSource.Create(ConnectionStringUtil.GetConnectionString(obj));
        
        foreach (var file in Directory.EnumerateFiles(obj.PathToFolder, "*.sql"))
        {
            var contents = await File.ReadAllTextAsync(file);
            var replacedContent = contents.Replace(CurrentDirectoryPlaceholder, Directory.GetCurrentDirectory());
            try
            {
                await using var closeCommand = dataSource.CreateCommand(replacedContent);
                {
                    await closeCommand.ExecuteNonQueryAsync();
                }
            }
            catch (Exception)
            {
                _logger.LogError("Failed to apply '{FileName}'", file);
                throw;
            }
            
            _logger.LogInformation("Applied '{FileName}'", file);
        }
    }
}