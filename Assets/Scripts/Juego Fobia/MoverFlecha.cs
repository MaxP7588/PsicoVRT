using UnityEngine;

public class MoverFlecha : MonoBehaviour
{
    [Header("Configuración Movimiento")]
    [SerializeField] private float amplitud = 0.5f;    // Distancia de movimiento
    [SerializeField] private float velocidad = 2.0f;   // Velocidad de oscilación
    
    private Vector3 posicionInicial;
    private float tiempoTranscurrido;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        tiempoTranscurrido += Time.deltaTime;
        float desplazamientoY = Mathf.Sin(tiempoTranscurrido * velocidad) * amplitud;
        transform.position = posicionInicial + new Vector3(0, desplazamientoY, 0);
    }
}