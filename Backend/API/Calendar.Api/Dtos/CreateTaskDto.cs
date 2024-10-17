using System.ComponentModel.DataAnnotations;

namespace Calendar.Api.Dtos;

public record class CreateTaskDto
(
    [Required][StringLength(20)]
    string TaskName
);
