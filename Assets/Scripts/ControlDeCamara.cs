using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 3f;
    public float climbSpeed = 2f; // Velocidad de subida de escaleras
    public Transform cameraTransform; // Referencia a la transformación de la cámara
    private Rigidbody rb;
    private bool isClimbing = false; // Indica si el personaje está subiendo una escalera

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Obtener la dirección de movimiento basada en la rotación de la cámara
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 movement = (forward * verticalInput + right * horizontalInput) * speed * Time.deltaTime;

        if (isClimbing)
        {
            // Movimiento en escalera
            Vector3 climbMovement = new Vector3(horizontalInput, verticalInput, 0) * climbSpeed * Time.deltaTime;
            rb.MovePosition(transform.position + climbMovement);
            rb.useGravity = false; // Desactiva la gravedad mientras sube la escalera
        }
        else
        {
            // Movimiento normal
            rb.MovePosition(transform.position + movement);
            rb.useGravity = true; // Asegura que la gravedad esté activa cuando no está en una escalera
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Escalera"))
        {
            isClimbing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Escalera"))
        {
            isClimbing = false;
        }
    }
}
