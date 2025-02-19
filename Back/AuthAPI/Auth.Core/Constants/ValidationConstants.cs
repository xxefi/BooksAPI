namespace Auth.Core.Constants;

public static class ValidationConstants
{
    public const int MinUsernameLength = 3;
    public const int MaxUsernameLength = 50;
    public const int MinRoleNameLength = 3;
    public const int MaxRoleNameLength = 50;
    public const int MinPasswordLength = 6; 
    public const int MaxPasswordLength = 100; 
    public const int MinNameLength = 2;
    public const int MaxNameLength = 50;

    public const string UsernameRegex = @"^[a-zA-Z0-9_\-\.]+$";
    public const string PasswordRegex = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$";
    public const string NameRegex = @"^[a-zA-Zа-яА-ЯёЁ]+([a-zA-Zа-яА-ЯёЁ\s\-]*)$"; 
    public const string EmailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
}