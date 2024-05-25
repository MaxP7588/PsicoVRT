using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresionarBoton : MonoBehaviour
{
    public GameObject boton;

    private bool encendido;
    private Transform botonTransform;
    private NivelJuegoMemoria nivelMemoriaScript;

    private void Start()
    {
        encendido = false;
        botonTransform = boton.GetComponent<Transform>();
        nivelMemoriaScript = FindObjectOfType<NivelJuegoMemoria>();
    }

    public void encender()
    {
        botonTransform.localScale = new Vector3(0.3f, botonTransform.localScale.y, botonTransform.localScale.z);
        encendido = true;
        nivelMemoriaScript.cambioDificultad(gameObject);
    }
    public void apagar()
    {
        botonTransform.localScale = new Vector3(0.5f, botonTransform.localScale.y, botonTransform.localScale.z);
        encendido = false;
    }

    public bool estaEncendido()
    {
        return encendido;
    }
}
