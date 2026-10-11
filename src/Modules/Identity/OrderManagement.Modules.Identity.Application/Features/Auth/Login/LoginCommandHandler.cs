using MediatR;
using OrderManagement.Modules.Identity.Application.Abstractions;
using OrderManagement.Modules.Identity.Application.Exceptions;

namespace OrderManagement.Modules.Identity.Application.Features.Auth.Login;

internal sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<string> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
            throw new InvalidCredentialsException();

        var passwordIsValid = _passwordHasher.Verify(user.PasswordHash, request.Password);

        if (!passwordIsValid)
            throw new InvalidCredentialsException();

        return _jwtTokenGenerator.Generate(user);
    }
}
