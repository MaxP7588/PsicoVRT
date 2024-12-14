using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Configuraciones : MonoBehaviour
{
    public enum ResolutionType { HD_720p, FullHD_1080p, QHD_2K }
    public enum ControllerType { Android, PS4, Xbox }

    [Header("Referencias UI")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown controllerDropdown;
    [SerializeField] private AudioMixer audioMixer;

    private Resolution[] resolutions;
    private ControllerType currentController;
    
    private readonly Vector2Int[] resolutionValues = new Vector2Int[] {
        new Vector2Int(1280, 720),   // HD
        new Vector2Int(1920, 1080),  // Full HD
        new Vector2Int(2560, 1440)   // 2K
    };

    void Start()
    {
        LoadSettings();
        InitializeUI();
    }

    private void InitializeUI()
    {
        // Configurar Dropdown de resolución
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(new List<string> { "720p", "1080p", "2K" });
        resolutionDropdown.value = PlayerPrefs.GetInt("Resolution", 0);
        
        // Configurar Slider de volumen
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        
        // Configurar Dropdown de controles
        controllerDropdown.ClearOptions();
        controllerDropdown.AddOptions(new List<string> { "Android", "PS4", "Xbox" });
        controllerDropdown.value = PlayerPrefs.GetInt("Controller", 0);
    }

    private void LoadSettings()
    {
        // Cargar resolución
        int resIndex = PlayerPrefs.GetInt("Resolution", 0);
        SetResolution(resIndex);
        
        // Cargar volumen
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        SetVolume(volume);
        
        // Cargar tipo de control
        currentController = (ControllerType)PlayerPrefs.GetInt("Controller", 0);
        ConfigureController(currentController);
    }

    public void SetResolution(int resolutionIndex)
    {
        Vector2Int res = resolutionValues[resolutionIndex];
        Screen.SetResolution(res.x, res.y, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetController(int controllerIndex)
    {
        currentController = (ControllerType)controllerIndex;
        ConfigureController(currentController);
        PlayerPrefs.SetInt("Controller", controllerIndex);
    }

    private void ConfigureController(ControllerType type)
    {
        switch (type)
        {
            case ControllerType.Android:
                // Configurar inputs para Android
                break;
            case ControllerType.PS4:
                // Configurar inputs para PS4
                break;
            case ControllerType.Xbox:
                // Configurar inputs para Xbox
                break;
        }
    }

    void OnDestroy()
    {
        PlayerPrefs.Save();
    }
}