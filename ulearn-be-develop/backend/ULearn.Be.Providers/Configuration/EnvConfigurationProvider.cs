using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ULearn.Be.Common.Attributes;
using ULearn.Be.Common.Exceptions;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Providers.Configuration.Context;
using ULearn.Be.Providers.Configuration.Enums;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models;
using ULearn.Be.Providers.Configuration.Models.Settings;

namespace ULearn.Be.Providers.Configuration;

public class EnvConfigurationProvider : IEnvConfigurationProvider
{
    public static string PASSWORD_REPLACE_TOKEN = "<password>";

    private static MasterPasswordSettings _masterPasswordSettings;
    private static readonly string SETTINGS_MODEL_NAMESPACE = "ULearn.Be.Providers.Configuration.Models.Settings.";
    private readonly IConfiguration _configuration;
    private readonly IEnvCfgRepositoryContextFactory _contextFactory;

    private readonly ICryptographyService _cryptographyService;
    private Dictionary<string, EnvConfiguration> _envCfgKeys;

    private Dictionary<EnvSettingsKeyEnum, object> _envCfgSettings;

    public EnvConfigurationProvider(
        IEnvCfgRepositoryContextFactory contextFactory,
        ICryptographyService cryptographyService,
        string masterPassword,
        IConfiguration configuration = null
    )
    {
        _contextFactory = contextFactory;
        _masterPasswordSettings = new MasterPasswordSettings { MasterPassword = masterPassword };
        _cryptographyService = cryptographyService;
        _configuration = configuration;

        if (string.IsNullOrEmpty(_masterPasswordSettings.MasterPassword))
        {
            throw new NotFoundException("MasterPassword not defined in configuration file");
        }
    }

    public async Task<Dictionary<string, object>> GetConfigurationAllFe()
    {
        return (await GetConfiguration())
            .ToDictionary(
                o => o.ConfigurationKey,
                ConvertEnvConfiguration
            );
    }

    public void ReloadEnvSettings()
    {
        _envCfgSettings = new Dictionary<EnvSettingsKeyEnum, object>();

        using var context = _contextFactory.Get();
        var allEnvKeys = Query(context).Where(o => !o.ConfigurationKey.Equals("obsolete")).ToList();

        foreach (EnvSettingsKeyEnum cfgKey in Enum.GetValues(typeof(EnvSettingsKeyEnum)))
        {
            var envKeys = allEnvKeys
                .Where(o => o.ConfigurationKey.StartsWith(cfgKey.GetDescription()))
                .ToList();

            var settingInstance = CreateInstanceSettings(envKeys, cfgKey);

            /* Save Settings */
            _envCfgSettings.Add(cfgKey, settingInstance);
        }

        _envCfgKeys = allEnvKeys.ToDictionary(o => o.ConfigurationKey);
    }

    /* Get settings from DB */
    public T GetSettings<T>(EnvSettingsKeyEnum key, bool throwNotFoundException = true) where T : class
    {
        if (_envCfgSettings == null)
        {
            ReloadEnvSettings();
        }

        if (_envCfgSettings.TryGetValue(key, out var result))
        {
            if (result == null)
            {
                throw new NotFoundException(
                    $"Cannot find {(key.IsOptionalSettings() ? "Optional " : "")}ENV Configuration Settings [{key.GetDescription()}] in DB"
                );
            }

            return (T)result;
        }

        if (throwNotFoundException)
        {
            throw new NotFoundException($"Cannot find ENV Configuration Settings [{key.GetDescription()}] in DB");
        }

        return null;
    }

