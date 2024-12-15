using UnityEngine;
using PW;

public class Basurero : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private AudioSource sonidoBasurero;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Basurero");
            PlayerSlots playerSlots = FindObjectOfType<PlayerSlots>();
            
            if (playerSlots != null )
            {
                // Recorrer todos los slots y eliminar items
                for (int i = 0; i < 3; i++)
                {
                    if (playerSlots)
                    {
                        BasicGameEvents.RaiseOnProductDeletedFromSlot(i);
                        playerSlots.slotUIObjects[i].sprite = null;
                    }
                }
                
                // Reproducir sonido
                if (sonidoBasurero != null)
                    sonidoBasurero.Play();
            }
        }
    }
}