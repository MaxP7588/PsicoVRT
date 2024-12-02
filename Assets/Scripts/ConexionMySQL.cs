using UnityEngine;
using MySqlConnector;
using System;
using System.Security.Cryptography;
using System.Text;

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
        connectionString = $"Server={server};Port=3307;Database={database};User ID={uid};Password={password};";

        try
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
            Debug.Log("Connection successful!");
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

    private string EncryptPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

    public bool RegisterUser(string nombre, string email, string password)
    {
        try
        {
            // Validar que el email no exista
            string checkEmail = "SELECT COUNT(*) FROM usuarios WHERE email = @email";
            using (MySqlCommand cmd = new MySqlCommand(checkEmail, connection))
            {
                cmd.Parameters.AddWithValue("@email", email);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    Debug.LogError("El email ya está registrado");
                    return false;
                }
            }

            // Encriptar contraseña
            string encryptedPassword = EncryptPassword(password);

            // Insertar nuevo usuario
            string query = "INSERT INTO usuarios (nombre, email, password) VALUES (@nombre, @email, @password)";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", encryptedPassword);
                
                cmd.ExecuteNonQuery();
                Debug.Log("Usuario registrado exitosamente");
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error al registrar usuario: {ex.Message}");
            return false;
        }
    }

    public bool LoginUser(string email, string password)
    {
        // Verificar si ya hay una sesión activa
        if (ControlSesion.Instance.IsLoggedIn())
        {
            Debug.LogWarning("Ya existe una sesión activa");
            return false;
        }

        try
        {
            string encryptedPassword = EncryptPassword(password);
            
            string query = "SELECT id_usuario, nombre, email FROM usuarios WHERE email = @email AND password = @password";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", encryptedPassword);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int userId = reader.GetInt32("id_usuario");
                        string nombre = reader.GetString("nombre");
                        string userEmail = reader.GetString("email");
                        
                        // Iniciar sesión en el controlador
                        if (ControlSesion.Instance.IniciarSesion(userId, nombre, userEmail))
                        {
                            Debug.Log($"Inicio de sesión exitoso. Usuario: {nombre}, ID: {userId}");
                            return true;
                        }
                    }
                    Debug.LogError("Credenciales inválidas");
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error al iniciar sesión: {ex.Message}");
            return false;
        }
    }
}