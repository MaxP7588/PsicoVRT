﻿using UnityEngine;
using System.Collections;

namespace PW
{
    public class Microwave : MonoBehaviour, IInteractable
    {
        HeatableProduct currentProduct; // The product we currently heating inside
        bool doorIsOpen;

        public Transform door;
        public Transform beginEnteringSpot;
        public Transform cookingSpot;

        public GameObject progressHelperprefab;

        ProgressHelper m_progressHelper;

        float heatingProcess;//how much time 

        [Header("Configuración Input")]
        public KeyCode joystickButton = KeyCode.JoystickButton3;
        public float maxDistance = 8f;
        private Camera mainCamera;

        public bool isEmpty
        {
            get
            {
                return !doorIsOpen && currentProduct == null;
            }
        }

        private void Start()
        {
            //Instantiate and set the UI indicator
            if (progressHelperprefab != null)
            {
                m_progressHelper = Instantiate(progressHelperprefab, transform).GetComponent<ProgressHelper>();
                //dont show the indicator now
                m_progressHelper.ToggleHelper(false);
            }

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

        public void ProcessInteraction()
        {
            if (!doorIsOpen && !isEmpty)
            {
                var PlayerSlots = FindObjectOfType<PlayerSlots>();
                if (PlayerSlots.CanHoldItem(currentProduct.orderID))
                {
                    BasicGameEvents.RaiseOnProductAddedToSlot(currentProduct.orderID);
                    StartCoroutine(currentProduct.AnimateGoingToSlot());
                    currentProduct = null;
                    StartCoroutine(PlayDoorAnim(true, true));
                }
                else
                    return;
            }
            else if (isEmpty)
            {
                StartCoroutine(PlayDoorAnim(true, true));
            }
        }

        private void OnMouseDown()
        {
            ProcessInteraction();
        }

        public void SetProduct(HeatableProduct product, float heatingAmount)
        {
            if (doorIsOpen || currentProduct != null)
                return;
            currentProduct = product;
            heatingProcess = heatingAmount;
            StartCoroutine(OpenMicrowaveAndHeatProduct());
        }

        IEnumerator OpenMicrowaveAndHeatProduct()
        {
            yield return StartCoroutine(PlayDoorAnim(true, true));
            yield return StartCoroutine(Heating());
        }

        IEnumerator PlayDoorAnim(bool open, bool alsoReverse = false)
        {
            doorIsOpen = open;
            float totalTime = 1f;
            float curTime = totalTime;
            float totalAngle = 90;
            float multiplier = 1f;
            float finalAngle = 90;
            if (!open)
            {
                finalAngle = 0;
                multiplier = -1f;
            }

            while (curTime > 0)
            {
                var amount = Time.deltaTime;

                door.transform.Rotate(new Vector3(0f, (multiplier * totalAngle) * amount / totalTime, 0f), Space.Self);
                curTime -= Time.deltaTime;
                yield return null;
            }
            door.transform.localRotation = Quaternion.Euler(new Vector3(0f, finalAngle, 0f));
            doorIsOpen = false;

            yield return new WaitForSeconds(.2f);
            if (alsoReverse)
            {
                yield return StartCoroutine(PlayDoorAnim(!open, false));
            }
        }

        IEnumerator Heating()
        {
            m_progressHelper.ToggleHelper(true);

            float curProcess = heatingProcess;

            while (curProcess > 0)
            {
                curProcess -= Time.deltaTime;
                m_progressHelper.UpdateProcessUI(curProcess, heatingProcess);
                yield return null;
            }
            m_progressHelper.ToggleHelper(false);
        }
    }
}