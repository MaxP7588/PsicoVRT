using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PW
{
    public class CreateProductOnPlaceHolder : MonoBehaviour, IInteractable
    {
        //This is set when the placeholder is created,
        //so we know what to generate
        public GameObject objectToGenerate;

        [Header("Configuración Input")]
        public KeyCode joystickButton = KeyCode.JoystickButton3;
        public float maxDistance = 8f;
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {

            // Verificar input de joystick
            if (Input.GetKeyDown(joystickButton))
            {
                CheckRaycastAndCreate();
            }
        }

        private void CheckRaycastAndCreate()
        {
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    ProcessInteraction();
                }
            }
        }

        private void OnMouseDown()
        {
            ProcessInteraction();
        }

        public void ProcessInteraction()
        {
            var go = GameObject.Instantiate(objectToGenerate, transform.parent);
            Destroy(objectToGenerate);
            go.transform.position = transform.position;

            var m_collider = go.GetComponent<Collider>();
            m_collider.enabled = true;

            go.SetActive(true);

            //Remove the plate first if we have one.
            var productGO = go.GetComponent<ProductGameObject>();
            if (productGO.AddToPlateBeforeServed)
            {
                Destroy(go.transform.GetChild(0).gameObject);
            }

            Destroy(gameObject);
        }
    }
}