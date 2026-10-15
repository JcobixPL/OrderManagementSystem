using Moq;
using OrderManagement.Modules.Identity.Application.Abstractions;
using OrderManagement.Modules.Identity.Application.Exceptions;
using OrderManagement.Modules.Identity.Application.Features.Auth.Register;
using OrderManagement.Modules.Identity.Domain.Entities;
using OrderManagement.Modules.Identity.Domain.Enums;

namespace OrderManagement.Modules.Identity.UnitTests;

public sealed class RegisterCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldRegisterUser_WhenEmailIsAvailable()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var unitOfWorkMock = new Mock<IIdentityUnitOfWork>();
        var passwordHasherMock = new Mock<IPasswordHasher>();

        userRepositoryMock
            .Setup(x => x.ExistsByEmailAsync(
                "test@example.com",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        passwordHasherMock
            .Setup(x => x.Hash("Password123"))
            .Returns("hashed-password");

        var handler = new RegisterCommandHandler(
            userRepositoryMock.Object,
            passwordHasherMock.Object,
            unitOfWorkMock.Object);

        var command = new RegisterCommand(
            "test@example.com",
            "Password123");

        // Act
        var userId = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, userId);

        userRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<User>(user =>
                    user.Email == "test@example.com" &&
                    user.PasswordHash == "hashed-password" &&
                    user.Role == Role.Customer),
                It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowEmailAlreadyInUseException_WhenEmailAlreadyExists()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var unitOfWorkMock = new Mock<IIdentityUnitOfWork>();
        var passwordHasherMock = new Mock<IPasswordHasher>();

        userRepositoryMock
            .Setup(x => x.ExistsByEmailAsync(
                "test@example.com",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new RegisterCommandHandler(
            userRepositoryMock.Object,
            passwordHasherMock.Object,
            unitOfWorkMock.Object);

        var command = new RegisterCommand(
            "test@example.com",
            "Password123");

        // Act
        var exception = await Assert.ThrowsAsync<EmailAlreadyInUseException>(
             () => handler.Handle(
                command,
                CancellationToken.None));

        Assert.Contains(
            "test@example.com",
            exception.Message);

        passwordHasherMock.Verify(
            x => x.Hash(It.IsAny<string>()),
            Times.Never);

        userRepositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<User>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}