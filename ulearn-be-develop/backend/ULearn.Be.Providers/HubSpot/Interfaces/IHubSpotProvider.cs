using ULearn.Be.Providers.HubSpot.Models;
using ULearn.Be.Providers.HubSpot.Models.Search.Response;

namespace ULearn.Be.Providers.HubSpot.Interfaces;

public interface IHubSpotProvider
{
    Task<HubSpotSearchResultVM> SearchUser(string email, bool throwIfNotSuccess = false);
    Task<HubSpotUpsertResponseVM> CreateContact(HubSpotInputVM input, bool throwIfNotSuccess = false);
    Task<HubSpotUpsertResponseVM> UpdateContact(string contactId, HubSpotInputVM input, bool throwIfNotSuccess = false);
}