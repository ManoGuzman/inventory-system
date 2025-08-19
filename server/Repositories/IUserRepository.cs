using Server.Models;

namespace Server.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User> CreateAsync(User user);
        Task<User?> UpdateAsync(int id, User user);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByUsernameAsync(string username, int? excludeId = null);
        Task UpdateLastLoginAsync(int userId);
    }
}
