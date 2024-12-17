using UnityEngine;

public class ConfiguracionCanvasVR : MonoBehaviour 
{
    [Header("Referencias")]
    [SerializeField] private Transform objetivoCanvas;
    
    [Header("Configuración")]
    [SerializeField] private float posicionX = 0f;
    [SerializeField] private float posicionY = 0f;
    [SerializeField] private float posicionZ = 4f;
    [SerializeField] private float escalaX = 0.001f;
    [SerializeField] private float escalaY = 0.001f;

    
    private Canvas canvas;

    void Start()
    {
        canvas = GetComponent<Canvas>();
        if (canvas != null && objetivoCanvas != null)
        {
            canvas.renderMode = RenderMode.WorldSpace;
            transform.localScale = new Vector3(escalaX, escalaY, 0.001f);
            
            // Posición específica
            transform.localPosition = new Vector3(posicionX, posicionY, posicionZ);
            transform.rotation = objetivoCanvas.rotation;
        }
    }

    void Update(){
        transform.localPosition = new Vector3(posicionX, posicionY, posicionZ);
        transform.localScale = new Vector3(escalaX, escalaY, 0.001f);
    }
}