using UnityEngine;
using System.Collections;

namespace PW
{
    public class CookingGameObject : MonoBehaviour, IInteractable
    {
        //offset to Pivot, Vector3.zero is the default.
        public Vector3 cookingSpot;

        //Animation starting position can be set here,zero is default.
        public Vector3 startingPositionOffset;

        public CookableProduct currentProduct;

        public GameObject progressHelperprefab;

        [HideInInspector]
        public ProgressHelper m_progressHelper;

        public float doorAnimTime = 1f;

        float cookingProcess;

        Collider m_Collider;

        [Header("Configuración Input")]
        public KeyCode joystickButton = KeyCode.JoystickButton3;
        public float maxDistance = 8f;
        private Camera mainCamera;

        private void Awake()
        {
            m_Collider = GetComponent<Collider>();
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

        public virtual void OnMouseDown()
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

        public virtual Vector3 GetCookingPosition()
        {
            return transform.position + cookingSpot;
        }

        public virtual void DoDoorAnimationsIfNeeded()
        {
           
        }

        public virtual bool IsEmpty()
        {
            return currentProduct == null;
        }

        public virtual void StartCooking(CookableProduct product)
        {
            currentProduct = product;
            cookingProcess = product.cookingTimeForProduct;

            m_Collider.enabled = false;
            StartCoroutine(Cooking());
        }

        public virtual void ReadyToServe()
        {
            currentProduct = null;
            m_Collider.enabled = true;
            DoDoorAnimationsIfNeeded();
        }

        public virtual bool HasStartAnimationPos(out Vector3 result)
        {
            result = Vector3.zero;
            if (startingPositionOffset != Vector3.zero)
            {
                result = transform.position + startingPositionOffset;
                return true;
            }
            else
                return false;
        }

        public virtual IEnumerator Cooking()
        {
            m_progressHelper.ToggleHelper(true);
            var curTime = cookingProcess + doorAnimTime;
            while (curTime > 0)
            {
                curTime -= Time.deltaTime;
                m_progressHelper.UpdateProcessUI(curTime, cookingProcess);
                yield return null;
            }
            currentProduct.DoneCooking();
            m_progressHelper.ToggleHelper(false);
            m_Collider.enabled = true;
        }
    }
}