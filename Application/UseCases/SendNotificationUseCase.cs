using NotificationAPI.Application.DTOs;
using NotificationAPI.Application.Interfaces;
using NotificationAPI.Application.Validators;
using NotificationAPI.Domain.Entities;
using NotificationAPI.Domain.Enums;

namespace NotificationAPI.Application.UseCases;

public class SendNotificationUseCase
{
    private readonly INotificationRepository _notificationRepo;
    private readonly IUserRepository _userRepo;
    private readonly SendNotificationRequestValidator _validator;

    public SendNotificationUseCase(
        INotificationRepository notificationRepo,
        IUserRepository userRepo,
        SendNotificationRequestValidator validator)
    {
        _notificationRepo = notificationRepo;
        _userRepo = userRepo;
        _validator = validator;
    }

    public async Task<SendNotificationResponse> ExecuteAsync(SendNotificationRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new SendNotificationResponse
            {
                Status = "Failed",
                Message = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
            };
        }

        var user = await _userRepo.GetByIdAsync(request.UserId);
        if (user is null)
        {
            return new SendNotificationResponse
            {
                Status = "Failed",
                Message = "User not found"
            };
        }

        var notification = new Notification
        {
            UserId = request.UserId,
            Subject = request.Subject,
            Message = request.Message,
            CreatedAt = DateTime.UtcNow,
            Status = NotificationStatus.Pending,
            IsRead = false
        };

        var saved = await _notificationRepo.AddAsync(notification);
        await _notificationRepo.SaveChangesAsync();

        return new SendNotificationResponse
        {
            NotificationId = saved.Id,
            Status = "Sent",
            Message = "Notification sent successfully"
        };
    }
}
