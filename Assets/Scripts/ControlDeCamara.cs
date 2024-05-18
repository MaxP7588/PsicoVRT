using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 8f;
    public Transform cameraTransform; // Referencia a la transformación de la cámara
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Asegúrate de que el Rigidbody no sea kinemático
        rb.isKinematic = false;
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

        // Aplicar el movimiento usando MovePosition
        rb.MovePosition(transform.position + movement);
    }
}
