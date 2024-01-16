using SharedKernel.Errors;

namespace Core.Users;

public static class UserErrors
{
    public static readonly Error UserNameMustBeProvide = new Error(
         $"[{nameof(User)}]",
         "User name must be provide"
    );

    public static readonly Error PasswordMustBeProvide = new Error(
        $"[{nameof(User)}]",
        "Password must be provide"
   );
}