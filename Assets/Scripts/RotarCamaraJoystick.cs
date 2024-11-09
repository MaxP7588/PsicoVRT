using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotarCamaraJoystick : MonoBehaviour
{
    // Sensibilidad de la rotación de la cámara
    public float mouseSensitivity = 100.0f;

    // Referencia al transform del jugador para rotar el cuerpo junto con la cámara
    public Transform playerBody;

    private float xRotation = 0.0f;

    void Start()
    {
        // Ocultar y bloquear el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Obtener la entrada del mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Calcular la rotación en el eje X (vertical)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limitar la rotación vertical

        // Aplicar la rotación en el eje X (vertical)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Aplicar la rotación en el eje Y (horizontal) al cuerpo del jugador
        playerBody.Rotate(Vector3.up * mouseX);
    }
}