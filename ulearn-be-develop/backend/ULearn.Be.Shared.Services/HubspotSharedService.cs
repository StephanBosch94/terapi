using ULearn.Be.Common.Extensions;
using ULearn.Be.Data;
using ULearn.Be.Data.Enums;
using ULearn.Be.Providers.Configuration.Interfaces;
using ULearn.Be.Providers.Configuration.Models.Settings;
using ULearn.Be.Providers.HubSpot.Interfaces;
using ULearn.Be.Providers.HubSpot.Models;
using ULearn.Be.Repositories.Interfaces;
using ULearn.Be.Shared.Services.Interfaces;

namespace ULearn.Be.Shared.Services;

public class HubspotSharedService : IHubspotSharedService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly FeSettings _feSettings;
    private readonly IHubSpotProvider _hubSpotProvider;

    public HubspotSharedService(
        IAppUserRepository appUserRepository,
        IHubSpotProvider hubSpotProvider,
        IEnvConfigurationProvider envConfigurationProvider
    )
    {
        _appUserRepository = appUserRepository;
        _hubSpotProvider = hubSpotProvider;
        _feSettings = envConfigurationProvider.GetSettings<FeSettings>(EnvSettingsKeyEnum.FeSettings);
    }

    public async Task SyncUser(int userId)
    {
        var user = await _appUserRepository.GetRawDbEntity(userId, includeDeleted: true);
        var state = (CUserStateEnum)user.UserStateId;
        var role = (CUserRoleEnum)user.UserRoleId;
        var subject = (CSubjectEnum?)user.SubjectId;

        var contactId = await UpsertContactIfNotExists(
            user.HubspotContactId,
            new HubSpotInputVM
            {
                Properties = new HubSpotInputPropertiesVM
                {
                    Company = user.CompanyName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.Phone,
                    Website = GetWebsiteUrl(user),
                    State = state.GetEnumKey(),
                    HourRate = user.HourRate,
                    Role = role.GetEnumKey(),
                    IanaTimezone = user.IanaTimezone,
                    Ico = user.Ico,
                    Vat = user.Dic,
                    IsVatPayer = user.IsVatPayer,
                    Subject = subject?.GetEnumKey(),
                    BusinessAddress = user.PlaceOfBusiness
                }
            }
        );

        if (!string.IsNullOrEmpty(contactId))
        {
            await _appUserRepository.UpdateHubspotInfo(userId, contactId);
        }
    }


    private async Task<string?> UpsertContactIfNotExists(string contactId, HubSpotInputVM input)
    {
        if (string.IsNullOrEmpty(contactId))
        {
            var searchResult = await _hubSpotProvider.SearchUser(input.Properties.Email);
            return searchResult == null
                ? (await _hubSpotProvider.CreateContact(input))?.Id
                : (await _hubSpotProvider.UpdateContact(searchResult.Id, input))?.Id;
        }

        var updateResult = await _hubSpotProvider.UpdateContact(contactId, input);
        return updateResult?.Id;
    }

    private string GetWebsiteUrl(AppUser user)
    {
        var root = _feSettings.FeUrl.LastOrDefault() == '/' ? _feSettings.FeUrl : _feSettings.FeUrl + '/';
        var subjectEnum = (CSubjectEnum?)user.SubjectId ?? CSubjectEnum.English;
        var username = user.FirstName + " " + user.LastName;
        return $"{root}tutor/{subjectEnum.GetEnumKey()}/{user.UserId}/{username}";
    }
}