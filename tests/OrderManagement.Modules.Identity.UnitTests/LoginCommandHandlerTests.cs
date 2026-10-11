using Moq;
using OrderManagement.Modules.Identity.Application.Abstractions;
using OrderManagement.Modules.Identity.Application.Exceptions;
using OrderManagement.Modules.Identity.Application.Features.Auth.Login;
using OrderManagement.Modules.Identity.Domain.Entities;
using OrderManagement.Modules.Identity.Domain.Enums;

namespace OrderManagement.Modules.Identity.UnitTests;

public sealed class LoginCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher>();
        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();

        var user = new User(
            "test@example.com",
            "hashed-password",
            Role.Customer);

        userRepositoryMock
            .Setup(x => x.GetByEmailAsync(
                "test@example.com",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        passwordHasherMock
            .Setup(x => x.Verify(
                "hashed-password",
                "Password123"))
            .Returns(true);

        jwtTokenGeneratorMock
            .Setup(x => x.Generate(user))
            .Returns("jwt-token");

        var handler = new LoginCommandHandler(
            userRepositoryMock.Object,
            passwordHasherMock.Object,
            jwtTokenGeneratorMock.Object);

        var command = new LoginCommand(
            "test@example.com",
            "Password123");

        // Act
        var token = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.Equal("jwt-token", token);

        jwtTokenGeneratorMock.Verify(
            x => x.Generate(user),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidCredentialsException_WhenUserDoesNotExist()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher>();
        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();

        userRepositoryMock
            .Setup(x => x.GetByEmailAsync(
                "test@example.com",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var handler = new LoginCommandHandler(
            userRepositoryMock.Object,
            passwordHasherMock.Object,
            jwtTokenGeneratorMock.Object);

        var command = new LoginCommand(
            "test@example.com",
            "Password123");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        passwordHasherMock.Verify(
            x => x.Verify(
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Never);

        jwtTokenGeneratorMock.Verify(
            x => x.Generate(It.IsAny<User>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidCredentialsException_WhenPasswordIsInvalid()
    {
        // Arrange
        var userRepositoryMock = new Mock<IUserRepository>();
        var passwordHasherMock = new Mock<IPasswordHasher>();
        var jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();

        var user = new User(
            "test@example.com",
            "hashed-password",
            Role.Customer);

        userRepositoryMock
            .Setup(x => x.GetByEmailAsync(
                "test@example.com",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        passwordHasherMock
            .Setup(x => x.Verify(
                "hashed-password",
                "WrongPassword"))
            .Returns(false);

        var handler = new LoginCommandHandler(
            userRepositoryMock.Object,
            passwordHasherMock.Object,
            jwtTokenGeneratorMock.Object);

        var command = new LoginCommand(
            "test@example.com",
            "WrongPassword");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        jwtTokenGeneratorMock.Verify(
            x => x.Generate(It.IsAny<User>()),
            Times.Never);
    }
}