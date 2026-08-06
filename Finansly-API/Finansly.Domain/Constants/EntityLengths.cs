namespace Finansly.Domain.Constants;

public static class EntityLengths
{
    public static class User
    {
        public const int Name = 100;
        public const int Email = 256;
        public const int PasswordHash = 256;
        public const int Bio = 500;
    }

    public static class Category
    {
        public const int Name = 100;
    }

    public static class Transaction
    {
        public const int Description = 500;
    }
}