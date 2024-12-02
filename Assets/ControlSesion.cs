using UnityEngine;

public class ControlSesion : MonoBehaviour
{
    // Singleton instance
    public static ControlSesion Instance { get; private set; }

    // Datos de la sesión actual
    private int userId;
    private string userName;
    private string userEmail;
    private bool isLoggedIn;

    void Awake()
    {
        // Configurar singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Getters para información de sesión
    public bool IsLoggedIn() => isLoggedIn;
    public int GetUserId() => userId;
    public string GetUserName() => userName;
    public string GetUserEmail() => userEmail;

    // Iniciar sesión
    public bool IniciarSesion(int id, string nombre, string email)
    {
        if (isLoggedIn)
        {
            Debug.LogWarning("Ya hay una sesión activa");
            return false;
        }

        userId = id;
        userName = nombre;
        userEmail = email;
        isLoggedIn = true;
        Debug.Log($"Sesión iniciada: {userName}");
        return true;
    }

    // Cerrar sesión
    public void CerrarSesion()
    {
        userId = 0;
        userName = null;
        userEmail = null;
        isLoggedIn = false;
        Debug.Log("Sesión cerrada");
    }
}