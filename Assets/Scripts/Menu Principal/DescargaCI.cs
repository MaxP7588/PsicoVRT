using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using TMPro;

public class DescargaCI : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private string pdfUrl = "https://drive.google.com/file/d/1F0EP7075qd95OebdToq1WIY9BGV7pZCh/view?usp=drive_link";
    [SerializeField] private string nombreArchivo = "consentimiento_informado.pdf";
    
    [Header("Referencias UI")]
    [SerializeField] private Button botonDescarga;
    [SerializeField] private TMP_Text textoEstado;
    [SerializeField] private GameObject panelDescarga;

    private void Start()
    {
        botonDescarga.onClick.AddListener(() => StartCoroutine(DescargarPDF()));
    }

    private IEnumerator DescargarPDF()
    {
        panelDescarga.SetActive(true);
        textoEstado.text = "Descargando...";
        botonDescarga.interactable = false;

        using (UnityWebRequest www = UnityWebRequest.Get(pdfUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error al descargar: {www.error}");
                textoEstado.text = "Error en la descarga";
            }
            else
            {
                string path = Path.Combine(Application.persistentDataPath, nombreArchivo);
                File.WriteAllBytes(path, www.downloadHandler.data);
                
                Debug.Log($"PDF descargado en: {path}");
                textoEstado.text = "Descarga completada";
                
                // Abrir el archivo
                Application.OpenURL("file://" + path);
            }
        }

        panelDescarga.SetActive(false);
        botonDescarga.interactable = true;
    }
}