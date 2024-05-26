using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoCorrecto : MonoBehaviour
{
    private MemoryTest memoryTest;
    private TomarObjeto tomarObjeto;
    private textoEnPantalla text;

    void Start()
    {
        // Inicializar memoryTest con el componente en la escena
        memoryTest = FindObjectOfType<MemoryTest>();
        tomarObjeto = FindAnyObjectByType<TomarObjeto>();
        text = FindAnyObjectByType<textoEnPantalla>();
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
                    text.setTextoPantalla("¡Encontraste el objeto!");
                    memoryTest.reiniciar();
                }
                else
                {
                    text.setTextoPantalla("¡Ese no es el objeto!");
                    memoryTest.reiniciar();
                }
            }
        }
    }
}
