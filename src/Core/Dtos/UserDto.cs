namespace Core.Dtos;

public record UserDto
(
    Guid UserId,
    string UserName
);

public record UserResponseDto
(
    Guid Id,
    string UserName,
    bool IsAdmin,
    string Token
);