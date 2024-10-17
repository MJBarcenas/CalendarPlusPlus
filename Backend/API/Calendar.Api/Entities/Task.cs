namespace Calendar.Api.Entities;

public class Task
{
    public int TaskId { get; set; }
    public required string TaskName { get; set; }
    public bool IsActive { get; set; }
}
