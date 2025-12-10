using Photon.Pun;
using UnityEngine;

public class TutorialManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform spawnPoint;
    void Start()
    {
        GameObject newPlayer = ConnectionManager.Instance.CreatePlayer(spawnPoint);
        newPlayer.transform.rotation = spawnPoint.rotation;
    }
   
}
