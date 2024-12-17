using UnityEngine;
using PW;

public class EntregarOrden : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Buscar todas las órdenes activas
            ServeOrder[] ordenesActivas = FindObjectsOfType<ServeOrder>();
            
            // Verificar cada orden
            foreach (ServeOrder orden in ordenesActivas)
            {
                orden.ServeMe(); // ServeMe() ya verifica si el jugador tiene el item correcto
            }
        }
    }
}