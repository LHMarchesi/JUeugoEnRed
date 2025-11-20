using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PauseScript : MonoBehaviourPunCallbacks
{
    
   public void LeaveGame()
    {
        TransitionManager.Instance.PlayTransitionAndLoadScene(TransitionType.FadeOut, 0);
        ConnectionManager.Instance.photonPunManager.LeaveRoom();
        GameManager.Instance.ChangeGameState(new GameState());
    }

}
