using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Analytics;
using UnityEngine.Analytics;




public class UGS_Analytics : MonoBehaviour
{
    async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
    }

    public void NextLevel(int currentLevel)
    {
        CustomEvent myEvent = new CustomEvent("next_level")
        {
            { "level_index", currentLevel }
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Analytics.FlushEvents();
        Debug.Log("vamo bien");
    }
   


}