using UnityEngine;
using UnityEngine.UI;

public class botonCerrarSesion : MonoBehaviour
{
    [SerializeField] private Button botonCerrar;
    void Start()
    {
        botonCerrar.onClick.AddListener(CerrarSesion);
    }

    public void CerrarSesion()
    {
        if (ControlSesion.Instance != null)
        {
            ControlSesion.Instance.CerrarSesion();
        }
    }
}