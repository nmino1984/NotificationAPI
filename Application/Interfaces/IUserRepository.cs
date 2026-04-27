using NotificationAPI.Domain.Entities;

namespace NotificationAPI.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User> AddAsync(User user);
    Task<bool> ExistsByEmailAsync(string email);
    Task SaveChangesAsync();
}
