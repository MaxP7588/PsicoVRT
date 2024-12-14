using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using WebSocketSharp;
using System;

public class WebRTCManager : MonoBehaviour
{
    private WebSocket webSocket;
    private RenderTexture renderTexture;
    [SerializeField] private RawImage previewImage; // Hacer visible en el inspector
    [SerializeField] private Camera cam; // Hacer visible en el inspector
    
    // Parámetros ajustables para rendimiento
    [SerializeField] private int textureWidth = 720;  // Reducido de 720
    [SerializeField] private int textureHeight = 480; // Reducido de 480
    [SerializeField] private int jpgQuality = 10;     // Reducido de 75
    [SerializeField] private float frameDelay = 0.1f; // ~15 FPS (reducido de 30 FPS)
    
    private float lastFrameTime;
    private int framesSentInLastSecond = 0;
    private float lastPerformanceCheck = 0;
    
    void Start()
    {
        // Inicializar WebSocket
        webSocket = new WebSocket("ws://pacheco.chillan.ubiobio.cl:124");
        webSocket.OnMessage += OnWebSocketMessage;
        webSocket.Connect();
        
        // Inicializar RenderTexture con menor resolución
        renderTexture = new RenderTexture(textureWidth, textureHeight, 24); 
        cam.targetTexture = renderTexture;
        previewImage.texture = renderTexture;
        
        StartCoroutine(SendVideoFrame());
    }
    
    IEnumerator SendVideoFrame()
    {
        while (true)
        {
            if (webSocket.IsAlive && Time.time - lastFrameTime >= frameDelay)
            {
                lastFrameTime = Time.time;
                framesSentInLastSecond++;
                
                // Monitorear rendimiento cada segundo
                if (Time.time - lastPerformanceCheck >= 1f)
                {
                    //AdjustQualityBasedOnPerformance();
                    framesSentInLastSecond = 0;
                    lastPerformanceCheck = Time.time;
                }
                
                // Convertir el RenderTexture a Texture2D
                Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
                RenderTexture.active = renderTexture;
                tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                tex.Apply();
                RenderTexture.active = null;
                
                // Convertir el frame a bytes con menor calidad
                byte[] bytes = tex.EncodeToJPG(jpgQuality); // Ajustar la calidad de la compresión JPG
                
                // Enviar frame por WebSocket
                string base64 = Convert.ToBase64String(bytes);
                webSocket.Send("{\"type\": \"video\", \"data\": \"" + base64 + "\"}");
                
                Destroy(tex);
            }
            yield return null;
        }
    }
    
    private void AdjustQualityBasedOnPerformance()
    {
        float fps = 1.0f / Time.deltaTime;
        
        // Si FPS < 20, reducir calidad
        if (fps < 20)
        {
            jpgQuality = Mathf.Max(30, jpgQuality - 5);
            frameDelay = Mathf.Min(0.1f, frameDelay + 0.01f);
        }
        // Si FPS > 40, podemos aumentar calidad
        else if (fps > 40)
        {
            jpgQuality = Mathf.Min(75, jpgQuality + 5);
            frameDelay = Mathf.Max(0.033f, frameDelay - 0.01f);
        }
        
        Debug.Log($"FPS: {fps}, Quality: {jpgQuality}, Delay: {frameDelay}");
    }
    
    void OnWebSocketMessage(object sender, MessageEventArgs e)
    {
        // Manejar mensajes del servidor si es necesario
        Debug.Log("Mensaje recibido: " + e.Data);
    }
    
    void OnDestroy()
    {
        if (webSocket != null)
            webSocket.Close();
        if (renderTexture != null)
            renderTexture.Release();
    }
}