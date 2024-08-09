using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    public float climbSpeed = 2f; // Velocidad de subida de escaleras
    public Transform cameraTransform; // Referencia a la transformación de la cámara
    private CharacterController characterController;
    private bool isClimbing = false; // Indica si el personaje está subiendo una escalera
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

        if (isClimbing)
        {
            // Movimiento en escalera
            Vector3 climbMovement = new Vector3(horizontalInput, verticalInput, 0) * climbSpeed;
            characterController.Move(climbMovement * Time.deltaTime);
        }
        else
        {
            // Movimiento normal
            characterController.Move(movement * Time.deltaTime);
        }

        // Aplicar gravedad manualmente
        if (!characterController.isGrounded && !isClimbing)
        {
            velocity.y += Physics.gravity.y * Time.deltaTime;
        }
        else if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Valor pequeño negativo para asegurar que el CharacterController esté pegado al suelo
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Escalera"))
        {
            isClimbing = true;
            velocity = Vector3.zero; // Reiniciar la velocidad cuando empieza a escalar
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
