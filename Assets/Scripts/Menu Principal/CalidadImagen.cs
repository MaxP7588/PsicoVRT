using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CalidadImagen : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Image imagenCalidad;
    [SerializeField] private TMP_Dropdown dropdownCalidad;

    [Header("Sprites de Calidad")]
    [SerializeField] private Sprite calidadBaja;    // 720p
    [SerializeField] private Sprite calidadMedia;   // 1080p
    [SerializeField] private Sprite calidadAlta;    // 2K

    void Start()
    {
        // Asignar listener al dropdown
        dropdownCalidad.onValueChanged.AddListener(CambiarCalidadImagen);
        
        // Cargar preferencia guardada
        int calidadGuardada = PlayerPrefs.GetInt("CalidadGrafica", 0);
        dropdownCalidad.value = calidadGuardada;
        CambiarCalidadImagen(calidadGuardada);
    }

    public void CambiarCalidadImagen(int indice)
    {
        switch (indice)
        {
            case 0: // 720p
                imagenCalidad.sprite = calidadBaja;
                break;
            case 1: // 1080p
                imagenCalidad.sprite = calidadMedia;
                break;
            case 2: // 2K
                imagenCalidad.sprite = calidadAlta;
                break;
        }
        
        // Guardar preferencia
        PlayerPrefs.SetInt("CalidadGrafica", indice);
        PlayerPrefs.Save();
    }
}