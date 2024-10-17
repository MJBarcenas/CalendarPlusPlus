using System.Data;
using Calendar.Api.Dtos;

namespace Calendar.Api.Mapping;

public static class TaskMapping
{
    public static TaskDto ToTaskDto(this DataRow row)
    {
        return new TaskDto
        (
            (int)row["TaskId"],
            row["TaskName"].ToString(),
            (bool)row["IsActive"]
        );
    }
}
