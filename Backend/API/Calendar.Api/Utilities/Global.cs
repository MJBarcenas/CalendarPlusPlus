using System;
using System.Data;
using SqlConnection = System.Data.SqlClient.SqlConnection;
using SqlDataAdapter = System.Data.SqlClient.SqlDataAdapter;
using SqlCommand = System.Data.SqlClient.SqlCommand;

namespace Calendar.Api.Utilities;

public static class Global
{
    public static string ServerName = "DESKTOP-2KNR5MS\\SQLEXPRESS2022,1433";
    public static string User = "sa";
    public static string Password = "michaelpogi";
    public static string DBName = "CalendarPP";
    public static string ConnectionString = $"Data Source={ServerName}; User={User}; Password={Password}; Initial Catalog={DBName}";
    public static DataTable GetDataTable(string query)
    {
        DataTable resultTable = new DataTable();
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                adapter.Fill(resultTable);
                return resultTable;
            }
        }
    }

    public static int ManageData(string query, CommandType type, Dictionary<Tuple<string, object>, SqlDbType> keyValuePairs)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                foreach (KeyValuePair<Tuple<string, object>, SqlDbType> kvp in keyValuePairs)
                {
                    command.Parameters.Add(kvp.Key.Item1, kvp.Value).Value = kvp.Key.Item2;
                }
                command.CommandType = type;
                command.Connection.Open();
                return command.ExecuteNonQuery();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            return -1;
        }
    }

    public static int ManageData(string query, CommandType type)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = type;
                command.Connection.Open();
                return command.ExecuteNonQuery();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            return -1;
        }
    }

    public static object? ManageDataScalar(string query, CommandType type, Dictionary<Tuple<string, object>, SqlDbType> keyValuePairs)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                foreach (KeyValuePair<Tuple<string, object>, SqlDbType> kvp in keyValuePairs)
                {
                    command.Parameters.Add(kvp.Key.Item1, kvp.Value).Value = kvp.Key.Item2;
                }
                command.CommandType = type;
                command.Connection.Open();
                return command.ExecuteScalar();
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
