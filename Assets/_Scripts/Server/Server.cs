using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.Diagnostics.Tracing;
using TMPro;

public class Server : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField lobbyname;
    [SerializeField] TMP_InputField code;
    [SerializeField] Button getroom;
    [SerializeField] Button createroom;


    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        getroom.onClick.AddListener(EnterRoom);

    }

    public override void OnConnectedToMaster()
    {
        
    }

    public void CreateOrJoinRoomByName(string roomName)
    {
        if (code != null)
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public void EnterRoom()
    {
        CreateOrJoinRoomByName(code.name);
    }
    public void MakeRoom()
    {
        CreateOrJoinRoomByName(lobbyname.name);
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene(1);
    }
}
