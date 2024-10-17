namespace Calendar.Api.Entities;

public class Calendar
{
    public int CalendarId { get; set; }
    public int UserId { get; set; }
    public int TaskId { get; set; }
    public string? TaskDescription { get; set; }
    public DateOnly DateCreated { get; set; }
    public DateOnly LastModified { get; set; }
    public bool IsDeleted { get; set; }
}
