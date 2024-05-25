using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NivelJuegoMemoria : MonoBehaviour
{
    public GameObject botonFacil;
    public GameObject botonMedio;
    public GameObject botonDificil;

    private int nivel;

    public void cambioDificultad(GameObject botonSwitch)
    {
        if (botonSwitch.gameObject == botonFacil.gameObject) {
            botonMedio.gameObject.GetComponent<PresionarBoton>().apagar();
            botonDificil.gameObject.GetComponent<PresionarBoton>().apagar();
            nivel = 3;
        }
        else if (botonSwitch.gameObject == botonMedio.gameObject)
        {
            botonFacil.gameObject.GetComponent<PresionarBoton>().apagar();
            botonDificil.gameObject.GetComponent<PresionarBoton>().apagar();
            nivel = 5;
        }
        else if (botonSwitch.gameObject == botonDificil.gameObject)
        {
            botonFacil.gameObject.GetComponent<PresionarBoton>().apagar();
            botonMedio.gameObject.GetComponent<PresionarBoton>().apagar();
            nivel = 7;
        }
    }

    public int elNivelEs()
    {
        return nivel;
    }
}