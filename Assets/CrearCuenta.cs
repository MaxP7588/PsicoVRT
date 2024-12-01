using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrearCuenta : MonoBehaviour
{
    public TMP_InputField nombreInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    
    private MySQLConnector mySQLConnector;

    void Start()
    {
        // Obtener referencia al componente MySQLConnector
        mySQLConnector = FindObjectOfType<MySQLConnector>();
        if (mySQLConnector == null)
        {
            Debug.LogError("No se encontró el componente MySQLConnector en la escena");
        }
    }

    public void OnCreateAccountClick()
    {
        string nombre = nombreInput.text;
        string email = emailInput.text;
        string password = passwordInput.text;

        // Validar campos vacíos
        if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Todos los campos son requeridos");
            return;
        }

        // Validar formato de email
        if (!IsValidEmail(email))
        {
            Debug.LogError("El formato del email no es válido");
            return;
        }

        // Validar longitud de contraseña
        if (password.Length < 8)
        {
            Debug.LogError("La contraseña debe tener al menos 8 caracteres");
            return;
        }

        // Intentar registrar usuario
        if (mySQLConnector.RegisterUser(nombre, email, password))
        {
            Debug.Log("Usuario registrado exitosamente");
            LimpiarCampos();
        }
        else
        {
            Debug.LogError("Error al registrar usuario");
        }
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private void LimpiarCampos()
    {
        nombreInput.text = "";
        emailInput.text = "";
        passwordInput.text = "";
    }
}