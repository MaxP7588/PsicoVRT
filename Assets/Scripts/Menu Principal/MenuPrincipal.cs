using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{

    public void Start()
    {
        Debug.Log("Apagando VR...");
        VrModeController vrModeController = new VrModeController();
        vrModeController.ExitVR();
    }
    
    public void jugarTutorial()
    {
        Debug.Log("Iniciando tutorial...");
        StartCoroutine(CargarEscena(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void juegoCognitivo()
    {
        Debug.Log("Iniciando juego cognitivo...");
        StartCoroutine(CargarEscena(SceneManager.GetActiveScene().buildIndex + 2));
    }

    public void juegoFobiaAltura()
    {
        Debug.Log("Iniciando juego cognitivo...");
        StartCoroutine(CargarEscena(SceneManager.GetActiveScene().buildIndex + 3));
    }

    public void juegoRol()
    {
        Debug.Log("Iniciando juego de rol...");
        StartCoroutine(CargarEscena(SceneManager.GetActiveScene().buildIndex + 4));
    }

    private IEnumerator CargarEscena(int sceneIndex)
    {
        Debug.Log("Cargando escena...");
        yield return null; // Puedes agregar una espera si es necesario
        Debug.Log("Cargando escena: " + sceneIndex);
        SceneManager.LoadScene(sceneIndex);
    }
}