using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    public Transform cameraTransform; // Referencia a la transformación de la cámara
    private CharacterController characterController;
    private Vector3 velocity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController no encontrado");
        }
        if (cameraTransform == null)
        {
            Debug.LogError("cameraTransform no asignado");
        }
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

        Vector3 movement = (forward * verticalInput + right * horizontalInput) * speed;

        // Movimiento normal
        characterController.Move(movement * Time.deltaTime);

        // Aplicar gravedad manualmente
        if (!characterController.isGrounded)
        {
            velocity.y += Physics.gravity.y * Time.deltaTime;
        }
        else if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Valor pequeño negativo para asegurar que el CharacterController esté pegado al suelo
        }

        characterController.Move(velocity * Time.deltaTime);
    }
}