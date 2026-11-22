using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ULearn.Be.Crypto.Models;
using ULearn.Be.Crypto.Options;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models.Settings;

namespace ULearn.Be.Crypto.Helpers;

public class EncryptionHelper
{
    private readonly ICryptographyService _cryptographyService;
    private readonly ILogger<EncryptionHelper> _logger;
    private readonly MasterPasswordSettings _masterPasswordSettings;

    public EncryptionHelper(
        ICryptographyService cryptographyService,
        IOptions<MasterPasswordSettings> masterPasswordSettings,
        ILogger<EncryptionHelper> logger
    )
    {
        _cryptographyService = cryptographyService;
        _masterPasswordSettings = masterPasswordSettings.Value;
        _logger = logger;
    }

    public void Encrypt(EncryptOptions options)
    {
        if (options.FileName != null)
        {
            var masterPassword = options.MasterPassword ?? _masterPasswordSettings.MasterPassword;
            FromJsonFile(options.FileName, masterPassword);
            return;
        }

        foreach (var secret in options.PlaintextSecrets)
        {
            var obj = new CryptoObject { Configuration_Key = "From Args", Clear = secret };

            try
            {
                var masterPassword = options.MasterPassword ?? _masterPasswordSettings.MasterPassword;
                Process(obj, masterPassword);
            }
            catch (Exception e)
            {
                throw new Exception($"Cannot encrypt value [{obj.Configuration_Key}]: {e.Message}");
            }

            Console.WriteLine(obj.Encrypted);
        }
    }

    public void Decrypt(DecryptOptions options)
    {
        foreach (var secret in options.EncryptedSecrets)
        {
            var obj = new CryptoObject { Configuration_Key = "From Args", Encrypted = secret };

            try
            {
                var masterPassword = options.MasterPassword ?? _masterPasswordSettings.MasterPassword;
                Process(obj, masterPassword);
            }
            catch (Exception e)
            {
                throw new Exception($"Cannot decrypt value [{obj.Configuration_Key}]: {e.Message}");
            }

            Console.WriteLine(obj.Clear);
        }
    }

    private void FromJsonFile(string filename, string masterPassword)
    {
        List<CryptoObject> cryptoObjects;

        try
        {
            var fileData = System.IO.File.ReadAllText(filename);
            cryptoObjects = JsonConvert.DeserializeObject<List<CryptoObject>>(fileData);
        }
        catch (Exception e)
        {
            throw new Exception($"Cannot read file [{filename}]: {e.Message}");
        }

        foreach (var cryptoObject in cryptoObjects)
        {
            try
            {
                Process(cryptoObject, masterPassword);

                cryptoObject.SqlUpdate =
                    $"UPDATE [dbo].[Env_Configuration] SET [Value_Origin] = N'{Path.GetFileName(filename)}', [Configuration_Value] = N'{cryptoObject.Encrypted}' WHERE [Configuration_Key] = N'{cryptoObject.Configuration_Key}'";
                cryptoObject.SqlProcedureExec =
                    $"EXEC [dbo].[p_ENV_Configure] @Key = N'{cryptoObject.Configuration_Key}', @Value = N'{cryptoObject.Encrypted}', @ValueOrigin = '{Path.GetFileName(filename)}';";
            }
            catch (Exception e)
            {
                throw new Exception($"Cannot process value [{cryptoObject.Configuration_Key}]: {e.Message}");
            }
        }

        try
        {
            var fileDataOut = JsonConvert.SerializeObject(cryptoObjects, Formatting.Indented);
            System.IO.File.WriteAllText(filename + ".out", fileDataOut);

            var sqlFileDataOut =
                string.Join(Environment.NewLine, cryptoObjects.Select(o => o.SqlProcedureExec));
            System.IO.File.WriteAllText(filename + ".sql", sqlFileDataOut);
        }
        catch (Exception e)
        {
            throw new Exception($"Cannot write output to file [{filename}.out/sql]: {e.Message}");
        }

        _logger.LogInformation($"File [{filename}] processed into [{filename}.out] and [{filename}.sql]");
    }


    private void Process(CryptoObject obj, string masterPassword)
    {
        if (string.IsNullOrEmpty(obj.Encrypted) && !string.IsNullOrEmpty(obj.Clear))
        {
            obj.Encrypted = _cryptographyService.Encrypt(obj.Clear, masterPassword);
        }

        if (!string.IsNullOrEmpty(obj.Encrypted) && string.IsNullOrEmpty(obj.Clear))
        {
            obj.Clear = _cryptographyService.Decrypt(obj.Encrypted, masterPassword);
        }
    }
}