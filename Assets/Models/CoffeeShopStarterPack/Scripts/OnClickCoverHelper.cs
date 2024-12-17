using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PW
{
    [RequireComponent(typeof(Collider))]
    public class OnClickCoverHelper : MonoBehaviour {
        [SerializeField]
        public UnityEvent methodToCall;
        Collider m_collider;

        [Header("Configuración Input")]
        public KeyCode joystickButton = KeyCode.JoystickButton3;
        public float maxDistance = 8f;
        private Camera mainCamera;

        void OnEnable()
        {
            m_collider = GetComponent<Collider>();
            m_collider.enabled = false;
            mainCamera = Camera.main;
        }

        void Update()
        {
        }

        private void OnMouseDown()
        {
            ProcessInteraction();
        }

        public void ProcessInteraction()
        {
            if (methodToCall != null)
            {
                methodToCall.Invoke();
                m_collider.enabled = false;
            }
        }

        public void ActivateCollider()
        {
            m_collider.enabled = true;
        }
    }
}