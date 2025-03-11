namespace ShareMyAdventures.Application.Common.Interfaces
{
    public interface IApplicationUserManager
    {
        Task FindFriendAsync(string userId, long id);
    }
}