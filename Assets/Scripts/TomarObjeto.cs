using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomarObjeto : MonoBehaviour
{
    public GameObject handPoint;
    private GameObject pickObject = null;

    public 

     void Update()
    {
        if (Input.GetKey(KeyCode.JoystickButton3))
        {
            pickObject.GetComponent<Rigidbody>().useGravity = true;
            pickObject.GetComponent<Rigidbody>().isKinematic = false;

            pickObject.gameObject.transform.SetParent(null);

            pickObject = null;
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.CompareTag("Objeto")){
            if(Input.GetKey(KeyCode.JoystickButton2) && pickObject == null){
                other.GetComponent<Rigidbody>().useGravity = false;
                other.GetComponent <Rigidbody>().isKinematic = true;

                other.transform.position = handPoint.transform.position;

                other.gameObject.transform.SetParent(handPoint.gameObject.transform);
                pickObject = other.gameObject;
            }
        }
    }
}

