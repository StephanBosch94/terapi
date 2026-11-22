using CommandLine;
using Microsoft.Extensions.Logging;
using ULearn.Db.Tools.Handlers;
using ULearn.Db.Tools.Models.Options;

namespace ULearn.Db.Tools;

public class Application
{
    private readonly CreateDatabaseHandler _createDatabaseHandler;
    private readonly DomainNameCheckHandler _domainNameCheckHandler;
    private readonly DropDatabaseHandler _dropDatabaseHandler;
    private readonly ILogger<Application> _logger;
    private readonly ResetDatabaseHandler _resetDatabaseHandler;
    private readonly RunSqlScriptsHandler _runSqlScriptsHandler;
    private readonly TranslationUploaderHandler _translationUploaderHandler;
    private readonly TsvTranslationUploadHandler _tsvTranslationUploadHandler;
    private readonly UpdateTransactionTemplateHandler _updateTransactionTemplateHandler;
    private readonly UploadProfilePhotosHandler _uploadProfilePhotosHandler;
    private readonly WordGeneratorHandler _wordGeneratorHandler;
    private readonly DeleteAccountHandler _deleteAccountHandler;

    public Application(
        ILogger<Application> logger,
        DropDatabaseHandler dropDatabaseHandler,
        ResetDatabaseHandler resetDatabaseHandler,
        CreateDatabaseHandler createDatabaseHandler,
        RunSqlScriptsHandler runSqlScriptsHandler,
        TranslationUploaderHandler translationUploaderHandler,
        UploadProfilePhotosHandler uploadProfilePhotosHandler,
        WordGeneratorHandler wordGeneratorHandler,
        DomainNameCheckHandler domainNameCheckHandler,
        TsvTranslationUploadHandler tsvTranslationUploadHandler,
        UpdateTransactionTemplateHandler updateTransactionTemplateHandler, 
        DeleteAccountHandler deleteAccountHandler
    )
    {
        _logger = logger;
        _dropDatabaseHandler = dropDatabaseHandler;
        _resetDatabaseHandler = resetDatabaseHandler;
        _createDatabaseHandler = createDatabaseHandler;
        _runSqlScriptsHandler = runSqlScriptsHandler;
        _translationUploaderHandler = translationUploaderHandler;
        _uploadProfilePhotosHandler = uploadProfilePhotosHandler;
        _wordGeneratorHandler = wordGeneratorHandler;
        _domainNameCheckHandler = domainNameCheckHandler;
        _updateTransactionTemplateHandler = updateTransactionTemplateHandler;
        _deleteAccountHandler = deleteAccountHandler;
        _tsvTranslationUploadHandler = tsvTranslationUploadHandler;
    }

    public void Run(IEnumerable<string> args)
    {
        try
        {
            Parser.Default
                .ParseArguments<
                    ResetDatabaseOptions,
                    DropDatabaseOptions,
                    CreateDatabaseOptions,
                    RunSqlScriptsOption,
                    TranslationUploadOptions,
                    UploadProfilePhotosOptions,
                    WordGeneratorOptions,
                    DomainNameCheckOptions,
                    TsvTranslationUploadOptions,
                    UpdateTransactionTemplateOption,
                    DeleteAccountOptions
                >(args)
                .WithParsed<ResetDatabaseOptions>(x => _resetDatabaseHandler.Run(x).GetAwaiter().GetResult())
                .WithParsed<DropDatabaseOptions>(x => _dropDatabaseHandler.Run(x).GetAwaiter().GetResult())
                .WithParsed<CreateDatabaseOptions>(x => _createDatabaseHandler.Run(x).GetAwaiter().GetResult())
                .WithParsed<RunSqlScriptsOption>(x => _runSqlScriptsHandler.Run(x).GetAwaiter().GetResult())
                .WithParsed<TranslationUploadOptions>(x => _translationUploaderHandler.Run(x).GetAwaiter().GetResult())
                .WithParsed<UploadProfilePhotosOptions>(
                    x => _uploadProfilePhotosHandler.Run(x).GetAwaiter().GetResult())
                .WithParsed<WordGeneratorOptions>(x => _wordGeneratorHandler.Run(x).GetAwaiter().GetResult())
                .WithParsed<DomainNameCheckOptions>(x => _domainNameCheckHandler.Run(x).GetAwaiter().GetResult())
                .WithParsed<DeleteAccountOptions>(x => _deleteAccountHandler.Run(x).GetAwaiter().GetResult())
                .WithParsed<TsvTranslationUploadOptions>(x =>
                    _tsvTranslationUploadHandler.Run(x).GetAwaiter().GetResult())
                .WithParsed<UpdateTransactionTemplateOption>(x =>
                    _updateTransactionTemplateHandler.Run(x).GetAwaiter().GetResult())
                .WithNotParsed(_ => Environment.Exit(1));
        }
        catch (Exception e)
        {
            _logger.LogError("{Message}", e.Message);
            Environment.Exit(1);
        }

        Environment.Exit(0);
    }
}