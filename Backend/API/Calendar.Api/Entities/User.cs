namespace Calendar.Api.Entities;

public class User
{
    public int UserId { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string UserType { get; set; }
    public bool IsEnabled { get; set; }
}
