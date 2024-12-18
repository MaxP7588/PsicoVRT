using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PW
{
    public class HeatableProduct : ProductGameObject, IInteractable
    {
        Collider m_collider;
        private Vector3 initialPosition;
        public Microwave m_Machine;
        public GameObject platePrefab;
        public float heatingTimeForProduct;

        [Header("Configuración Input")]
        public KeyCode joystickButton = KeyCode.JoystickButton3;
        public float maxDistance = 8f;
        private Camera mainCamera;

        private void Awake()
        {
            m_collider = GetComponent<Collider>();
            m_collider.enabled = true;
        }

        private void OnEnable()
        {
            initialPosition = transform.position;
        }

        private void Start()
        {
            if (m_Machine == null)
                m_Machine = FindObjectOfType<Microwave>();
            
            mainCamera = Camera.main;
        }

        void Update()
        {

            // Verificar input de joystick
            if (Input.GetKeyDown(joystickButton))
            {
                CheckRaycastAndInteract();
            }
        }

        private void CheckRaycastAndInteract()
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

        void OnMouseDown()
        {
            ProcessInteraction();
        }

        public void ProcessInteraction()
        {
            if (m_Machine != null)
            {
                if (m_Machine.isEmpty)
                {
                    if (AddToPlateBeforeServed)
                    {
                        var plate = Instantiate(platePrefab, transform.position, Quaternion.identity);
                        plate.transform.SetParent(transform);
                    }

                    m_Machine.SetProduct(this, heatingTimeForProduct);
                    StartCoroutine(MoveToMicrowave());
                }
            }
        }

        IEnumerator MoveToMicrowave()
        {
            transform.position = m_Machine.beginEnteringSpot.position;
            yield return base.MoveToPlace(m_Machine.cookingSpot.position);
        }

        public override IEnumerator AnimateGoingToSlot()
        {
            if (RegenerateProduct)
            {
                if (AddToPlateBeforeServed)
                {
                    Destroy(transform.GetChild(0).gameObject);
                }
                else if (serveAsDifferentGameObject != null)
                {
                    Destroy(transform.GetChild(0).gameObject);
                }
                BasicGameEvents.RaiseInstantiatePlaceHolder(transform.parent, initialPosition, gameObject);
            }
            yield return base.AnimateGoingToSlot();
            gameObject.SetActive(false);
        }
    }
}