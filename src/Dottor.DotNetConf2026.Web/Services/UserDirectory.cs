using System.Collections.Concurrent;
using Dottor.DotNetConf2026.Web.Models;

namespace Dottor.DotNetConf2026.Web.Services;

public sealed class UserDirectory
{
    private readonly ConcurrentDictionary<string, RegisteredUser> users = new(StringComparer.OrdinalIgnoreCase);
    private int nextId = 2;

    public UserDirectory()
    {
        users["anna@example.com"] = new(1, "Anna Rossi", "anna@example.com", 32);
        users["marco@example.com"] = new(2, "Marco Bianchi", "marco@example.com", 41);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        await Task.Delay(800, cancellationToken);
        return users.ContainsKey(email.Trim());
    }

    public RegisteredUser? TryCreate(UserRegistration model)
    {
        var user = new RegisteredUser(Interlocked.Increment(ref nextId), model.FullName.Trim(), model.Email.Trim(), model.Age);
        return users.TryAdd(user.Email, user) ? user : null;
    }
}

public sealed record RegisteredUser(int Id, string FullName, string Email, int Age);