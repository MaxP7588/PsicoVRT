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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving && transform.position.y <= maxHeight)
        {
            // Mover el elevador hacia arriba
            transform.position += Vector3.up * speed * Time.deltaTime;
            Debug.Log("Elevador en movimiento. Altura actual: " + transform.position.y);

            // Detener el elevador cuando alcance la altura máxima
            if (transform.position.y >= maxHeight)
            {
                isMoving = false;
                Debug.Log("Elevador ha alcanzado la altura máxima.");
            }
        }
    }

    // Detectar colisión con el jugador
    void OnTriggerEnter(Collider other)
    {
        // Imprimir en consola el nombre del objeto con el que colisiona
        Debug.Log("Colisión detectada con: " + other.name);
            Debug.Log("Jugador detectado, iniciando movimiento del elevador.");
            //esperar 1 segundo
            StartCoroutine(StartElevator());
            
            
    }

    private IEnumerator StartElevator()
    {
        //esperar un segundo
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
            // sumar al eje x +1.5f en puerta D
            puertaD.position += Vector3.right * 0.5f;
            // restar al eje x -1.5f en puerta I
            puertaI.position -= Vector3.right * 0.5f;

            Debug.Log("Puertas del elevador cerradas.");
        }
    }
}