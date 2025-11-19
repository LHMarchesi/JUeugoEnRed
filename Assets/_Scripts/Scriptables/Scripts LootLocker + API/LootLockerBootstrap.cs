using LootLocker.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootLockerBootstrap : Singleton<LootLockerBootstrap>
{
    public static bool SessionStarted {  get; private set; }

    int number;
    [SerializeField] string playerIdentifier = "guest";

    private void Awake()
    {
        base.Awake();
        number = UnityEngine.Random.Range(1, 10000);
        playerIdentifier = "guest" + number.ToString();
        DontDestroyOnLoad(gameObject);
        StartGuest();
    }

    void StartGuest()
    {
        LootLockerSDKManager.StartGuestSession(playerIdentifier, response =>
        {
            if (!response.success)
            {
                Debug.LogError("Fallo");
                return;
            }
            SessionStarted = true;
            Debug.Log("Conectado");
        });
    }

    public void SetPlayerName(string name)
    {
        if (!SessionStarted)
            return;
        LootLockerSDKManager.SetPlayerName(name, response =>
        {
            if (!response.success)
            {
                Debug.LogError("Fallo al setear nombre");
                return;
            }
            Debug.Log("Nombre seteado");
        });
    }
    public string GetPlayerIdentifier()
    {
        return playerIdentifier;
    }
}
