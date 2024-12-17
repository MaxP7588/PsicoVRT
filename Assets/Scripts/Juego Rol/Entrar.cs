using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Entrar : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Image panelNegro;
    [SerializeField] private Transform puntoDestino;
    [SerializeField] private AudioSource locutor;
    [SerializeField] private CharacterController playerController; // Agregar referencia
    
    [Header("Voces")]
    [SerializeField] private AudioClip bienvenidaClip;
    [SerializeField] private AudioClip instruccionesClip;
    
    [Header("Configuración")]
    [SerializeField] private float duracionFade = 1f;
    [SerializeField] private float tiempoEspera = 0.5f;

    [Header("Game")]
    [SerializeField] private GameObject game ;
    [SerializeField] private GameObject visual ;
    [SerializeField] private GameObject city ;
    [SerializeField] private GameObject flecha ;

    public void OscurecerPantalla()
    {
        StartCoroutine(FadeToBlack());
        game.SetActive(false);
        visual.SetActive(false);
        
    }

    private IEnumerator FadeToBlack()
    {
        float tiempoTranscurrido = 0;
        while (tiempoTranscurrido < duracionFade)
        {
            tiempoTranscurrido += Time.deltaTime;
            float alfa = Mathf.Lerp(0, 1, tiempoTranscurrido / duracionFade);
            panelNegro.color = new Color(0, 0, 0, alfa);
            yield return null;
        }
    }

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

        // Congela al jugador
        playerController = jugador.GetComponent<CharacterController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Fade out
        tiempoTranscurrido = 0;
        while (tiempoTranscurrido < duracionFade)
        {
            tiempoTranscurrido += Time.deltaTime;
            float alfa = Mathf.Lerp(1, 0, tiempoTranscurrido / duracionFade);
            panelNegro.color = new Color(0, 0, 0, alfa);
            yield return null;
        }

        // Descativar flecha
        flecha.SetActive(false);

        // Reproducir audios
        if (locutor != null && bienvenidaClip != null )//false
        {
            locutor.clip = bienvenidaClip;
            locutor.Play();
            yield return new WaitForSeconds(bienvenidaClip.length + 1f);

            locutor.clip = instruccionesClip;
            locutor.Play();
            yield return new WaitForSeconds(instruccionesClip.length);
        }

        // Descongela al jugador
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        // Empezar el juego
        game.SetActive(true);
        visual.SetActive(true);
        city.SetActive(false);
    }
}