using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Restablecer : MonoBehaviour
{
    [SerializeField] private Button botonRestablecer;

    private List<Dropdown> dropdowns = new List<Dropdown>();
    private List<Slider> sliders = new List<Slider>();
    private List<Toggle> toggles = new List<Toggle>();

    private Dictionary<Dropdown, int> dropdownValoresIniciales = new Dictionary<Dropdown, int>();
    private Dictionary<Slider, float> sliderValoresIniciales = new Dictionary<Slider, float>();
    private Dictionary<Toggle, bool> toggleValoresIniciales = new Dictionary<Toggle, bool>();

    void Start()
    {
        // Guardar el estado inicial de todos los componentes UI
        GuardarEstadoInicial(transform);

        // Asignar el listener al botón de restablecer
        botonRestablecer.onClick.AddListener(RestablecerEstado);
    }

    void GuardarEstadoInicial(Transform objeto)
    {
        // Guardar el estado inicial de los componentes UI en el objeto
        foreach (Dropdown dropdown in objeto.GetComponentsInChildren<Dropdown>())
        {
            dropdowns.Add(dropdown);
            dropdownValoresIniciales[dropdown] = dropdown.value;
        }

        foreach (Slider slider in objeto.GetComponentsInChildren<Slider>())
        {
            sliders.Add(slider);
            sliderValoresIniciales[slider] = slider.value;
        }

        foreach (Toggle toggle in objeto.GetComponentsInChildren<Toggle>())
        {
            toggles.Add(toggle);
            toggleValoresIniciales[toggle] = toggle.isOn;
        }

        // Guardar el estado inicial de todos los hijos
        foreach (Transform hijo in objeto)
        {
            GuardarEstadoInicial(hijo);
        }
    }

    void RestablecerEstado()
    {
        // Restablecer el estado de todos los Dropdowns
        foreach (Dropdown dropdown in dropdowns)
        {
            dropdown.value = dropdownValoresIniciales[dropdown];
        }

        // Restablecer el estado de todos los Sliders
        foreach (Slider slider in sliders)
        {
            slider.value = sliderValoresIniciales[slider];
        }

        // Restablecer el estado de todos los Toggles
        foreach (Toggle toggle in toggles)
        {
            toggle.isOn = toggleValoresIniciales[toggle];
        }
    }
}