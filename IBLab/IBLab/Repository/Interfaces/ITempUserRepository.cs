namespace IBLab.Repository.Interfaces
{
    public interface ITempUserRepository
    {
        Task CleanupExpiredTempUsers();
    }
}
