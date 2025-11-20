using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    string playerName = null;
    // Start is called before the first frame update
    void Start()
    {
        playerName = LootLockerBootstrap.Instance.GetPlayerIdentifier();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AddScore();
        }
    }

    public void AddScore() {         
        LeaderboardService.SubmitScore(100, "gifts", success =>
        {
            if (success)
            {
                Debug.Log("Score submitted successfully.");
            }
            else
            {
                Debug.LogError("Failed to submit score.");
            }
        });
    }
}
