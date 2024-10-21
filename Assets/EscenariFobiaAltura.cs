using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscenariFobiaAltura : MonoBehaviour
{
    public Transform puertaD; // Referencia a la puerta derecha
    public Transform puertaI; // Referencia a la puerta izquierda
    public float speed = 2.0f; // Velocidad a la que sube el elevador
    public float maxHeight = 37.0f; // Altura máxima a la que sube el elevador
    private bool isMoving = false; // Indica si el elevador está en movimiento
    private bool isDoorClosed = false; // Indica si las puertas del elevador están cerradas

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            // Mover el elevador hacia arriba
            transform.position += Vector3.up * speed * Time.deltaTime;

            // Detener el elevador cuando alcance la altura máxima
            if (transform.position.y >= maxHeight)
            {
                OpenDoors();
                isMoving = false;
                transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z); // Asegurarse de que la posición sea exactamente maxHeight
                Debug.Log("Elevador ha alcanzado la altura máxima.");
            }
        }
    }

    // Detectar colisión con el jugador
    void OnTriggerEnter(Collider other)
    {
        // Imprimir en consola el nombre del objeto con el que colisiona
        Debug.Log("Colisión detectada con: " + other.name);
        if (other.CompareTag("Player") && !isDoorClosed)
        {
            Debug.Log("Jugador detectado, iniciando movimiento del elevador.");
            StartCoroutine(StartElevator());
        }
    }

    private IEnumerator StartElevator()
    {
        // Esperar un segundo
        yield return new WaitForSeconds(1);
        isMoving = true;
        CloseDoors();
    }

    // Método para cerrar las puertas del elevador
    void CloseDoors()
    {
        if (puertaD != null && puertaI != null)
        {
            // Cerrar las puertas del elevador
            puertaD.position += Vector3.right * 0.5f; // Mover la puerta derecha
            puertaI.position -= Vector3.right * 0.5f; // Mover la puerta izquierda
            isDoorClosed = true;
            Debug.Log("Puertas del elevador cerradas.");
        }
    }

    // Método para abrir las puertas del elevador
    void OpenDoors()
    {
        if (puertaD != null && puertaI != null)
        {
            // Abrir las puertas del elevador
            puertaD.position -= Vector3.right * 0.5f; // Mover la puerta derecha
            puertaI.position += Vector3.right * 0.5f; // Mover la puerta izquierda
            isDoorClosed = false;
            Debug.Log("Puertas del elevador abiertas.");
        }
    }
}