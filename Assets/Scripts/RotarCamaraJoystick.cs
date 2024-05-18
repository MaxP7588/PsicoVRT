using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class RotarCamaraJoystick : MonoBehaviour
{
    // Velocidad de rotaci�n de la c�mara
    public float rotationSpeed = 2.0f;

    // Nombre del eje horizontal y vertical del joystick
    public string HorizontalDerecha = "HorizontalDerecha";


    void Start()
    {
    }

    void Update()
    {
        if(Input.GetAxis(HorizontalDerecha)==1 || Input.GetAxis(HorizontalDerecha) == -1)
        {
            // Obtiene la entrada horizontal y vertical del joystick
            float Horizontal = Input.GetAxis(HorizontalDerecha);

            // Calcula la rotaci�n basada en la entrada del joystick
            Vector3 rotation = new Vector3(0.0f, Horizontal, 0.0f) * rotationSpeed;

            transform.Rotate(rotation);
        }
        
    }
}
