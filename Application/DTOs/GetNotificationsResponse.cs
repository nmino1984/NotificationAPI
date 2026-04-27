namespace NotificationAPI.Application.DTOs;

public class GetNotificationsResponse
{
    public int Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public string Status { get; set; } = string.Empty;
}
