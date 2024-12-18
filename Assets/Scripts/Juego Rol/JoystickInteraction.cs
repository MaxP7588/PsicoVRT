using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoystickInteraction : MonoBehaviour
{
    public Camera cam;
    public float maxDistance = 8.0f; // La distancia máxima del rayo
    public KeyCode joystickButton = KeyCode.JoystickButton3;

    void Update()
    {
        // Dibujar la línea del raycast en la escena
        Vector3 rayOrigin = cam.transform.position;
        Vector3 rayDirection = cam.transform.forward * maxDistance;
        Debug.DrawLine(rayOrigin, rayOrigin + rayDirection, Color.red);

        // Verificar input de joystick
        if (Input.GetKeyDown(joystickButton))
        {
            CheckRaycastAndInteract();
        }
        
    }

    private void CheckRaycastAndInteract()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            var interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.ProcessInteraction();
            }
        }
    }
}

public interface IInteractable
{
    void ProcessInteraction();
}