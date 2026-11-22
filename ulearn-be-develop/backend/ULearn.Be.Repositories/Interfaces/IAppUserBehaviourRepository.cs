namespace ULearn.Be.Repositories.Interfaces;

public interface IAppUserBehaviourRepository
{
    Task AddConnected(string uuid, string url, int userId);
    Task UpdateSetDisconnected(string uuid);
    Task<bool> IsOnline(int userId);
    Task<DateTime> GetLastVisited(int userId);
    Task<IReadOnlyCollection<int>> GetOnlineUserIds(IReadOnlyCollection<int> userIds);
    Task<IReadOnlyDictionary<int, DateTime>> GetLastVisitedByUserId(IReadOnlyCollection<int> userIds);
}