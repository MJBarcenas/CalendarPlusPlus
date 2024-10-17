namespace Calendar.Api.Dtos;

public record class TaskDto
(
    int TaskId,
    string TaskName,
    bool IsActive
);