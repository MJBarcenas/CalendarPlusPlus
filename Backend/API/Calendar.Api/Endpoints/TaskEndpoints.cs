using System;
using System.Data;
using Calendar.Api.Dtos;
using Calendar.Api.Mapping;
using Calendar.Api.Utilities;

namespace Calendar.Api.Endpoints;

public static class TaskEndpoints
{
    const string GetTaskEndpointName = "GetTask";

    public static RouteGroupBuilder MapTaskEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("task")
                       .WithParameterValidation();

        // Get all tasks
        group.MapGet("/", () =>
        {
            string query = "SELECT * FROM Tasks";
            DataTable tasks = Global.GetDataTable(query);
            return Results.Ok(tasks.AsEnumerable().Select(task => task.ToTaskDto()));
        });

        // Get a task
        group.MapGet("/{id}", (int id) =>
        {
            string getTask = $"SELECT * FROM Tasks WHERE TaskId = {id}";
            DataTable resultTask = Global.GetDataTable(getTask);
            if (resultTask.Rows.Count == 0)
            {
                return Results.NotFound();
            }

            TaskDto task = resultTask.Rows[0].ToTaskDto();
            return Results.Ok(task);
        })
        .WithName(GetTaskEndpointName); // <-- Route creation used when a user is created.

        // Create new task
        group.MapPost("/", (CreateTaskDto newTask) =>
        {
            string insertTask = "INSERT INTO Tasks(TaskName) VALUES(@taskname); SELECT SCOPE_IDENTITY();";
            Dictionary<Tuple<string, object>, SqlDbType> parameters = new Dictionary<Tuple<string, object>, SqlDbType>();
            parameters.Add(Tuple.Create("@taskname", (object)newTask.TaskName), SqlDbType.NVarChar);

            var result = Global.ManageDataScalar(insertTask, CommandType.Text, parameters);
            int insertedId = -1;
            if (!int.TryParse(result?.ToString(), out insertedId))
            {
                return Results.BadRequest();
            }

            return Results.CreatedAtRoute(GetTaskEndpointName, new { id = insertedId });
        });

        // Updating task info
        group.MapPut("/{id}", (int id, UpdateTaskDto updatedTask) =>
        {
            string getTask = $"SELECT * FROM Tasks WHERE TaskId = {id}";
            DataTable resultTask = Global.GetDataTable(getTask);
            if (resultTask.Rows.Count == 0)
            {
                return Results.NotFound();
            }

            string updateTask = "UPDATE Tasks SET TaskName = @taskname, IsActive = @isactive WHERE TaskId = @taskid";
            Dictionary<Tuple<string, object>, SqlDbType> parameters = new Dictionary<Tuple<string, object>, SqlDbType>();
            parameters.Add(Tuple.Create("@taskid", (object)id), SqlDbType.Int);
            parameters.Add(Tuple.Create("@taskname", (object)updatedTask.TaskName), SqlDbType.NVarChar);
            parameters.Add(Tuple.Create("@isactive", (object)updatedTask.IsActive), SqlDbType.Bit);

            int result = Global.ManageData(updateTask, CommandType.Text, parameters);
            if (result == -1)
            {
                return Results.BadRequest();
            }

            return Results.NoContent();
        });

        // Delete a task
        group.MapDelete("/{id}", (int id) =>
        {
            string deleteTask = "DELETE FROM Tasks WHERE TaskId = @taskid";
            Dictionary<Tuple<string, object>, SqlDbType> parameters = new Dictionary<Tuple<string, object>, SqlDbType>();
            parameters.Add(Tuple.Create("@taskid", (object)id), SqlDbType.Int);
            Global.ManageData(deleteTask, CommandType.Text, parameters);

            return Results.NoContent();
        });

        return group;
    }
}
