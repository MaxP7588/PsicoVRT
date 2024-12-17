// ******------------------------------------------------------******
// OrderGenerator.cs
//
// Author:
//       K.Sinan Acar <ksa@puzzledwizard.com>
//
// Copyright (c) 2019 PuzzledWizard
//
// ******------------------------------------------------------******

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

namespace PW
{
    public class OrderGenerator : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI textoTemporizador;
        [SerializeField] private TextMeshProUGUI textoOrdenes;
        [SerializeField] private TextMeshProUGUI textoFinal;
        
        [Header("Configuración Juego")]
        [SerializeField] private float tiempoTotal = 5f; // 3 minutos
        private float tiempoRestante;
        private int ordenesCompletadas = 0;

        //This limits generating orders constantly
        public int MaxConcurrentOrder=2;

        public int currentOrderCount;

        public Sprite[] orderSprites;

        [HideInInspector]
        public int[] orderedProducts;

        public Transform UIParentForOrders;

        public GameObject orderRepPrefab;//The general prefab for order represantation

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip campanita;

        private bool juegoTerminado = false;

        private Entrar entrarScript;

        private void OnEnable()
        {
            //We'll listen for order events;
            BasicGameEvents.onOrderCancelled += BasicGameEvents_onOrderCancelled;
            BasicGameEvents.onOrderCompleted += BasicGameEvents_onOrderCompleted;

        }
        private void OnDisable()
        {
            //Don't forget to remove listeners from events on disable.
            BasicGameEvents.onOrderCancelled -= BasicGameEvents_onOrderCancelled;
            BasicGameEvents.onOrderCompleted -= BasicGameEvents_onOrderCompleted;

        }

        private void BasicGameEvents_onOrderCancelled(int ID)
        {
            //We could also do something with the ID of the product,
            //Or we could pass other things as parameters,
            //but for demo purposes this is fine.
            currentOrderCount--;

        }

        private void BasicGameEvents_onOrderCompleted(int ID,float percentageSucccess)
        {
            currentOrderCount--;
            ordenesCompletadas++;
            
            // Reproducir sonido de campana
            if (audioSource && campanita)
                audioSource.PlayOneShot(campanita);

            ActualizarUI();
        }


        void Start()
        {
            //In a demo only manner we start calling the coroutine here on Start.

            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();

            // Inicializar temporizador
            textoOrdenes.gameObject.SetActive(true);
            textoTemporizador.gameObject.SetActive(true);
            tiempoRestante = tiempoTotal;
            ActualizarUI();
            
            StartCoroutine(GenerateOrderRoutine(3f));

            // Obtener referencia al script Entrar
            entrarScript = FindObjectOfType<Entrar>();
        }

        void Update()
        {
            if (juegoTerminado) return;

            if (tiempoRestante > 0f)
            {
                tiempoRestante -= Time.deltaTime;
                ActualizarUI();
            }
            else
            {
                FinalizarJuego();
            }
        }

        public IEnumerator GenerateOrderRoutine(float intervalTime)
        {
            //We assume we don't pause the game or something,
            //You should check if your game state is playing here
            while (true)
            {
                if (currentOrderCount < MaxConcurrentOrder)
                {
                    GenerateOrder();
                    yield return new WaitForSeconds(intervalTime);
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        public void GenerateOrder()
        {
            Debug.Log("Generating order");

            //Get a random ID from sprites list
            //We could store the ID of the object to track last generated orders,
            //Totally random generation may create the same order in row repeatedly.

            int spriteIndex = Random.Range(10, 12);

            int orderID = orderedProducts[spriteIndex];

            var newOrder = GameObject.Instantiate(orderRepPrefab, UIParentForOrders).GetComponent<ServeOrder>();

            newOrder.SetOrder(orderID,40f);

            newOrder.SetSprite(orderSprites[spriteIndex]);

            currentOrderCount++;

        }

        public Sprite GetSpriteForOrder(int orderID)
        {
            var spriteIndex = Array.IndexOf(orderedProducts, orderID);
            if (spriteIndex<0)
                return null;
            return orderSprites[spriteIndex];
        }

        private void ActualizarUI()
        {
            if(textoTemporizador != null && textoOrdenes != null)
            {
                int minutos = Mathf.FloorToInt(tiempoRestante / 60);
                int segundos = Mathf.FloorToInt(tiempoRestante % 60);
                textoTemporizador.text = string.Format("{0:00}:{1:00}", minutos, segundos);
                textoOrdenes.text = $"Órdenes: {ordenesCompletadas}";
            }
        }

        private void FinalizarJuego()
        {
            if (juegoTerminado) return;
            
            Debug.Log("¡Juego terminado!");

            juegoTerminado = true;
            StopAllCoroutines();

            // Desactivar Manager de órdenes

            
            // Limpiar órdenes pendientes
            ServeOrder[] ordenesActivas = FindObjectsOfType<ServeOrder>();
            Debug.Log($"Ordenes activas: {ordenesActivas.Length}");
            foreach (var orden in ordenesActivas)
            {
                Debug.Log($"Destruyendo orden {orden.gameObject.name}");
                Destroy(orden.gameObject);
            }

            // Mostrar pantalla final
            Debug.Log("Mostrando pantalla final");
            textoFinal.gameObject.SetActive(true);
            textoFinal.text = $"¡Juego terminado!\nÓrdenes completadas: {ordenesCompletadas}";
            
            // Ocultar UI del juego
            Debug.Log("Ocultando UI del juego");
            textoTemporizador.gameObject.SetActive(false);
            textoOrdenes.gameObject.SetActive(false);
            
            // Oscurecer pantalla
            Debug.Log("Oscureciendo pantalla");
            if (entrarScript != null)
            {
                Debug.Log("Entrar script encontrado");
                entrarScript.OscurecerPantalla();
            }

            Debug.Log($"¡Juego terminado! Órdenes completadas: {ordenesCompletadas}");
        }
    }
}