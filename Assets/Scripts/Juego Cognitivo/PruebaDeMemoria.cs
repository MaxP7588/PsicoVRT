using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MemoryTest : MonoBehaviour
{
    public List<GameObject> objectsToHide; // Lista de objetos en la escena
    public GameObject curtain; // Cortina que se cierra y abre
    public float initialViewTime = 5.0f; // Tiempo que se muestran los objetos al principio
    public float curtainCloseTime = 2.0f; // Tiempo que la cortina permanece cerrada
    public TextMeshProUGUI nivelNoSeleccionadoTexto; // Objeto TextMeshPro para mostrar el mensaje
    public float fadeDuration = 2.0f; // Duración del desvanecimiento
    public GameObject player; // El objeto del jugador
    public Camera playerCamera; // La cámara del jugador

    private Vector3 initialPlayerPosition = new Vector3(-1.55f, 1.0f, 0.0f); // Posición inicial del jugador
    private Quaternion initialPlayerRotation = Quaternion.Euler(0f, -180f, 0f); // Rotación inicial del jugador
    private List<Vector3> posicionesOriginales; // Lista de la posición de cada objeto para devolverlo a su sitio
    private List<Quaternion> rotacionesOriginales; // Lista de la rotación de cada objeto para devolverlo a su sitio
    private List<GameObject> shownObjects; // Lista de objetos que se mostrarán inicialmente
    private GameObject newObject; // Nuevo objeto que aparecerá después
    private int numObjetos;
    private NivelJuegoMemoria memoria;
    private Coroutine fadeCoroutine;
    private PlayerController playerController; // Controlador del jugador

    void Start()
    {
        // Asegurarse de que la cortina está inicialmente abierta
        curtain.SetActive(false);
        shownObjects = new List<GameObject>();
        memoria = FindObjectOfType<NivelJuegoMemoria>();
        playerController = player.GetComponent<PlayerController>();

        posicionesOriginales = new List<Vector3>(); // Inicializar la lista de posiciones originales
        rotacionesOriginales = new List<Quaternion>(); // Inicializar la lista de rotaciones originales
        foreach (GameObject obj in objectsToHide)
        {
            posicionesOriginales.Add(obj.transform.position);
            rotacionesOriginales.Add(obj.transform.rotation);
        }

        // Asegurarse de que el mensaje de nivel no seleccionado esté inicialmente desactivado
        nivelNoSeleccionadoTexto.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            StartMemoryTest();
        }
    }

    public void StartMemoryTest()
    {
        numObjetos = memoria.elNivelEs();
        if (numObjetos != 0)
        {
            // Ocultar el mensaje de nivel no seleccionado
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            nivelNoSeleccionadoTexto.gameObject.SetActive(false);
            StartCoroutine(MemoryTestCoroutine());
        }
        else
        {
            Debug.Log("no ha seleccionado nivel");
            // Mostrar el mensaje de nivel no seleccionado
            nivelNoSeleccionadoTexto.text = "Seleccione nivel";
            nivelNoSeleccionadoTexto.color = new Color(nivelNoSeleccionadoTexto.color.r, nivelNoSeleccionadoTexto.color.g, nivelNoSeleccionadoTexto.color.b, 1f); // Asegurarse de que el texto esté completamente visible
            nivelNoSeleccionadoTexto.gameObject.SetActive(true);
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeText());
        }
    }

    private IEnumerator MemoryTestCoroutine()
    {
        // Desactivar el control del jugador
        playerController.enabled = false;

        // Teletransportar al jugador a la posición y rotación iniciales
        player.transform.position = initialPlayerPosition;
        playerCamera.transform.rotation = initialPlayerRotation;

        // Asegurarse de que la lista está vacía al inicio de cada prueba
        shownObjects.Clear();

        foreach (GameObject obj in objectsToHide)
        {
            obj.SetActive(false);
        }

        // Cerrar la cortina
        curtain.SetActive(true);
        yield return new WaitForSeconds(curtainCloseTime - 1);

        // Escoger objetos al azar para mostrar inicialmente
        List<int> selectedIndices = new List<int>();
        while (selectedIndices.Count < numObjetos)
        {
            int randomIndex = Random.Range(0, objectsToHide.Count);
            if (!selectedIndices.Contains(randomIndex))
            {
                selectedIndices.Add(randomIndex);
                shownObjects.Add(objectsToHide[randomIndex]);
            }
        }

        // Mostrar los objetos
        foreach (GameObject obj in shownObjects)
        {
            obj.SetActive(true);
        }

        curtain.SetActive(false);

        // Esperar para que el jugador vea los objetos
        yield return new WaitForSeconds(initialViewTime);

        // Cerrar la cortina
        curtain.SetActive(true);
        yield return new WaitForSeconds(curtainCloseTime);

        // Seleccionar un nuevo objeto que no esté en la lista de mostrados
        List<int> remainingIndices = new List<int>();
        for (int i = 0; i < objectsToHide.Count; i++)
        {
            if (!selectedIndices.Contains(i))
            {
                remainingIndices.Add(i);
            }
        }

        int newObjectIndex = remainingIndices[Random.Range(0, remainingIndices.Count)];
        newObject = objectsToHide[newObjectIndex];

        // Mostrar el nuevo objeto
        newObject.SetActive(true);

        // Abrir la cortina
        curtain.SetActive(false);

        // Reactivar el control del jugador
        playerController.enabled = true;
    }

    public void ShowInitialObjects()
    {
        // Mostrar todos los objetos que fueron ocultados inicialmente
        foreach (GameObject obj in shownObjects)
        {
            obj.SetActive(true);
        }
    }

    public bool CheckNewObject(GameObject selectedObject)
    {
        // Verificar si el objeto seleccionado es el nuevo objeto
        return selectedObject == newObject;
    }

    public GameObject getNewObj()
    {
        return newObject;
    }

    public void reiniciar()
    {
        for (int i = 0; i < objectsToHide.Count; i++)
        {
            // Regresar a su posición y rotación originales
            objectsToHide[i].transform.position = posicionesOriginales[i];
            objectsToHide[i].transform.rotation = rotacionesOriginales[i];
            objectsToHide[i].SetActive(false);
        }

        newObject = null;
        shownObjects.Clear();
    }

    private IEnumerator FadeText()
    {
        float elapsedTime = 0f;
        Color originalColor = nivelNoSeleccionadoTexto.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        while (elapsedTime < fadeDuration)
        {
            nivelNoSeleccionadoTexto.color = Color.Lerp(originalColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        nivelNoSeleccionadoTexto.color = targetColor;
        nivelNoSeleccionadoTexto.gameObject.SetActive(false);
    }
}
