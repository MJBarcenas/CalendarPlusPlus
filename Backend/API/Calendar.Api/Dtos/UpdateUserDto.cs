using System.ComponentModel.DataAnnotations;

namespace Calendar.Api.Dtos;

public record class UpdateUserDto
(
    [Required][StringLength(20)]
    string Username,
    [Required][StringLength(20)]
    string Password,
    [Required][StringLength(20)]
    string UserType,
    [Required]
    bool IsEnabled
);
