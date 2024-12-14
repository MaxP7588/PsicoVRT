using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    private MeshRenderer miniMapRenderer; // Referencia al MeshRenderer del minimapa
    private bool isMiniMapVisible = true; // Variable para rastrear la visibilidad del minimapa

    void Start()
    {
        miniMapRenderer = GetComponent<MeshRenderer>();
        if (miniMapRenderer == null)
        {
            Debug.LogError("MeshRenderer no encontrado en el objeto del minimapa.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            isMiniMapVisible = !isMiniMapVisible; // Alternar la visibilidad del minimapa
            if (miniMapRenderer != null)
            {
                miniMapRenderer.enabled = isMiniMapVisible; // Activar/Desactivar el MeshRenderer
            }
        }
    }
}
