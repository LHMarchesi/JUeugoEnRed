using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLeaderboardScene : MonoBehaviour
{
    public void loadLeaderboard()
    {
       TransitionManager.Instance.PlayTransitionAndLoadScene(TransitionType.FadeOut, 3);
    }
}
