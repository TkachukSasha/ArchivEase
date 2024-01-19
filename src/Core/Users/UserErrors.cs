using SharedKernel.Errors;

namespace Core.Users;

public static class UserErrors
{
    public static readonly Error UserNameMustBeProvide = Error.Validation(
         $"[{nameof(User)}]",
         "User name must be provide"
    );

    public static readonly Error PasswordMustBeProvide = Error.Validation(
        $"[{nameof(User)}]",
        "Password must be provide"
   );

    public static Error UserIsNotFound(string userName) => Error.NotFound(
        $"[{nameof(User)}]",
        $"user with username: [{userName}] not found!"
    );

    public static Error UserPasswordIsNotValid(string password) => Error.Failure(
       $"[{nameof(User)}]",
       $"user password: [{password}] isn't valid!"
   );
}