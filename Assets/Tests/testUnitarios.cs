using System.Collections;
using System;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using WebSocketSharp;

[TestFixture]
public class NewTestScriptTest 
{
    private GameObject testGameObject;
    private WebSocket webSocket;

    [SetUp]
    public void Setup()
    {
        testGameObject = new GameObject();
    }

    [TearDown]
    public void Teardown()
    {
        if (testGameObject != null)
        {
            UnityEngine.Object.DestroyImmediate(testGameObject);
        }
        if (webSocket != null && webSocket.IsAlive)
        {
            webSocket.Close();
        }
    }

    [UnityTest]
    public IEnumerator TestVRSceneLoading()
    {
        LogAssert.Expect(LogType.Exception, "NullReferenceException: Object reference not set to an instance of an object");
        
        // Arrange
        int vrSceneIndex = 2;
        
        // Act
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(vrSceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    
        yield return new WaitForSeconds(1);
    
        // Assert
        Scene currentScene = SceneManager.GetActiveScene();
        Assert.AreEqual(vrSceneIndex, currentScene.buildIndex, "La escena cargada no es la correcta");
    
        // Verificar componentes necesarios
        //var vrController = GameObject.FindObjectOfType<VrModeController>();
        //Assert.IsNotNull(vrController, "No se encontró VrModeController en la escena");
    
        var mainCamera = GameObject.FindObjectOfType<Camera>();
        Assert.IsNotNull(mainCamera, "No se encontró la cámara principal en la escena");
    
        // Verificar que la cámara tiene los componentes necesarios
        Assert.IsTrue(mainCamera.gameObject.activeInHierarchy, "La cámara principal está desactivada");
    }

    [Test]
    public void TestPasswordEncryption()
    {
        // Arrange
        string password = "TestPassword123";
        
        // Act
        string encryptedPassword1 = EncryptPassword(password);
        string encryptedPassword2 = EncryptPassword(password);
        
        // Assert
        Assert.IsNotNull(encryptedPassword1, "La contraseña encriptada no puede ser null");
        Assert.AreNotEqual(password, encryptedPassword1, "La contraseña no fue encriptada");
        Assert.AreEqual(encryptedPassword1, encryptedPassword2, "La encriptación no es consistente");
    }

    private string EncryptPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

    [UnityTest]
    public IEnumerator TestWebSocketConnection()
    {
        // Arrange
        webSocket = new WebSocket("ws://pacheco.chillan.ubiobio.cl:124/");
        bool isConnected = false;
        
        webSocket.OnOpen += (sender, e) => isConnected = true;

        // Act
        webSocket.Connect();
        
        // Esperar hasta 5 segundos por la conexión
        float timeout = 5f;
        float elapsed = 0f;
        while (!isConnected && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Assert
        Assert.IsTrue(isConnected, "No se pudo conectar al WebSocket");
        Assert.IsTrue(webSocket.IsAlive, "La conexión WebSocket no está activa");
    }
}