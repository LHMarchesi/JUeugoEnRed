using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Analytics;




public class UGS_Analytics : MonoBehaviour
{
    async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
    }

    public void NextLevel(int currentLevel)
    {
        //if (!_isInitialized)
        //{
        //    return;
        //}
        CustomEvent myEvent = new CustomEvent("next_level")
        {
            { "level_index", currentLevel }
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        AnalyticsService.Instance.Flush();
        Debug.Log("vamo bien");
    }
    //public void RestartGame()
    //{
    //    AnalyticsService.Instance.RecordEvent
    //}

    //public void GiveConsent()
    //{
    //    // Call if consent has been given by the user
    //    EndUserConsent.SetConsentState(new ConsentState
    //    {
    //        AnalyticsIntent = ConsentStatus.Granted,
    //    });
    //    Debug.Log($"Consent has been provided. The SDK is now collecting data!");
    //}


}