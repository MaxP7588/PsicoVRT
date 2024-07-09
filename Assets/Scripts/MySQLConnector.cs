using UnityEngine;
using MySqlConnector;
using System;
using UnityEditor.Search;

public class MySQLConnector : MonoBehaviour
{
    private string server = "127.0.0.1";
    private string database = "psicovrt";
    private string uid = "root";
    private string password = "0507";
    private string connectionString;
    private MySqlConnection connection;

    void Start()
    {
        connectionString = $"Server={server};Port=3306;Database={database};User ID={uid};Password={password};";

        try
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
            Debug.Log("Connection successful!");
            consulta();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}");
            if (ex.InnerException != null)
            {
                Debug.LogError($"Inner Exception: {ex.InnerException.Message}");
            }
        }
    }

    private void consulta()
    {
        string query = "SELECT * FROM users";
        using (MySqlCommand cmd = new MySqlCommand(query, connection))
        {
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Asumiendo que la tabla users tiene columnas id, nombre, email
                    int id = reader.GetInt32("id");
                    string nombre = reader.GetString("nombre");
                    string email = reader.GetString("email");

                    Debug.Log($"Usuario: ID={id}, Nombre={nombre}, Email={email}");
                }
            }
        }
    }


    void OnApplicationQuit()
    {
        if (connection != null)
        {
            connection.Close();
            Debug.Log("Connection closed.");
        }
    }

    public void ExecuteQuery(string query)
    {
        if (connection != null && connection.State == System.Data.ConnectionState.Open)
        {
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();
        }
        else
        {
            Debug.LogError("No connection to the database.");
        }
    }
}
