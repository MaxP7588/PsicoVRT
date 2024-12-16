using UnityEngine;
using PW;

public class Basurero : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private AudioSource sonidoBasurero;
    [SerializeField] private AudioClip sonidoBasura;

    void Start()
    {
        // Inicializar AudioSource si no existe
        if (sonidoBasurero == null)
        {
            sonidoBasurero = gameObject.AddComponent<AudioSource>();
            sonidoBasurero.playOnAwake = false;
            sonidoBasurero.spatialBlend = 0f; // Sonido 2D
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerSlots playerSlots = FindObjectOfType<PlayerSlots>();
            
            if (playerSlots != null)
            {
                bool tieneItems = false;
                
                // Recorrer slots y verificar si hay items
                for (int i = 0; i < 3; i++)
                {
                    if (playerSlots && playerSlots.slotUIObjects[i].sprite != null)
                    {
                        tieneItems = true;
                        BasicGameEvents.RaiseOnProductDeletedFromSlot(i);
                        playerSlots.slotUIObjects[i].sprite = null;
                    }
                }
                
                // Reproducir sonido solo si había items
                if (tieneItems && sonidoBasurero != null && sonidoBasura != null)
                {
                    sonidoBasurero.PlayOneShot(sonidoBasura);
                }
            }
        }
    }
}