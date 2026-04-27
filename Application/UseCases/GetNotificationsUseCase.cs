using NotificationAPI.Application.DTOs;
using NotificationAPI.Application.Interfaces;

namespace NotificationAPI.Application.UseCases;

public class GetNotificationsUseCase
{
    private readonly INotificationRepository _notificationRepo;

    public GetNotificationsUseCase(INotificationRepository notificationRepo)
    {
        _notificationRepo = notificationRepo;
    }

    public async Task<List<GetNotificationsResponse>> ExecuteAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("UserId debe ser mayor que 0");

        var notifications = await _notificationRepo.GetByUserIdAsync(userId);

        return notifications
            .Select(n => new GetNotificationsResponse
            {
                Id = n.Id,
                Subject = n.Subject,
                Message = n.Message,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead,
                Status = n.Status.ToString()
            })
            .ToList();
    }
}
