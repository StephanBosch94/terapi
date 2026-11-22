using ULearn.Be.Common.Extensions;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Utils;

public static class TranslationUtil
{
    private const string ZeroSuffix = "_zero";
    private const string OneSuffix = "_one";
    private const string FewSuffix = "_few";
    private const string ManySuffix = "_many";
    public const string CountParameter = "count";

    public static string GetLanguageCode(int? translationLanguageId)
    {
        var translationEnum = translationLanguageId.HasValue
            ? (CTranslationLanguageEnum)translationLanguageId.Value
            : CTranslationLanguageEnum.English;

        return translationEnum.GetEnumKey();
    }

    public static string Translate(
        IReadOnlyDictionary<string, string> translations,
        string translationKey,
        IReadOnlyDictionary<string, string> parameters = null
    )
    {
        var key = GetKeyBasedOnParams(translationKey, parameters);
        translations.TryGetValue(key, out var keyValue);
        translations.TryGetValue(translationKey, out var translationKeyValue);
        var value = keyValue ?? translationKeyValue ?? translationKey;
        return InsertParameters(value, parameters);
    }

    private static string GetKeyBasedOnParams(
        string translationKey,
        IReadOnlyDictionary<string, string> parameters
    )
    {
        if (parameters == null || !parameters.ContainsKey(CountParameter))
        {
            return translationKey;
        }

        if (!int.TryParse(parameters[CountParameter], out var count))
        {
            return translationKey;
        }

        return count switch
        {
            0 => $"{translationKey}{ZeroSuffix}",
            1 => $"{translationKey}{OneSuffix}",
            > 1 and < 5 => $"{translationKey}{FewSuffix}",
            >= 5 => $"{translationKey}{ManySuffix}",
            _ => translationKey
        };
    }

    private static string InsertParameters(string value, IReadOnlyDictionary<string, string> parameters)
    {
        if (string.IsNullOrEmpty(value) || parameters == null)
        {
            return value;
        }

        var result = $"{value}";
        foreach (var (param, paramValue) in parameters)
        {
            result = result.Replace(GetPlaceholder(param), paramValue);
        }

        return result;
    }

    private static string GetPlaceholder(string parameter)
    {
        return "{{" + parameter + "}}";
    }
}