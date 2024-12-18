﻿using UnityEngine;
using System.Collections;

namespace PW
{
    public class StoveGameObject : CookingGameObject, IInteractable
    {
        public Transform doorTransform;

        private Vector3 progressHelperOffset = new Vector3(0f, 0.8f, 0f);

        private bool doorIsOpen = false;
        private bool isAnimating;

        [Header("Configuración Input")]
        public KeyCode joystickButton = KeyCode.JoystickButton3;
        public float maxDistance = 8f;
        private Camera mainCamera;

        private void Start()
        {
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

        public override void OnMouseDown()
        {
            ProcessInteraction();
        }

        public void ProcessInteraction()
        {
            if (currentProduct != null && currentProduct.IsCooked)
            {
                //Try to serve currentProduct if player slots are available
                if (currentProduct.CanGoPlayerSlot())
                    ReadyToServe();
            }
            else
            {
                DoDoorAnimationsIfNeeded();
            }
        }

        public override void DoDoorAnimationsIfNeeded()
        {
            base.DoDoorAnimationsIfNeeded();
            if(!isAnimating)
                StartCoroutine(PlayDoorAnim(true, true));
        }

        public override void StartCooking(CookableProduct product)
        {
            base.StartCooking(product);
            m_progressHelper.transform.position += progressHelperOffset;
        }

        IEnumerator PlayDoorAnim(bool open, bool alsoReverse = false)
        {
            doorIsOpen = open;
            isAnimating = true;
            float totalTime = doorAnimTime;
            float curTime = totalTime;
            float totalAngle = 66;
            float multiplier = 1f;
            float finalAngle = 66;
            if (!open)
            {
                finalAngle = 0;
                multiplier = -1f;
            }

            while (curTime > 0)
            {
                var amount = Time.deltaTime;
                var eulerTemp = doorTransform.rotation.eulerAngles;

                doorTransform.Rotate(new Vector3((multiplier * totalAngle) * amount / totalTime, 0f, 0f), Space.Self);
                curTime -= Time.deltaTime;
                yield return null;
            }
            doorTransform.localRotation = Quaternion.Euler(new Vector3(finalAngle, 0f, 0f));
            doorIsOpen = false;

            yield return new WaitForSeconds(.2f);
            if (alsoReverse)
            {
                yield return StartCoroutine(PlayDoorAnim(!open, false));
                isAnimating = false;
            }
            else
            {
                isAnimating = false;
            }
        }
    }
}