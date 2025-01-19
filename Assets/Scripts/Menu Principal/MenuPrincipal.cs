using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public PantallaDeCarga pantallaDeCarga;

    public void Start()
    {
        Debug.Log("Apagando VR...");
        VrModeController vrModeController = new VrModeController();
        vrModeController.ExitVR();
    }
    
    public void jugarTutorial()
    {
        Debug.Log("Iniciando tutorial...");
        pantallaDeCarga.CargarEscena(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void juegoCognitivo()
    {
        Debug.Log("Iniciando juego cognitivo...");
        pantallaDeCarga.CargarEscena(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void juegoFobiaAltura()
    {
        Debug.Log("Iniciando juego cognitivo...");
        pantallaDeCarga.CargarEscena(SceneManager.GetActiveScene().buildIndex + 3);
    }

    public void juegoRol()
    {
        Debug.Log("Iniciando juego de rol...");
        pantallaDeCarga.CargarEscena(SceneManager.GetActiveScene().buildIndex + 4);
    }

    public void calibracion()
    {
        Debug.Log("Iniciando calibracion...");
        pantallaDeCarga.CargarEscena(SceneManager.GetActiveScene().buildIndex + 5);
    }
}