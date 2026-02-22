using static BCrypt.Net.BCrypt; // Note the 'static' keyword and the double BCrypt
using Finansly.Application.Common;

namespace Finansly.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) => EnhancedHashPassword(password);

    public bool Verify(string password, string passwordHash) => EnhancedVerify(password, passwordHash);
}