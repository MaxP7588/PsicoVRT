using System.Collections;
using UnityEngine;

namespace PW
{
    public class ReadyToServe : ProductGameObject
    {
        [Header("Referencias")]
        public GameObject platePrefab;
        public Camera mainCamera;
        
        [Header("Configuración")]
        public float maxDistance = 8f;
        public KeyCode joystickButton = KeyCode.JoystickButton3;

        private Collider m_collider;
        private bool canInteract = true;

        private void Awake()
        {
            m_collider = GetComponent<Collider>();
            m_collider.enabled = true;
            
            if(mainCamera == null)
                mainCamera = Camera.main;
        }

        void Update()
        {
            // Verificar input de joystick
            if (Input.GetKeyDown(joystickButton) && canInteract)
            {
                CheckRaycastAndServe();
            }
        }

        void OnMouseDown()
        {
            if (canInteract)
                ServeProduct();
        }

        private void CheckRaycastAndServe()
        {
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    ServeProduct();
                }
            }
        }

        private void ServeProduct()
        {
            if (!base.CanGoPlayerSlot())
                return;

            canInteract = false;

            if (AddToPlateBeforeServed)
            {
                var plate = GameObject.Instantiate(platePrefab, transform.position, Quaternion.identity);
                plate.transform.SetParent(transform);
                if (plateOffset.magnitude > 0)
                {
                    plate.transform.localPosition = plateOffset;
                }
                plate.transform.SetAsFirstSibling();
            }

            if (RegenerateProduct)
            {
                BasicGameEvents.RaiseInstantiatePlaceHolder(transform.parent, transform.position, gameObject);
            }

            StartCoroutine(AnimateGoingToSlot());
        }

        public override IEnumerator AnimateGoingToSlot()
        {
            yield return base.AnimateGoingToSlot();
            gameObject.SetActive(false);
            canInteract = true;
        }
    }
}