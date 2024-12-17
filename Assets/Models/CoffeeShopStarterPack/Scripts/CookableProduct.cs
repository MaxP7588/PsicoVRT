﻿using UnityEngine;
using System.Collections;
using PW; // Add the correct namespace for IInteractable

namespace PW
{
    public class CookableProduct : ProductGameObject
    {
        Collider m_collider;
        private Vector3 initialPosition;
        public CookingGameObject cookingObject;
        public GameObject platePrefab;
        public float cookingTimeForProduct;

        [HideInInspector]
        public bool IsCooked = false;

        [Header("Configuración Input")]
        public KeyCode joystickButton = KeyCode.JoystickButton3;
        public float maxDistance = 10f;
        private Camera mainCamera;
        public Transform raycastOrigin; // Nuevo objeto para lanzar el raycast

        private void Awake()
        {
            m_collider = GetComponent<Collider>();
            m_collider.enabled = true;
        }

        private void Start()
        {
            if (cookingObject == null)
                cookingObject = FindObjectOfType<CookingGameObject>();
            
            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            IsCooked = false;
            initialPosition = transform.position;
        }

        void Update()
        { 
        } 

        void OnMouseDown()
        {
            ProcessInteraction();
        }

        public void ProcessInteraction()
        {
            if (cookingObject == null)
                return;
            if (!cookingObject.IsEmpty() && !IsCooked)
                return;

            var targetPos = Vector3.zero;

            if (!IsCooked)
            {
                targetPos = cookingObject.GetCookingPosition();
                Vector3 startPos = Vector3.zero;

                if (cookingObject.HasStartAnimationPos(out startPos))
                {
                    transform.position = startPos;
                }

                StartCoroutine(MoveToPlace(targetPos));
            }
        }

        public override IEnumerator MoveToPlace(Vector3 targetPos)
        {
            cookingObject.DoDoorAnimationsIfNeeded();
            yield return new WaitForSeconds(cookingObject.doorAnimTime);
            yield return base.MoveToPlace(targetPos);

            m_collider.enabled = false;

            if (cookingObject.IsEmpty())
                cookingObject.StartCooking(this);

            yield return null;
        }

        public void DoneCooking()
        {
            IsCooked = true;
        }

        public override bool CanGoPlayerSlot()
        {
            if (base.CanGoPlayerSlot())
            {
                StartCoroutine(AnimateGoingToSlot());
                return true;
            }
            return false;
        }

        public override IEnumerator AnimateGoingToSlot()
        {
            if (RegenerateProduct)
            {
                BasicGameEvents.RaiseInstantiatePlaceHolder(transform.parent, initialPosition, gameObject);
            }
            yield return new WaitForSeconds(cookingObject.doorAnimTime);
            yield return base.AnimateGoingToSlot();
            gameObject.SetActive(false);
        }
    }
}