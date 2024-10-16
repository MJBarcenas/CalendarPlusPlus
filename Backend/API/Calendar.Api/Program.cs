using Calendar.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<UserDto> users = [
    new (1, "van_", "1234567", "Admin", true),
    new (2, "mae_", "1234567", "User", true)
];

const string GetUserEndpointName = "GetUser";

// Get all users
app.MapGet("users", () => users);

// Get a user
app.MapGet("users/{id}", (int id) => users.Find(user => user.UserId == id))
   .WithName(GetUserEndpointName); // <-- Route creation used when a user is created.

// Create new user
app.MapPost("users", (CreateUserDto newUser) =>
{
    UserDto user = new(
        users.Count + 1,
        newUser.Username,
        newUser.Password,
        newUser.UserType,
        true
    );

    users.Add(user);

    return Results.CreatedAtRoute(GetUserEndpointName, new { id = user.UserId }, user);
});

app.Run();
