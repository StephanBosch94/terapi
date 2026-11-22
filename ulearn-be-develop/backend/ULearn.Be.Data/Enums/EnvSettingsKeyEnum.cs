using System.ComponentModel;
using ULearn.Be.Common.Attributes;

namespace ULearn.Be.Data.Enums;

public enum EnvSettingsKeyEnum
{
    [Description("be.module.auth.settings")] [SettingsObjectName("AuthSettings")]
    AuthSettings = 1,

    [Description("be.module.google_api.settings")] [SettingsObjectName("GoogleApiSettings")]
    GoogleApiSettings = 2,

    [Description("be.module.seznam_api.settings")] [SettingsObjectName("SeznamApiSettings")]
    SeznamApiSettings = 3,

    [Description("be.module.facebook_api.settings")] [SettingsObjectName("FacebookApiSettings")]
    FacebookApiSettings = 4,

    [Description("be.module.email.settings")] [SettingsObjectName("EmailSettings")]
    EmailSettings = 5,

    [Description("be.module.registration.settings")] [SettingsObjectName("RegistrationSettings")]
    RegistrationSettings = 6,

    [Description("be.module.fe.settings")] [SettingsObjectName("FeSettings")]
    FeSettings = 7,

    [Description("be.fileservice.disk.settings")] [SettingsObjectName("DiskSettings")]
    FileServiceDiskSettings = 8,

    [Description("be.fileservice.azureBlob.settings")] [SettingsObjectName("AzureBlobSettings")]
    FileServiceAzureSettings = 9,

    [Description("be.instance.settings")] [SettingsObjectName("InstanceSettings")]
    InstanceSettings = 10,

    [Description("be.fileservice.auth.settings")] [SettingsObjectName("RemoteFileSystemSettings")]
    RemoteFileSystemSettings = 11,

    [Description("be.videocall.settings")] [SettingsObjectName("VideoCallSettings")]
    VideoCallSettings = 12,

    [Description("be.stripe.settings")] [SettingsObjectName("StripeSettings")]
    StripeSettings = 13,

    [Description("be.slack.settings")] [SettingsObjectName("SlackSettings")]
    SlackSettings = 14,
    
    [Description("be.hubspot.settings")] [SettingsObjectName("HubSpotSettings")]
    HubSpotSettings = 15
}