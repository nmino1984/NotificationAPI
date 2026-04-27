using Moq;
using NotificationAPI.Application.DTOs;
using NotificationAPI.Application.Interfaces;
using NotificationAPI.Application.UseCases;
using NotificationAPI.Application.Validators;
using NotificationAPI.Domain.Entities;
using NotificationAPI.Domain.Enums;

namespace NotificationAPI.Tests;

public class SendNotificationUseCaseTests
{
    private readonly Mock<INotificationRepository> _mockNotificationRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    
    // Real validator used — pure logic with no external dependencies, no reason to mock it
    private readonly SendNotificationRequestValidator _validator;
    private readonly SendNotificationUseCase _useCase;

    public SendNotificationUseCaseTests()
    {
        _mockNotificationRepo = new Mock<INotificationRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        _validator = new SendNotificationRequestValidator();
        _useCase = new SendNotificationUseCase(
            _mockNotificationRepo.Object,
            _mockUserRepo.Object,
            _validator);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = new SendNotificationRequest
        {
            UserId = 1,
            Subject = "Test Subject",
            Message = "Test message"
        };

        var user = new User { Id = 1, Name = "John", Email = "john@example.com" };
        var savedNotification = new Notification
        {
            Id = 1,
            UserId = 1,
            Subject = request.Subject,
            Message = request.Message,
            Status = NotificationStatus.Pending
        };

        _mockUserRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
        _mockNotificationRepo.Setup(r => r.AddAsync(It.IsAny<Notification>())).ReturnsAsync(savedNotification);

        // Act
        var response = await _useCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);
        // Entity status stays Pending; "Sent" in the response means the use case completed successfully
        Assert.Equal("Sent", response.Status);
        Assert.Equal(1, response.NotificationId);
    }

    [Fact]
    public async Task ExecuteAsync_WithEmptySubject_ReturnsFailedWithSubjectError()
    {
        // Arrange
        var request = new SendNotificationRequest
        {
            UserId = 1,
            Subject = "",
            Message = "Test message"
        };

        // Act
        var response = await _useCase.ExecuteAsync(request);

        // Assert
        Assert.Equal("Failed", response.Status);
        Assert.Contains("Subject", response.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WithEmptyMessage_ReturnsFailedWithMessageError()
    {
        // Arrange
        var request = new SendNotificationRequest
        {
            UserId = 1,
            Subject = "Valid subject",
            Message = ""
        };

        var response = await _useCase.ExecuteAsync(request);

        Assert.Equal("Failed", response.Status);
        Assert.Contains("Message", response.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WithInvalidUserId_ReturnsFailedWithUserIdError()
    {
        // Arrange
        var request = new SendNotificationRequest
        {
            UserId = 0,
            Subject = "Test",
            Message = "Test message"
        };

        var response = await _useCase.ExecuteAsync(request);

        Assert.Equal("Failed", response.Status);
        Assert.Contains("UserId", response.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistentUser_ReturnsFailedUserNotFound()
    {
        // Arrange
        var request = new SendNotificationRequest
        {
            UserId = 999,
            Subject = "Test",
            Message = "Test message"
        };

        _mockUserRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((User?)null);

        // Act
        var response = await _useCase.ExecuteAsync(request);

        // Assert
        Assert.Equal("Failed", response.Status);
        Assert.Contains("User not found", response.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidRequest_CallsAddAsyncOnce()
    {
        // Arrange
        var request = new SendNotificationRequest { UserId = 1, Subject = "Test", Message = "Msg" };
        var user = new User { Id = 1, Name = "Ana", Email = "ana@test.com" };
        var saved = new Notification { Id = 5, UserId = 1, Subject = "Test", Message = "Msg" };

        _mockUserRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
        _mockNotificationRepo.Setup(r => r.AddAsync(It.IsAny<Notification>())).ReturnsAsync(saved);

        // Act
        await _useCase.ExecuteAsync(request);

        // Verify both Add and SaveChanges are called — staging without persisting would be a silent bug
        _mockNotificationRepo.Verify(r => r.AddAsync(It.IsAny<Notification>()), Times.Once);
        _mockNotificationRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithSubjectTooLong_ReturnsFailedResponse()
    {
        // Arrange
        var request = new SendNotificationRequest
        {
            UserId = 1,
            Subject = new string('A', 201),
            Message = "Valid message"
        };

        var response = await _useCase.ExecuteAsync(request);

        Assert.Equal("Failed", response.Status);
        Assert.Contains("Subject", response.Message);
    }
}
