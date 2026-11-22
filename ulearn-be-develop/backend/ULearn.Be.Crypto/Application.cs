using CommandLine;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ULearn.Be.Crypto.Helpers;
using ULearn.Be.Crypto.Options;

namespace ULearn.Be.Crypto;

public class Application
{
    private readonly EncryptionHelper _encryptProcessor;
    private readonly ILogger<Application> _logger;

    public Application(
        ILogger<Application> logger,
        EncryptionHelper encryptProcessor
    )
    {
        _logger = logger;
        _encryptProcessor = encryptProcessor;
    }

    public void Run(IEnumerable<string> args)
    {
        try
        {
            Parser.Default.ParseArguments<DecryptOptions, EncryptOptions, HashOptions, TokenOptions>(args)
                .WithParsed<DecryptOptions>(_encryptProcessor.Decrypt)
                .WithParsed<EncryptOptions>(_encryptProcessor.Encrypt)
                .WithParsed<HashOptions>(HashHelper.Hash)
                .WithNotParsed(o => Environment.Exit(1));
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            Environment.Exit(1);
        }

        Environment.Exit(0);
    }
}