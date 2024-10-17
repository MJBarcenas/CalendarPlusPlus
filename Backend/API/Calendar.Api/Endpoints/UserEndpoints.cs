using System.Data;
using Calendar.Api.Dtos;
using Calendar.Api.Mapping;
using Calendar.Api.Utilities;

namespace Calendar.Api.Endpoints;

public static class UserEndpoints
{
    const string GetUserEndpointName = "GetUser";

    public static RouteGroupBuilder MapUsersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("users")
                       .WithParameterValidation();

        // Get all users
        group.MapGet("/", () =>
        {
            string query = "SELECT * FROM Users";
            DataTable users = Global.GetDataTable(query);
            return Results.Ok(users.AsEnumerable().Select(user => user.ToUserDto()));
        });

        // Get a user
        group.MapGet("/{id}", (int id) =>
        {
            string getUser = $"SELECT * FROM Users WHERE UserId = {id}";
            DataTable resultUser = Global.GetDataTable(getUser);
            if (resultUser.Rows.Count == 0)
            {
                return Results.NotFound();
            }

            UserDto user = resultUser.Rows[0].ToUserDto();
            return Results.Ok(user);
        })
        .WithName(GetUserEndpointName); // <-- Route creation used when a user is created.

        // Create new user
        group.MapPost("/", (CreateUserDto newUser) =>
        {
            string insertUser = "INSERT INTO Users(Username, Password, UserType) VALUES(@username, @password, @usertype); SELECT SCOPE_IDENTITY();";
            Dictionary<Tuple<string, object>, SqlDbType> parameters = new Dictionary<Tuple<string, object>, SqlDbType>();
            parameters.Add(Tuple.Create("@username", (object)newUser.Username), SqlDbType.NVarChar);
            parameters.Add(Tuple.Create("@password", (object)newUser.Password), SqlDbType.NVarChar);
            parameters.Add(Tuple.Create("@usertype", (object)newUser.UserType), SqlDbType.NVarChar);

            var result = Global.ManageDataScalar(insertUser, CommandType.Text, parameters);
            int insertedId = -1;
            if (!int.TryParse(result?.ToString(), out insertedId))
            {
                return Results.BadRequest();
            }

            return Results.CreatedAtRoute(GetUserEndpointName, new { id = insertedId });
        });

        // Updating user info
        group.MapPut("/{id}", (int id, UpdateUserDto updatedUser) =>
        {
            string getUser = $"SELECT * FROM Users WHERE UserId = {id}";
            DataTable resultUser = Global.GetDataTable(getUser);
            if (resultUser.Rows.Count == 0)
            {
                return Results.NotFound();
            }

            string updateUser = "UPDATE Users SET Username = @username, Password = @password, UserType = @usertype, IsEnabled = @isenabled WHERE UserId = @userid";
            Dictionary<Tuple<string, object>, SqlDbType> parameters = new Dictionary<Tuple<string, object>, SqlDbType>();
            parameters.Add(Tuple.Create("@userid", (object)id), SqlDbType.Int);
            parameters.Add(Tuple.Create("@username", (object)updatedUser.Username), SqlDbType.NVarChar);
            parameters.Add(Tuple.Create("@password", (object)updatedUser.Password), SqlDbType.NVarChar);
            parameters.Add(Tuple.Create("@usertype", (object)updatedUser.UserType), SqlDbType.NVarChar);
            parameters.Add(Tuple.Create("@isenabled", (object)updatedUser.IsEnabled), SqlDbType.Bit);

            int result = Global.ManageData(updateUser, CommandType.Text, parameters);
            if (result == -1)
            {
                return Results.BadRequest();
            }

            return Results.NoContent();
        });

        // Delete a user
        group.MapDelete("/{id}", (int id) =>
        {
            string deleteUser = "DELETE FROM Users WHERE UserId = @userid";
            Dictionary<Tuple<string, object>, SqlDbType> parameters = new Dictionary<Tuple<string, object>, SqlDbType>();
            parameters.Add(Tuple.Create("@userid", (object)id), SqlDbType.Int);
            Global.ManageData(deleteUser, CommandType.Text, parameters);

            return Results.NoContent();
        });

        return group;
    }
}
