namespace ULearn.Be.Repositories.Interfaces;

public interface IAppUserProfileBehaviourRepository
{
    Task<bool> Exists(int userId, int visitedUserId);
    Task Create(int userId, int visitedUserId);
}