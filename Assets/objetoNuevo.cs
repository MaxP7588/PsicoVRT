using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objetoNuevo : MonoBehaviour
{
    private MemoryTest memoryTest;
    private TomarObjeto tomarObjeto;

    void Start()
    {
        // Inicializar memoryTest con el componente en la escena
        memoryTest = FindObjectOfType<MemoryTest>();
        tomarObjeto = FindAnyObjectByType<TomarObjeto>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!tomarObjeto.hayObjetoTomado())
        {
            if (other.gameObject.CompareTag("Objeto") )
            {
                // Verificar si el objeto en el trigger es el nuevo objeto
                if (memoryTest.getNewObj().gameObject == other.gameObject)
                {
                    other.gameObject.SetActive(false);
                    Debug.Log("¡Encontraste el nuevo objeto!");
                }
                else
                {
                    Debug.Log("Este no es el nuevo objeto.");
                }
            }
        }
    }
}
