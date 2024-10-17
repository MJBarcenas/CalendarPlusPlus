using System.Data;
using Calendar.Api.Dtos;
using Calendar.Api.Entities;

namespace Calendar.Api.Mapping;

public static class UserMapping
{
    public static UserDto ToUserDto(this DataRow row)
    {
        return new UserDto
        (
            (int)row["UserId"],
            row["Username"].ToString(),
            row["Password"].ToString(),
            row["UserType"].ToString(),
            (bool)row["IsEnabled"]
        );
    }
}
