public class UserRepository
{
    public async Task<User> GetUserAsync()
    {
        await Task.Yield();

        return new User("");
    }
}