using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class textoEnPantalla : MonoBehaviour
{
    public TextMeshProUGUI textoPantalla; // Objeto TextMeshPro para mostrar el mensaje
    public float fadeDuration = 2.0f; // Duración del desvanecimiento
    private Coroutine fadeCoroutine;


    void Start()
    {
        textoPantalla.gameObject.SetActive(false);
    }

    public void setTextoPantalla(string texto)
    {
        textoPantalla.text = texto;
        textoPantalla.color = new Color(textoPantalla.color.r, textoPantalla.color.g, textoPantalla.color.b, 1f); // Asegurarse de que el texto esté completamente visible
        textoPantalla.gameObject.SetActive(true);
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeText());

    }

    private IEnumerator FadeText()
    {
        float elapsedTime = 0f;
        Color originalColor = textoPantalla.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        while (elapsedTime < fadeDuration)
        {
            textoPantalla.color = Color.Lerp(originalColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textoPantalla.color = targetColor;
        textoPantalla.gameObject.SetActive(false);
    }
}
