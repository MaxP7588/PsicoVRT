using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryTest : MonoBehaviour
{
    public List<GameObject> objectsToHide; // Lista de objetos en la escena
    public GameObject curtain; // Cortina que se cierra y abre
    public float initialViewTime = 5.0f; // Tiempo que se muestran los 3 objetos al principio
    public float curtainCloseTime = 2.0f; // Tiempo que la cortina permanece cerrada

    private List<GameObject> shownObjects; // Lista de objetos que se mostrarán inicialmente
    private GameObject newObject; // Nuevo objeto que aparecerá después

    void Start()
    {
        // Asegurarse de que la cortina está inicialmente abierta
        curtain.SetActive(false);
        shownObjects = new List<GameObject>();
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
        StartCoroutine(MemoryTestCoroutine());
    }

    private IEnumerator MemoryTestCoroutine()
    {
        // Asegurarse de que la lista está vacía al inicio de cada prueba
        shownObjects.Clear();

        foreach (GameObject obj in objectsToHide)
        {
            obj.SetActive(false);
        }

        // Cerrar la cortina
        curtain.SetActive(true);
        yield return new WaitForSeconds(curtainCloseTime-1);

        // Escoger 3 objetos al azar para mostrar inicialmente
        List<int> selectedIndices = new List<int>();
        while (selectedIndices.Count < 3)
        {
            int randomIndex = Random.Range(0, objectsToHide.Count);
            if (!selectedIndices.Contains(randomIndex))
            {
                selectedIndices.Add(randomIndex);
                shownObjects.Add(objectsToHide[randomIndex]);
            }
        }

        // Mostrar los 3 objetos
        foreach (GameObject obj in shownObjects)
        {
            obj.SetActive(true);
        }

        curtain.SetActive(false);

        // Esperar 5 segundos para que el jugador vea los objetos
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
}
