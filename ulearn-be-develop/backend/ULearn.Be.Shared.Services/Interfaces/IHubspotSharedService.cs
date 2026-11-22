namespace ULearn.Be.Shared.Services.Interfaces;

public interface IHubspotSharedService
{
    Task SyncUser(int userId);
}