public class Validator
{
    public static bool IsUserValid(User user)
    {
        return user.Name is { Length: > 0 };
    }

    public static Result<User> ValidateUser(User user)
    {
        return user.Name switch
        {
            { Length: > 0 } => new Success<User>(user),
            _ => new Failure<User>(user, [$"{nameof(user.Name)} should have a valid length"])
        };
    }

    public static void EnsureUserValid(User user)
    {
        if (user.Name is { Length: <= 0 })
        {
            throw new InvalidUserException($"{nameof(user.Name)} should have a valid length");
        }
    }
}