using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class ConnectionManager : Singleton<ConnectionManager>
{
    public PhotonPunConnectionManager photonPunManager;
    public Action OnConnectedToServer;
    public Action OnJoinedRoomEvent;

    public Action OnPlayerEnteredRoomEvent;
    public Action OnPlayerLeftRoomEvent;

    private List<RoomInfo> rooms = new List<RoomInfo>();


    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void Init()
    {
        photonPunManager.init(OnJoinedRoomEvent, OnPlayerLeftRoomEvent, OnPlayerEnteredRoomEvent);
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

    public void JoinSelectedRoom(string roomName)
    {
        photonPunManager.JoinRoom(roomName);
    }

    public List<RoomInfo> GetAllRooms()
    {
        return rooms;
    }
    private void HandleRoomCreated(List<RoomInfo> rooms)
    {
        this.rooms = rooms;
    }

    public void JoinOrCreateRoom(Action OnJoinRoom = null)
    {
        if (OnJoinRoom != null)
            OnJoinedRoomEvent += OnJoinRoom;

        photonPunManager.JoinOrCreateRoom(OnJoinRoom);
    }

    public void LoadScene(int scene)
    {
        photonPunManager.LoadSceneForAll(scene);
    }

    public GameObject CreatePlayer(Transform spawnPos)
    {
        return photonPunManager.InstantiatePlayer(spawnPos);
    }

    public void CreateRoom(string roomName)
    {
        /*
        if (PhotonNetwork.IsConnectedAndReady)
        {
            photonPunManager.CreateRoom(roomName);
        }
        else
        {
            //   Debug.WriteLine("Not connected yet. Will create room after connecting...");
            OnConnectedToServer += () => photonPunManager.CreateRoom(roomName);
        }*/
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

