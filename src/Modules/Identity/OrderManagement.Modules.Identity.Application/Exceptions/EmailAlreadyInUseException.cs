namespace OrderManagement.Modules.Identity.Application.Exceptions;

public sealed class EmailAlreadyInUseException : Exception
{
    public EmailAlreadyInUseException(string email)
        : base($"The email '{email}' is already in use.")
    {
    }
}
