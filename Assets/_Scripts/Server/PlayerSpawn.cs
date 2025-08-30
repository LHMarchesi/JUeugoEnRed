using UnityEngine;
using Photon.Pun;

public class PlayerSpawn : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.Instantiate("FirstPersonController", transform.position, Quaternion.identity);
    }
}
