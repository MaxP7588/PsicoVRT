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
    
    void Start()
    {
        // Inicializar WebSocket
        webSocket = new WebSocket("ws://pacheco.chillan.ubiobio.cl:124");
        webSocket.OnMessage += OnWebSocketMessage;
        webSocket.Connect();
        
        // Inicializar RenderTexture con mayor resolución
        renderTexture = new RenderTexture(720, 480, 24); 
        cam.targetTexture = renderTexture;
        previewImage.texture = renderTexture;
        
        StartCoroutine(SendVideoFrame());
    }
    
    IEnumerator SendVideoFrame()
    {
        while (true)
        {
            if (webSocket.IsAlive)
            {
                // Convertir el RenderTexture a Texture2D
                Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
                RenderTexture.active = renderTexture;
                tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                tex.Apply();
                RenderTexture.active = null;
                
                // Convertir el frame a bytes con mayor calidad
                byte[] bytes = tex.EncodeToJPG(75); // Ajustar la calidad de la compresión JPG al máximo (100)
                
                // Enviar frame por WebSocket
                string base64 = Convert.ToBase64String(bytes);
                webSocket.Send("{\"type\": \"video\", \"data\": \"" + base64 + "\"}");
                
                Destroy(tex);
            }
            yield return new WaitForSeconds(0.033f); // ~30 FPS
        }
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