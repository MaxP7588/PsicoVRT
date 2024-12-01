using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IniciarSesion : MonoBehaviour
{
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

    public void OnLoginClick()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        // Validar campos vacíos
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
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

        // Intentar iniciar sesión
        if (mySQLConnector.LoginUser(email, password))
        {
            Debug.Log("Inicio de sesión exitoso");
            LimpiarCampos();
            // Aquí puedes agregar código para cambiar de escena o mostrar el menú principal
        }
        else
        {
            Debug.LogError("Credenciales inválidas");
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
        emailInput.text = "";
        passwordInput.text = "";
    }
}