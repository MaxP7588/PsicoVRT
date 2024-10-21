using Google.XR.Cardboard;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardboardStartup : MonoBehaviour
{
    
    public void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.brightness = 1.0f;
        
        if (!Api.HasDeviceParams())
        {
            //Api.ScanDeviceParams();
        }
    }


    public void Update()
    {
        if (Api.IsGearButtonPressed)
        {
            //Api.ScanDeviceParams();
        }

        if (Api.IsCloseButtonPressed)
        {
            SceneManager.LoadScene(0);
        }

        if (Api.IsTriggerHeldPressed)
        {
            Api.Recenter();
        }

        if (Api.HasNewDeviceParams())
        {
            Api.ReloadDeviceParams();
        }

        #if !UNITY_EDITOR
            Api.UpdateScreenParams();
        #endif
    }
}
