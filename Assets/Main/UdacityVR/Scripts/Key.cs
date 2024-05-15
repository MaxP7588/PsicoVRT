using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    public Boolean takeObjet;
    public GameObject key;
    public GameObject keyPoofPrefab;
    public Door door;

	void Update () {

        if (takeObjet)
        {
            Vector3 currentPosition = key.transform.position;
            currentPosition.y = 2 + Mathf.Sin(Time.time * 2.0f);
            key.transform.position = currentPosition;
        }
        
        
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

    public void OnKeyClicked () {

        
    }
}
