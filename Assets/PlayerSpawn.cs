using UnityEngine;
using Photon.Pun;

public class PlayerSpawn : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate("FirstPersonController", transform.position, Quaternion.identity);
    }
}
