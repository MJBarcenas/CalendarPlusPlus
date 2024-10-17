namespace Calendar.Api.Dtos;

public record class UserDto
(
    int UserId,
    string Username,
    string Password,
    string UserType,
    bool IsEnabled
);