    public string EncryptPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return "";
        }

        try
        {
            return _cryptographyService.Encrypt(password, _masterPasswordSettings.MasterPassword);
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    public string DecryptPassword(string encryptedPassword)
    {
        if (string.IsNullOrEmpty(encryptedPassword))
        {
            return "";
        }

        try
        {
            return _cryptographyService.Decrypt(encryptedPassword, _masterPasswordSettings.MasterPassword);
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }


    private object CreateInstanceSettings(
        List<EnvConfiguration> envKeys,
        EnvSettingsKeyEnum cfgKey,
        bool throwNotFoundException = true
    )
    {
        var settingsName = cfgKey.GetSettingsObjectName();
        var settingsType = Type.GetType(SETTINGS_MODEL_NAMESPACE + settingsName);

        var dbSettings = GetSettingsFromDb(settingsName, settingsType, envKeys, cfgKey);

        /* Read configuration from file wih highest priority */
        var fileSettings = GetSettingsFromFile(settingsName, settingsType);

        /* Settings in DB is not set return fileSettings or null or exception */
        if (dbSettings == null)
        {
            if (fileSettings != null)
            {
                return fileSettings;
            }

            if (cfgKey.IsOptionalSettings())
            {
                return null;
            }

            if (throwNotFoundException)
            {
                throw new NotFoundException($"Keys with ConfigurationKey [{cfgKey.GetDescription()}*] not found in DB");
            }

            return null;
        }

        /* Override DB settings by File settings */
        if (fileSettings != null)
        {
            MergeSettings(dbSettings, fileSettings, settingsType);
        }

        return dbSettings;
    }

    private object GetSettingsFromDb(
        string settingsName,
        Type settingsType,
        List<EnvConfiguration> envKeys,
        EnvSettingsKeyEnum cfgKey
    )
    {
        var paramList = new List<object>();

        if (envKeys.Count == 0)
        {
            return null;
        }

        foreach (var property in settingsType.GetProperties())
        {
            var propName = property.Name;
            var propertyFoundInDbCfg = false;

            foreach (var envKey in envKeys.OrderBy(o => o.ConfigurationKey))
            {
                var cfgKeyName = "";
                try
                {
                    cfgKeyName =
                        envKey.ConfigurationKey.Remove(
                            0,
                            cfgKey.GetDescription().Length + 1
                        ); /* +1 includes ending '.' */

                    if (cfgKeyName.Contains('.'))
                    {
                        /* Cannot be used logger as this class could be used also for its configuration */
                        //throw new Exception("Composed object");
                        continue;
                    }
                }
                catch (Exception)
                {
                    ;
                }

                if (propName.ToLower().Equals(cfgKeyName.ToLower()))
                {
                    var value = ConvertEnvConfiguration(envKey);

                    var propObject = new PropertyObject
                    {
                        DataTypeEnum = (EnvConfigurationDataTypeEnum)envKey.DataTypeId, Property = property,
                        Value = value,
                        OriginalValue = envKey.ConfigurationValue
                    };
                    paramList.Add(propObject);

                    propertyFoundInDbCfg = true;

                    break;
                }
            }

            if (propertyFoundInDbCfg == false)
            {
                Console.WriteLine(
                    $"    Setting property [{settingsName}.{propName}] not found in DB ENV_Configuration by key [{cfgKey.GetDescription()}.{propName}]"
                );
            }
        }

        /* Create Settings and populate variables */
        var settingInstance = Activator.CreateInstance(settingsType);
        foreach (PropertyObject propObject in paramList)
        {
            if (!string.IsNullOrEmpty(propObject.OriginalValue)
                && (propObject.DataTypeEnum == EnvConfigurationDataTypeEnum.Object
                    || propObject.DataTypeEnum == EnvConfigurationDataTypeEnum.ObjectArray
                    || propObject.DataTypeEnum == EnvConfigurationDataTypeEnum.ObjectMap
                    || propObject.DataTypeEnum == EnvConfigurationDataTypeEnum.StringArray
                    || propObject.DataTypeEnum == EnvConfigurationDataTypeEnum.IntegerArray)
               )
            {
                propObject.Property.SetValue(
                    settingInstance,
                    JsonConvert.DeserializeObject(propObject.OriginalValue, propObject.Property.PropertyType)
                );
                continue;
            }

            propObject.Property.SetValue(settingInstance, propObject.Value);
        }

        return settingInstance;
    }

    private object GetSettingsFromFile(string settingsName, Type settingsType)
    {
        var settings = _configuration?.GetSection(settingsName).Get(settingsType);


        /* Decrypt values by MasterPassword if it is needed */
        if (settings != null)
        {
            foreach (var property in settingsType.GetProperties())
            {
                if (property.CustomAttributes.Any(o => o.AttributeType == typeof(EncryptedByMasterPasswordAttribute)))
                {
                    var encrypted = (string)property.GetValue(settings);
                    var decrypted = DecryptPassword(encrypted);

                    property.SetValue(settings, decrypted);
                }
            }
        }

        return settings;
    }

    private object ConvertEnvConfiguration(EnvConfiguration envCfg)
    {
        if (string.IsNullOrEmpty(envCfg?.ConfigurationValue))
        {
            return null;
        }

        try
        {
            if (envCfg.DataTypeId == (int)EnvConfigurationDataTypeEnum.Integer)
            {
                return JsonConvert.DeserializeObject<int>(envCfg.ConfigurationValue);
            }

            if (envCfg.DataTypeId == (int)EnvConfigurationDataTypeEnum.StringArray)
            {
                return JsonConvert.DeserializeObject<List<string>>(envCfg.ConfigurationValue);
            }

            if (envCfg.DataTypeId == (int)EnvConfigurationDataTypeEnum.IntegerArray)
            {
                return JsonConvert.DeserializeObject<List<int>>(envCfg.ConfigurationValue);
            }

            if (envCfg.DataTypeId == (int)EnvConfigurationDataTypeEnum.Object)
            {
                return JsonConvert.DeserializeObject<object>(envCfg.ConfigurationValue);
            }

            if (envCfg.DataTypeId == (int)EnvConfigurationDataTypeEnum.ObjectArray)
            {
                return JsonConvert.DeserializeObject<List<object>>(envCfg.ConfigurationValue);
            }

            if (envCfg.DataTypeId == (int)EnvConfigurationDataTypeEnum.Boolean)
            {
                return JsonConvert.DeserializeObject<bool>(envCfg.ConfigurationValue);
            }

            if (envCfg.DataTypeId == (int)EnvConfigurationDataTypeEnum.ObjectMap)
            {
                return JsonConvert.DeserializeObject<object>(envCfg.ConfigurationValue);
            }

            if (envCfg.DataTypeId == (int)EnvConfigurationDataTypeEnum.Password)
            {
                return DecryptPassword(envCfg.ConfigurationValue);
            }

            if (envCfg.DataTypeId == (int)EnvConfigurationDataTypeEnum.Long)
            {
                return JsonConvert.DeserializeObject<long>(envCfg.ConfigurationValue);
            }
        }
        catch (Exception e)
        {
            throw new Exception(
                $"Cannot convert EnvSettings [{envCfg.ConfigurationKey}] value to DataType [{envCfg.DataTypeId}]. Exception: {e}"
            );
        }

        return envCfg.ConfigurationValue;
    }

    private async Task<ICollection<EnvConfiguration>> GetConfiguration(bool backendOnly = false)
    {
        using (var context = _contextFactory.Get())
        {
            return await Query(context)
                .Where(x => x.BackendOnly == backendOnly)
                .ToListAsync();
        }
    }

    /* Copy all Settings properties from src to dst */
    /* So, settings in dst will be overwritten by settings in src */
    private void MergeSettings<T>(T dst, T src, Type type) where T : class
    {
        if (src != null && dst == null)
        {
            dst = src;
        }

        var properties = type
            .GetProperties()
            .Where(x => x.CanRead && x.CanWrite)
            .ToList();
        foreach (var sourceProp in properties)
        {
            if (properties.Any(x => x.Name == sourceProp.Name))
            {
                var p = properties.First(x => x.Name == sourceProp.Name);
                if (p.CanWrite)
                {
                    var sourceValue = sourceProp.GetValue(src, null);

                    /* Copy only populated values - not default values */
                    /* So, int value 0 will not be copied */
                    if ((sourceProp.PropertyType.IsValueType &&
                         !sourceValue.Equals(Activator.CreateInstance(sourceProp.PropertyType)))
                        || (!sourceProp.PropertyType.IsValueType &&
                            Convert.ChangeType(sourceValue, sourceProp.PropertyType) != null))
                    {
                        p.SetValue(dst, sourceValue, null);
                    }
                }
            }
        }
    }

    private IQueryable<EnvConfiguration> Query(
        EnvCfgRepositoryContext context,
        bool asNoTracking = true
    )
    {
        var query = context.EnvConfiguration;
        return asNoTracking ? query.AsNoTracking() : query;
    }
}