using UnityEngine;

public class BotonSesion : MonoBehaviour
{
    [Header("Objetos de Sesión")]
    [SerializeField] private GameObject objetoSinSesion;
    [SerializeField] private GameObject objetoConSesion;

    void Start()
    {
        ActualizarEstadoObjetos();
    }

    void Update()
    {
        ActualizarEstadoObjetos();
    }

    private void ActualizarEstadoObjetos()
    {
        if (ControlSesion.Instance != null)
        {
            bool sesionIniciada = ControlSesion.Instance.IsLoggedIn();
            objetoSinSesion.SetActive(!sesionIniciada);
            objetoConSesion.SetActive(sesionIniciada);
        }
    }
}