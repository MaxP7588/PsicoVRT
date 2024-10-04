using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuegoCognitivo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //iniciar vr
        Debug.Log("Iniciando VR...");
        VrModeController vrModeController = new VrModeController();
        vrModeController.EnterVR();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
