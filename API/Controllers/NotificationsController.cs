using Microsoft.AspNetCore.Mvc;
using NotificationAPI.Application.DTOs;
using NotificationAPI.Application.UseCases;

namespace NotificationAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly SendNotificationUseCase _sendUseCase;
    private readonly GetNotificationsUseCase _getUseCase;

    public NotificationsController(
        SendNotificationUseCase sendUseCase,
        GetNotificationsUseCase getUseCase)
    {
        _sendUseCase = sendUseCase;
        _getUseCase = getUseCase;
    }

    /// <summary>Send a notification to a user</summary>
    [HttpPost("send")]
    [ProducesResponseType(typeof(SendNotificationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendNotification([FromBody] SendNotificationRequest request)
    {
        var response = await _sendUseCase.ExecuteAsync(request);

        if (response.Status == "Failed")
            return BadRequest(response);

        return Ok(response);
    }

    /// <summary>Get all notifications for a user</summary>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(List<GetNotificationsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserNotifications(int userId)
    {
        try
        {
            var notifications = await _getUseCase.ExecuteAsync(userId);
            return Ok(notifications);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
