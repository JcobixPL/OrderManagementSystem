using MediatR;
using OrderManagement.Modules.Identity.Application.Abstractions;
using OrderManagement.Modules.Identity.Application.Exceptions;
using OrderManagement.Modules.Identity.Domain.Entities;
using OrderManagement.Modules.Identity.Domain.Enums;

namespace OrderManagement.Modules.Identity.Application.Features.Auth.Register;

internal sealed class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IIdentityUnitOfWork _unitOfWork;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IIdentityUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var emailExists = await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken);

        if (emailExists)
            throw new EmailAlreadyInUseException(request.Email);

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = new User(
            request.Email,
            passwordHash,
            Role.Customer);

        await _userRepository.AddAsync(user, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
