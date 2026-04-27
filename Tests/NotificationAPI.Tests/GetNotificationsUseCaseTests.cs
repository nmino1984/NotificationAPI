using Moq;
using NotificationAPI.Application.Interfaces;
using NotificationAPI.Application.UseCases;
using NotificationAPI.Domain.Entities;
using NotificationAPI.Domain.Enums;

namespace NotificationAPI.Tests;

public class GetNotificationsUseCaseTests
{
    private readonly Mock<INotificationRepository> _mockRepo;
    private readonly GetNotificationsUseCase _useCase;

    public GetNotificationsUseCaseTests()
    {
        _mockRepo = new Mock<INotificationRepository>();
        _useCase = new GetNotificationsUseCase(_mockRepo.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidUserId_ReturnsNotificationList()
    {
        // Arrange
        var userId = 1;
        var notifications = new List<Notification>
        {
            new()
            {
                Id = 1,
                UserId = userId,
                Subject = "Test 1",
                Message = "Message 1",
                CreatedAt = DateTime.UtcNow,
                Status = NotificationStatus.Sent,
                IsRead = false
            },
            new()
            {
                Id = 2,
                UserId = userId,
                Subject = "Test 2",
                Message = "Message 2",
                CreatedAt = DateTime.UtcNow,
                Status = NotificationStatus.Pending,
                IsRead = false
            }
        };

        _mockRepo.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(notifications);

        // Act
        var result = await _useCase.ExecuteAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Test 1", result[0].Subject);
        Assert.Equal("Test 2", result[1].Subject);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidUserId_MapsStatusToString()
    {
        // Arrange
        var userId = 1;
        var notifications = new List<Notification>
        {
            new() { Id = 1, UserId = userId, Subject = "S", Message = "M",
                    Status = NotificationStatus.Sent, CreatedAt = DateTime.UtcNow }
        };

        _mockRepo.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(notifications);

        // Act
        var result = await _useCase.ExecuteAsync(userId);

        // Status is the enum name serialized as a string ("Sent", "Pending", "Failed")
        Assert.Equal("Sent", result[0].Status);
    }

    [Fact]
    public async Task ExecuteAsync_WithInvalidUserId_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(-1));
    }

    [Fact]
    public async Task ExecuteAsync_WithZeroUserId_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(0));
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoNotifications_ReturnsEmptyList()
    {
        // Arrange
        var userId = 42;
        _mockRepo.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(new List<Notification>());

        // Act
        var result = await _useCase.ExecuteAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidUserId_MapsIsReadCorrectly()
    {
        // Arrange
        var userId = 1;
        var notifications = new List<Notification>
        {
            new() { Id = 1, UserId = userId, Subject = "Read", Message = "M",
                    Status = NotificationStatus.Sent, IsRead = true, CreatedAt = DateTime.UtcNow },
            new() { Id = 2, UserId = userId, Subject = "Unread", Message = "M",
                    Status = NotificationStatus.Pending, IsRead = false, CreatedAt = DateTime.UtcNow }
        };

        _mockRepo.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(notifications);

        var result = await _useCase.ExecuteAsync(userId);

        Assert.True(result[0].IsRead);
        Assert.False(result[1].IsRead);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidUserId_ReturnsCorrectIds()
    {
        // Arrange
        var userId = 5;
        var notifications = new List<Notification>
        {
            new() { Id = 10, UserId = userId, Subject = "A", Message = "M",
                    Status = NotificationStatus.Pending, CreatedAt = DateTime.UtcNow }
        };

        _mockRepo.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(notifications);

        var result = await _useCase.ExecuteAsync(userId);

        Assert.Equal(10, result[0].Id);
    }
}
