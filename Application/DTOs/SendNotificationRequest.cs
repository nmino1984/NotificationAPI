namespace NotificationAPI.Application.DTOs;

public class SendNotificationRequest
{
    public int UserId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
