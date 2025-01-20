using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class CrearCuenta : MonoBehaviour
{
    public TMP_InputField nombreInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public GameObject vistaLogin;
    public GameObject vistaCrearCuenta;
    [SerializeField] private GameObject mensajeError; // Nuevo objeto para mostrar mensajes de error
    [SerializeField] private TMP_Text textoMensajeError; // Texto del mensaje de error
    
    private MySQLConnector mySQLConnector;

    void Start()
    {
        // Obtener referencia al componente MySQLConnector
        mySQLConnector = FindObjectOfType<MySQLConnector>();
        if (mySQLConnector == null)
        {
            Debug.LogError("No se encontró el componente MySQLConnector en la escena");
        }

        // Asegurarse de que el mensaje de error esté desactivado al inicio
        mensajeError.SetActive(false);
    }

    public void OnCreateAccountClick()
    {
        string nombre = nombreInput.text;
        string email = emailInput.text;
        string password = passwordInput.text;

        // Validar campos vacíos
        if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            MostrarMensajeError("Todos los campos son requeridos");
            return;
        }

        // Validar formato de email
        if (!IsValidEmail(email))
        {
            MostrarMensajeError("El formato del email no es válido");
            return;
        }

        // Validar longitud de contraseña
        if (password.Length < 8)
        {
            MostrarMensajeError("La contraseña debe tener al menos 8 caracteres");
            return;
        }

        // Intentar registrar usuario
        if (mySQLConnector.RegisterUser(nombre, email, password))
        {
            Debug.Log("Usuario registrado exitosamente");
            LimpiarCampos();
            VolverAlLogin();
        }
        else
        {
            MostrarMensajeError("Error al registrar usuario");
        }
    }

    private void VolverAlLogin()
    {
        // Implementar lógica para volver a la pantalla de login
        vistaCrearCuenta.SetActive(false);
        vistaLogin.SetActive(true);
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

    private void MostrarMensajeError(string mensaje)
    {
        textoMensajeError.text = mensaje;

        CanvasGroup canvasGroup = mensajeError.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = mensajeError.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 1; // Reiniciar la opacidad a 1

        mensajeError.SetActive(true);
        StartCoroutine(DesvanecerMensajeError());
    }

    private IEnumerator DesvanecerMensajeError()
    {
        yield return new WaitForSeconds(3);

        CanvasGroup canvasGroup = mensajeError.GetComponent<CanvasGroup>();
        float fadeDuration = 1.0f;
        float fadeSpeed = 1.0f / fadeDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, progress);
            progress += fadeSpeed * Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0;
        mensajeError.SetActive(false);
    }
}