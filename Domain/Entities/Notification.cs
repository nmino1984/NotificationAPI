using NotificationAPI.Domain.Enums;

namespace NotificationAPI.Domain.Entities;

public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public NotificationStatus Status { get; set; }
    public User User { get; set; } = null!;
}
