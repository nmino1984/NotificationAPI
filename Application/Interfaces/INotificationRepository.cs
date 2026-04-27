using NotificationAPI.Domain.Entities;

namespace NotificationAPI.Application.Interfaces;

public interface INotificationRepository
{
    Task<Notification> AddAsync(Notification notification);
    Task<List<Notification>> GetByUserIdAsync(int userId);
    Task<Notification?> GetByIdAsync(int id);
    Task UpdateAsync(Notification notification);
    Task SaveChangesAsync();
}
