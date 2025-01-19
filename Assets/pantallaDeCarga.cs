using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PantallaDeCarga : MonoBehaviour
{
    public GameObject pantallaDeCarga;
    public Slider barraDeProgreso; // Opcional, si quieres mostrar una barra de progreso
    public float velocidadCarga = 0.5f; // Velocidad de carga de la barra de progreso

    public void CargarEscena(int sceneIndex)
    {
        StartCoroutine(CargarEscenaAsync(sceneIndex));
    }

    private IEnumerator CargarEscenaAsync(int sceneIndex)
    {
        pantallaDeCarga.SetActive(true);

        AsyncOperation operacion = SceneManager.LoadSceneAsync(sceneIndex);
        operacion.allowSceneActivation = false; // Evitar que la escena se active automáticamente

        float progresoFalso = 0f;

        while (!operacion.isDone)
        {
            // Incrementar el progreso falso
            progresoFalso += Time.deltaTime * velocidadCarga;
            if (progresoFalso > 1f)
            {
                progresoFalso = 1f;
            }

            // Calcular el progreso real
            float progresoReal = Mathf.Clamp01(operacion.progress / 0.9f);

            // Usar el menor valor entre el progreso falso y el real
            float progreso = Mathf.Min(progresoFalso, progresoReal);

            if (barraDeProgreso != null)
            {
                barraDeProgreso.value = progreso;
            }

            // Activar la escena cuando el progreso sea completo
            if (progreso >= 1f)
            {
                operacion.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}