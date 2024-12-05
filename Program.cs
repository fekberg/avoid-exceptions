using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

// Execute all available benchmarks in the current assembly
BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

// Is it worth refactoring our code to avoid exceptions?
[SimpleJob(RuntimeMoniker.Net481)]
[SimpleJob(RuntimeMoniker.Net80, baseline: true)]
[SimpleJob(RuntimeMoniker.Net90)]
public class BenchmarkExceptions
{
    private UserRepository repository = new();

    [Benchmark]
    public async Task<(int, int)> UsingExceptions()
    {
        int validUsers = 0;
        int invalidUsers = 0;
        for (int i = 0; i < 1000000; i++)
        {
            try
            {
                var user = await repository.GetUserAsync();

                Validator.EnsureUserValid(user);

                validUsers += 1;
            }
            catch (InvalidUserException ex)
            {
                invalidUsers += 1;
            }
            catch (Exception)
            {
                invalidUsers += 1;
            }
        }

        return (validUsers, invalidUsers);
    }

    [Benchmark]
    public async Task<(int, int)> AvoidExceptions()
    {
        int validUsers = 0;
        int invalidUsers = 0;
        for (int i = 0; i < 1000000; i++)
        {
            var user = await repository.GetUserAsync();

            if (Validator.IsUserValid(user))
            {
                validUsers += 1;
            }
            else
            {
                invalidUsers += 1;
            }
        }

        return (validUsers, invalidUsers);
    }

    [Benchmark]
    public async Task<(int, int)> AvoidExceptionsUsingResultPattern()
    {
        int validUsers = 0;
        int invalidUsers = 0;
        for (int i = 0; i < 1000000; i++)
        {
            var user = await repository.GetUserAsync();

            if (Validator.ValidateUser(user) is Failure<User> failure)
            {
                invalidUsers += 1;
            }
            else
            {
                validUsers += 1;
            }
        }

        return (validUsers, invalidUsers);
    }
}