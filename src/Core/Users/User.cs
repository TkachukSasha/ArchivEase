using SharedKernel.Abstractions;
using SharedKernel.Errors;

namespace Core.Users;

public sealed class User : Entity<UserId>
{
    private User() { }

    private User(
        Guid id,
        string userName,
        string password
    ) : base(id)
    {
        UserName = userName;
        Password = password;
    }

    public string UserName { get; } = string.Empty;

    public string Password { get; } = string.Empty;

    public static Result<User> Init(
       string userName,
       string password
   ) =>
       Result.Ensure(
           (userName, password),
           (_ => !string.IsNullOrWhiteSpace(userName), UserErrors.UserNameMustBeProvide),
           (_ => !string.IsNullOrWhiteSpace(password), UserErrors.PasswordMustBeProvide)
       )
       .Map(_ => new User(new UserId(), userName, password));
}

public sealed class UserId : TypeId
{
    public UserId()
        : this(Guid.NewGuid()) { }

    public UserId(Guid value) : base(value) { }

    public static implicit operator UserId(Guid id) => new(id);

    public static implicit operator Guid(UserId id) => id.Value;
    public override string ToString() => Value.ToString();
}