using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomarObjeto : MonoBehaviour
{
    public GameObject handPoint;
    private GameObject pickObject = null;
    public Camera cam;
    public float maxDistance = 8.0f; // La distancia m�xima del rayo
    private bool buttonPressed = false;

    void Update()
    {
        // Dibujar la l�nea del raycast en la escena
        Vector3 rayOrigin = cam.transform.position;
        Vector3 rayDirection = cam.transform.forward * maxDistance;
        Debug.DrawLine(rayOrigin, rayOrigin + rayDirection, Color.red);

        if (Input.GetKey(KeyCode.JoystickButton3)) // SOLTAR UN OBJETO
        {
            if (pickObject != null)
            {
                pickObject.GetComponent<Rigidbody>().useGravity = true;
                pickObject.GetComponent<Rigidbody>().isKinematic = false;

                pickObject.gameObject.transform.SetParent(null);
                if (pickObject.name == "Key")
                {
                    pickObject.gameObject.GetComponent<Key>().takeObjet = true;
                    pickObject.transform.rotation = Quaternion.Euler(-90, 90, 0);
                    pickObject.gameObject.GetComponent<Key>().verPunto();
                }

                pickObject = null;
            }
        }

        if (Input.GetKey(KeyCode.JoystickButton2)) // TOMAR UN OBJETO
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.CompareTag("Objeto"))
                {
                    if (pickObject == null)
                    {
                        Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                        rb.useGravity = false;
                        rb.isKinematic = true;

                        hit.collider.transform.position = handPoint.transform.position;
                        hit.collider.transform.SetParent(handPoint.transform);

                        if (hit.collider.name == "Key")
                        {
                            hit.collider.GetComponent<Key>().takeObjet = false;
                            Vector3 direction = cam.transform.forward;
                            direction.y = 0;
                            Quaternion targetRotation = Quaternion.LookRotation(direction);
                            hit.collider.transform.rotation = targetRotation * Quaternion.Euler(180, 0, 0);
                            hit.collider.GetComponent<Key>().ocultarPunto();
                        }

                        pickObject = hit.collider.gameObject;
                    }else{
                        Debug.Log("Ya tienes un objeto en la mano");
                    }
                }
                else if (hit.collider.CompareTag("Boton"))
                {
                    if (!buttonPressed)
                    {
                        buttonPressed = true;
                        hit.collider.GetComponent<PresionarBoton>().encender();
                    }
                }
            }
        }
        else
        {
            buttonPressed = false;
        }
    }

    public bool hayObjetoTomado()
    {
        return pickObject != null;
    }
}
