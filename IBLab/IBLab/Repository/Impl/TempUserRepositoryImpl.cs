using IBLab.Data;
using IBLab.Repository.Interfaces;

namespace IBLab.Repository.Impl
{
    public class TempUserRepositoryImpl : ITempUserRepository
    {
        private readonly IBLabContext _context;

        public TempUserRepositoryImpl(IBLabContext context)
        {
            _context = context;
        }

        public async Task CleanupExpiredTempUsers()
        {
            var expiredUsers = _context.TempUsers
                .Where(u => u.ExpirationTime <= DateTime.UtcNow)
                .ToList();

            if (expiredUsers.Any())
            {
                _context.TempUsers.RemoveRange(expiredUsers);
                await _context.SaveChangesAsync();
            }
        }
    }
}
