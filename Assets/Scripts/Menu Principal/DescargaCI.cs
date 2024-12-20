using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DescargaCI : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private string pdfUrl = "https://drive.usercontent.google.com/u/2/uc?id=1F0EP7075qd95OebdToq1WIY9BGV7pZCh&export=download";
    
    [Header("Referencias UI")]
    [SerializeField] private Button botonDescarga;
    [SerializeField] private TMP_Text textoEstado;

    private void Start()
    {
        botonDescarga.onClick.AddListener(() => RedirigirAlNavegador());
    }

    private void RedirigirAlNavegador()
    {
        textoEstado.text = "Redirigiendo al navegador...";
        Application.OpenURL(pdfUrl);
    }
}