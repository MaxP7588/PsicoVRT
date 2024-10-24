using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    public Boolean takeObjet;
    public GameObject key;
    public GameObject keyPoofPrefab;
    public Door door;
    public GameObject puntoKey;
    public Animator anim;

	void Update () {

        

    }

    public void takeKey()
    {
        takeObjet = true;
        anim.SetBool("isTake", takeObjet);
    }
    public void untakeKey()
    {
        takeObjet = false;
        anim.SetBool("isTake", takeObjet);
    }

    private void OnTriggerStay(Collider other)
    {
        // Verifica si el objeto con el que colisiona tiene el tag "TargetTag"
        if (other.gameObject.CompareTag("DoorKey"))
        {
            Debug.Log("La llave ha colisionado con el objeto objetivo.");
            door.Unlock();
            Instantiate(keyPoofPrefab, key.transform.position, Quaternion.identity);
            Destroy(key);
        }
    }

    public void verPunto()
    {
        puntoKey.SetActive(true);
    }

    public void ocultarPunto()
    {
        puntoKey.SetActive(false);
    }
}
