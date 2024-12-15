using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Entrar : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Image panelNegro;
    [SerializeField] private Transform puntoDestino;
    
    [Header("Configuración")]
    [SerializeField] private float duracionFade = 1f;
    [SerializeField] private float tiempoEspera = 0.5f;

    [Header("Game")]
    [SerializeField] private GameObject game ;
    [SerializeField] private GameObject visual ;
    [SerializeField] private GameObject city ;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entrar");
            StartCoroutine(RealizarTransicion(other.gameObject));
        }
    }

    private IEnumerator RealizarTransicion(GameObject jugador)
    {
        // Fade a negro
        float tiempoTranscurrido = 0;
        while (tiempoTranscurrido < duracionFade)
        {
            tiempoTranscurrido += Time.deltaTime;
            float alfa = Mathf.Lerp(0, 1, tiempoTranscurrido / duracionFade);
            panelNegro.color = new Color(0, 0, 0, alfa);
            yield return null;
        }

        // Esperar un momento
        yield return new WaitForSeconds(tiempoEspera);

        // Mover jugador
        jugador.transform.position = puntoDestino.position;
        jugador.transform.rotation = puntoDestino.rotation;

        // Fade out
        tiempoTranscurrido = 0;
        while (tiempoTranscurrido < duracionFade)
        {
            tiempoTranscurrido += Time.deltaTime;
            float alfa = Mathf.Lerp(1, 0, tiempoTranscurrido / duracionFade);
            panelNegro.color = new Color(0, 0, 0, alfa);
            yield return null;
        }

        //Empezar el juego
        game.SetActive(true);
        visual.SetActive(true);
        city.SetActive(false);
    }
}