namespace Calendar.Api.Entities;

public class Note
{
    public int NoteId { get; set; }
    public int UserId { get; set; }
    public string? NoteContent { get; set; }
}
