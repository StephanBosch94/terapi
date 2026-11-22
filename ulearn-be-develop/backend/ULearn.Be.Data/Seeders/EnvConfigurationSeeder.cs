using Newtonsoft.Json;
using ULearn.Be.Common.Extensions;
using ULearn.Be.Data.Enums;

namespace ULearn.Be.Data.Seeders;

public static class EnvConfigurationSeeder
{
    private const string ValueOrigin = "init_scripts";

    public static readonly List<EnvConfiguration> Entities = new()
    {
        new EnvConfiguration
        {
            ConfigurationId = 1,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.Password,
            ConfigurationKey = $"{EnvSettingsKeyEnum.AuthSettings.GetDescription()}.Secret",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 2,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.Long,
            ConfigurationKey = $"{EnvSettingsKeyEnum.AuthSettings.GetDescription()}.ExpirationMs",
            AdminConfigurable = false,
            BackendOnly = true,
            ConfigurationValue = "31104000000",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 3,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.AuthSettings.GetDescription()}.Issuer",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 4,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.GoogleApiSettings.GetDescription()}.ClientId",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 5,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.Password,
            ConfigurationKey = $"{EnvSettingsKeyEnum.GoogleApiSettings.GetDescription()}.ClientSecret",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 6,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.GoogleApiSettings.GetDescription()}.TutorRedirectUrl",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 7,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.GoogleApiSettings.GetDescription()}.OauthUrl",
            AdminConfigurable = false,
            BackendOnly = false,
            ConfigurationValue = "https://accounts.google.com/o/oauth2/v2/auth",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 8,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.StringArray,
            ConfigurationKey = $"{EnvSettingsKeyEnum.GoogleApiSettings.GetDescription()}.TutorScopes",
            AdminConfigurable = false,
            BackendOnly = false,
            ConfigurationValue = JsonConvert.SerializeObject(
                new List<string>
                {
                    "https://www.googleapis.com/auth/userinfo.profile",
                    "https://www.googleapis.com/auth/userinfo.email"
                }
            ),
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 9,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.StringArray,
            ConfigurationKey = $"{EnvSettingsKeyEnum.GoogleApiSettings.GetDescription()}.StudentScopes",
            AdminConfigurable = false,
            BackendOnly = false,
            ConfigurationValue = JsonConvert.SerializeObject(
                new List<string>
                {
                    "https://www.googleapis.com/auth/userinfo.profile",
                    "https://www.googleapis.com/auth/userinfo.email"
                }
            ),
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 10,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.SeznamApiSettings.GetDescription()}.ClientId",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 11,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.Password,
            ConfigurationKey = $"{EnvSettingsKeyEnum.SeznamApiSettings.GetDescription()}.ClientSecret",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 12,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.SeznamApiSettings.GetDescription()}.StudentRedirectUrl",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 13,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.SeznamApiSettings.GetDescription()}.OauthUrl",
            AdminConfigurable = false,
            BackendOnly = false,
            ConfigurationValue = "https://login.szn.cz/api/v1/oauth/auth",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 14,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.StringArray,
            ConfigurationKey = $"{EnvSettingsKeyEnum.SeznamApiSettings.GetDescription()}.Scopes",
            AdminConfigurable = false,
            BackendOnly = false,
            ConfigurationValue = JsonConvert.SerializeObject(
                new List<string>
                {
                    "identity",
                    "contact-phone",
                    "avatar"
                }
            ),
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 15,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.FacebookApiSettings.GetDescription()}.ClientId",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 16,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.Password,
            ConfigurationKey = $"{EnvSettingsKeyEnum.FacebookApiSettings.GetDescription()}.ClientSecret",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 17,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.FacebookApiSettings.GetDescription()}.StudentRedirectUrl",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 18,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.FacebookApiSettings.GetDescription()}.OauthUrl",
            AdminConfigurable = false,
            BackendOnly = false,
            ConfigurationValue = "https://www.facebook.com/v14.0/dialog/oauth",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 19,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.StringArray,
            ConfigurationKey = $"{EnvSettingsKeyEnum.FacebookApiSettings.GetDescription()}.Scopes",
            AdminConfigurable = false,
            BackendOnly = false,
            ConfigurationValue = JsonConvert.SerializeObject(
                new List<string>
                {
                    "email",
                    "public_profile",
                    "user_birthday"
                }
            ),
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 20,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.Password,
            ConfigurationKey = $"{EnvSettingsKeyEnum.EmailSettings.GetDescription()}.ApiKey",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = null,
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 21,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.RegistrationSettings.GetDescription()}.RedirectUrl",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = null,
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 22,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.EmailSettings.GetDescription()}.SenderEmail",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "noreply@ulearn.cz",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 23,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.EmailSettings.GetDescription()}.SenderName",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "ULearn.cz s.r.o.",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 24,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.GoogleApiSettings.GetDescription()}.StudentRedirectUrl",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = null,
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 25,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.FeSettings.GetDescription()}.FeUrl",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = null,
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 26,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.FileServiceDiskSettings.GetDescription()}.FileStoragePath",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "C:\\ServerData\\ulearn-test\\",
            ValueOrigin = ValueOrigin,
        },
        new EnvConfiguration
        {
            ConfigurationId = 27,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.FileServiceAzureSettings.GetDescription()}.ContainerName",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = null,
            ValueOrigin = ValueOrigin,
        },
        new EnvConfiguration
        {
            ConfigurationId = 28,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.FileServiceAzureSettings.GetDescription()}.Connection",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = null,
            ValueOrigin = ValueOrigin,
        },
        new EnvConfiguration
        {
            ConfigurationId = 29,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.Password,
            ConfigurationKey = $"{EnvSettingsKeyEnum.FileServiceAzureSettings.GetDescription()}.Password",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = null,
            ValueOrigin = ValueOrigin,
        },
        new EnvConfiguration
        {
            ConfigurationId = 30,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.InstanceSettings.GetDescription()}.InstanceName",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin,
        },
        new EnvConfiguration
        {
            ConfigurationId = 31,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.RemoteFileSystemSettings.GetDescription()}.ApiUrl",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin,
        },
        new EnvConfiguration
        {
            ConfigurationId = 32,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.RemoteFileSystemSettings.GetDescription()}.AccessToken",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin,
        },
        new EnvConfiguration
        {
            ConfigurationId = 33,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.ObjectArray,
            ConfigurationKey = $"{EnvSettingsKeyEnum.VideoCallSettings.GetDescription()}.Servers",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "[]",
            ValueOrigin = ValueOrigin,
        },
        new EnvConfiguration
        {
            ConfigurationId = 34,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.Boolean,
            ConfigurationKey = $"{EnvSettingsKeyEnum.VideoCallSettings.GetDescription()}.Trickle",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "true",
            ValueOrigin = ValueOrigin,
        },
        new EnvConfiguration
        {
            ConfigurationId = 35,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.AuthSettings.GetDescription()}.Domain",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "localhost",
            ValueOrigin = ValueOrigin,
        },
        new EnvConfiguration
        {
            ConfigurationId = 36,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.RegistrationSettings.GetDescription()}.ResetPasswordRedirectUrl",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin,
        },
        new EnvConfiguration
        {
            ConfigurationId = 37,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.FacebookApiSettings.GetDescription()}.TutorRedirectUrl",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 38,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.SeznamApiSettings.GetDescription()}.TutorRedirectUrl",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 39,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.StripeSettings.GetDescription()}.PublishableKey",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 40,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.Password,
            ConfigurationKey = $"{EnvSettingsKeyEnum.StripeSettings.GetDescription()}.SecretKey",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 41,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.StripeSettings.GetDescription()}.ReservationSuccessUrl",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 42,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.StripeSettings.GetDescription()}.ReservationCancelUrl",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 43,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.Password,
            ConfigurationKey = $"{EnvSettingsKeyEnum.StripeSettings.GetDescription()}.WebhookKey",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 44,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.InstanceSettings.GetDescription()}.BeApiUrl",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 45,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.StripeSettings.GetDescription()}.MoneyRefillSuccessUrl",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 46,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.StripeSettings.GetDescription()}.MoneyRefillCancelUrl",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 47,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.Password,
            ConfigurationKey = $"{EnvSettingsKeyEnum.VideoCallSettings.GetDescription()}.Secret",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 48,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.VideoCallSettings.GetDescription()}.ApiKey",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 49,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.SlackSettings.GetDescription()}.WebhookEndpoint",
            AdminConfigurable = true,
            BackendOnly = true,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 50,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.GoogleApiSettings.GetDescription()}.CompanyRedirectUrl",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 51,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.StringArray,
            ConfigurationKey = $"{EnvSettingsKeyEnum.GoogleApiSettings.GetDescription()}.CompanyScopes",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = JsonConvert.SerializeObject(
                new List<string>
                {
                    "https://www.googleapis.com/auth/userinfo.profile",
                    "https://www.googleapis.com/auth/userinfo.email"
                }
            ),
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 52,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.FacebookApiSettings.GetDescription()}.CompanyRedirectUrl",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 53,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.SeznamApiSettings.GetDescription()}.CompanyRedirectUrl",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 54,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.Password,
            ConfigurationKey = $"{EnvSettingsKeyEnum.HubSpotSettings.GetDescription()}.ApiKey",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
        new EnvConfiguration
        {
            ConfigurationId = 55,
            DataTypeId = (int)EnvConfigurationDataTypeEnum.String,
            ConfigurationKey = $"{EnvSettingsKeyEnum.SlackSettings.GetDescription()}.LowPriorityWebhookEndpoint",
            AdminConfigurable = true,
            BackendOnly = false,
            ConfigurationValue = "",
            ValueOrigin = ValueOrigin
        },
    };
}