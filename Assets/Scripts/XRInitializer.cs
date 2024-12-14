using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;

public class XRInitializer : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(InitializeXR());
    }

    private IEnumerator InitializeXR()
    {
        if (XRGeneralSettings.Instance == null)
        {
            Debug.LogError("XRGeneralSettings.Instance is null.");
            yield break;
        }

        if (XRGeneralSettings.Instance.Manager == null)
        {
            Debug.LogError("XRGeneralSettings.Instance.Manager is null.");
            yield break;
        }

        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed.");
        }
        else
        {
            Debug.Log("XR initialized.");
            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            Debug.Log("XR started.");
        }
    }
}