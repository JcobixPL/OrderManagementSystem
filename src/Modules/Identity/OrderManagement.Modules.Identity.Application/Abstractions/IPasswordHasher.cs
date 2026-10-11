namespace OrderManagement.Modules.Identity.Application.Abstractions;

public interface IPasswordHasher
{
    string Hash(string password);
}
