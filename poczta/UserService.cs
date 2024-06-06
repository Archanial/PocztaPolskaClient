namespace poczta;

public interface IUserService
{
    Task<User> Authenticate(string username, string password);
    Task<IEnumerable<User>> GetAll();
}

public sealed class UserService : IUserService
{
    private readonly List<User> _users =
        [new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }];

    public async Task<User> Authenticate(string username, string password)
    {
        // wrapped in "await Task.Run" to mimic fetching user from a db
        var user = await Task.Run(() => _users.SingleOrDefault(x => x.Username == username && x.Password == password));

        // on auth fail: null is returned because user is not found
        // on auth success: user object is returned
        return user!;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        // wrapped in "await Task.Run" to mimic fetching users from a db
        return await Task.Run(() => _users);
    }
}