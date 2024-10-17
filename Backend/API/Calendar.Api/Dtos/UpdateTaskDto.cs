using System.ComponentModel.DataAnnotations;

namespace Calendar.Api.Dtos;

public record class UpdateTaskDto
(
    [Required][StringLength(20)]
    string TaskName,
    [Required]
    bool IsActive
);