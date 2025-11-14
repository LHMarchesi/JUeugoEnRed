using System;
using UnityEngine;
using Photon.Realtime;
using System.Collections.Generic;

public class ConnectionManager : Singleton<ConnectionManager>
{
    public PhotonPunConnectionManager photonPunManager;
    public Action OnConnectedToServer;
    public Action OnJoinedRoom;

    public Action OnPlayerEnteredRoom;
    public Action<Player> OnPlayerLeftRoom;

    private List<RoomInfo> rooms = new List<RoomInfo>();


    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        photonPunManager.Init(HandleJoinedRoom,
            HandleRoomCreated,
            HandleNewPlayerInRoom,
            HandlePlayerLeftRoom);
    }

    private void HandleRoomCreated(List<RoomInfo> rooms)
    {
        this.rooms = rooms;
    }

    public void JoinSelectedRoom(string roomName)
    {
        photonPunManager.JoinRoom(roomName);
    }

    private void HandleJoinedRoom()
    {
        OnJoinedRoom?.Invoke();
    }

    public void HandleNewPlayerInRoom()
    {
        OnPlayerEnteredRoom?.Invoke();
    }

    public void HandlePlayerLeftRoom( Player otherplayer)
    {
        OnPlayerLeftRoom?.Invoke(otherplayer);
    }

    public void SetNickName(string name)
    {
        photonPunManager.SetNickname(name);
    }

    public void ConnectedToServer(Action CallBack = null)
    {
        photonPunManager.ConnectToServer(HandleConnectionToServer);
        OnConnectedToServer += CallBack;
    }

    public string GetCurrentRoomName()
    {
        string currentRoomName = photonPunManager.GetCurrenRoom().Name;
        return currentRoomName != null ? currentRoomName : "No Room";
    }

    public void JoinOrCreateRoom(Action OnJoinRoom = null)
    {
        if (OnJoinRoom != null)
            OnJoinedRoom += OnJoinRoom;

        photonPunManager.JoinOrCreateRoom(OnJoinRoom);
    }

    public void LeaveRoom()
    {
        photonPunManager.LeaveRoom();
    }

    public bool IsConnectedToServer()
    {
        return photonPunManager.IsConnectedToServer();
    }

    public void LoadScene(int scene)
    {
        photonPunManager.LoadSceneForAll(scene);
    }

    public GameObject CreatePlayer(Transform spawnPos)
    {
        return photonPunManager.InstantiatePlayer(spawnPos);
    }

    public Dictionary<int, Player> GetPlayersInRoom()
    {
        return photonPunManager.GetPlayersInRoom();
    }

    public List<RoomInfo> GetAllRooms()
    {
        return rooms;
    }

    public void CreateRoom(string roomName)
    {
        photonPunManager.CreateRoom(roomName);
    }

    public void JoinLobby()
    {
        photonPunManager.JoinLobby();
    }
    public void HandleConnectionToServer()
    {
        OnConnectedToServer?.Invoke();
    }
}

