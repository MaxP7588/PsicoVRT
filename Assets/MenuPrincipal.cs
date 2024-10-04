using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
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

    private IEnumerator CargarEscena(int sceneIndex)
    {
        Debug.Log("Cargando escena...");
        yield return null; // Puedes agregar una espera si es necesario
        Debug.Log("Cargando escena: " + sceneIndex);
        SceneManager.LoadScene(sceneIndex);
    }
}