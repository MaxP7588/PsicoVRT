using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using NativeFilePickerNamespace; // Asegúrate de importar el namespace del plugin

public class GuardarPDF : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private string nombreArchivo = "Consentimiento_Informado.pdf";
    [SerializeField] private string rutaPersonalizada = "Assets/Models/PDFs/";

    [Header("Referencias UI")]
    [SerializeField] private Button botonGuardar;
    [SerializeField] private TMP_Text textoEstado;

    private string rutaArchivoOrigen;

    private void Start()
    {
        rutaArchivoOrigen = Path.Combine(Application.dataPath, rutaPersonalizada, nombreArchivo);
        Debug.Log("Ruta de origen del archivo: " + rutaArchivoOrigen);

        botonGuardar.onClick.AddListener(() => AbrirSelectorDeCarpetas());
    }

    private void AbrirSelectorDeCarpetas()
    {
        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
            {
                textoEstado.text = "Guardado cancelado";
                Debug.Log("Guardado cancelado por el usuario.");
                return;
            }

            GuardarPDFEnUbicacion(Path.GetDirectoryName(path));
        }, new string[] { });

        if (permission == NativeFilePicker.Permission.Denied)
        {
            textoEstado.text = "Permiso denegado";
            Debug.LogError("Permiso denegado para acceder al almacenamiento.");
        }
    }

    private void GuardarPDFEnUbicacion(string rutaDestino)
    {
        Debug.Log("Iniciando proceso de guardado...");
        Debug.Log("Ruta de destino seleccionada: " + rutaDestino);

        if (!string.IsNullOrEmpty(rutaDestino))
        {
            string rutaCompletaDestino = Path.Combine(rutaDestino, nombreArchivo);
            try
            {
                File.Copy(rutaArchivoOrigen, rutaCompletaDestino, true);
                textoEstado.text = "Archivo guardado en: " + rutaCompletaDestino;
                Debug.Log("Archivo guardado en: " + rutaCompletaDestino);
            }
            catch (IOException e)
            {
                textoEstado.text = "Error al guardar el archivo: " + e.Message;
                Debug.LogError("Error al guardar el archivo: " + e.Message);
            }
        }
        else
        {
            textoEstado.text = "Guardado cancelado";
            Debug.Log("Guardado cancelado por el usuario.");
        }
    }
}