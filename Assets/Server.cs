using Photon.Pun;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviourPunCallbacks
{
    void Start()
    {
       PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene(1);
    }
}
