using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PW
{
    public class ProductWithCover : MonoBehaviour
    {
        Collider m_collider;

        public Transform coverObject;

        public Vector3 openCoverOffset;

        public bool autoCloseCover;

        const float k_AutoCloseCoverTime = 0.5f;

        bool IsAnimating = false;

        [Header("Configuración Input")]
        public KeyCode joystickButton = KeyCode.JoystickButton3;
        public float maxDistance = 8f;
        private Camera mainCamera;

        void OnEnable()
        {
            m_collider = GetComponent<Collider>();
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
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
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
            if (IsAnimating)
                return;

            //Open the cover
            StartCoroutine(OpenCloseDisplay(true, autoCloseCover));
        }

        public void HandleCoverCloseClick()
        {
            if (IsAnimating)
                return;
            StartCoroutine(OpenCloseDisplay(false, false));
        }

        IEnumerator OpenCloseDisplay(bool open, bool alsoReverse = false)
        {
            IsAnimating = true;
            float totalTime = 1f;
            float curTime = totalTime;
            var totalDist = (openCoverOffset);
            var finalPos = coverObject.position + openCoverOffset;

            if (!open)
            {
                totalDist = -openCoverOffset;
                finalPos = coverObject.position - openCoverOffset;
            }

            while (curTime > 0)
            {
                var amount = Time.deltaTime;
                var eulerTemp = coverObject.transform.rotation.eulerAngles;

                coverObject.transform.position += (totalDist * amount) / totalTime;
                curTime -= Time.deltaTime;
                yield return null;
            }
            m_collider.enabled = !open;

            coverObject.transform.position = finalPos;
            
            yield return new WaitForSeconds(.2f);

            if (alsoReverse)
            {
                if (autoCloseCover)
                {
                    //If auto closing enable wait for relevant time before closing.
                    yield return new WaitForSeconds(k_AutoCloseCoverTime);
                }
                yield return StartCoroutine(OpenCloseDisplay(!open, false));
                yield break;
            }

            m_collider.enabled = !open;

            if (open)
                coverObject.GetComponent<OnClickCoverHelper>().ActivateCollider();
            IsAnimating = false;
        }
    }
}