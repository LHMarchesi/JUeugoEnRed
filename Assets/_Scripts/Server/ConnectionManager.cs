using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;


public class ConnectionManager : Singleton<ConnectionManager>
{
    public PhotonPunConnectionManager photonPunManager;
    public Action OnConnectedToServer;
    public Action OnJoinedRoomEvent;
    public List<RoomInfo> roomList = new List<RoomInfo>();
    public Action<List<RoomInfo>> OnNewRoomCreated;

    public Action OnPlayerEnteredRoomEvent;
    public Action OnPlayerLeftRoomEvent;

    public void Init()
    {
        photonPunManager.init(OnJoinedRoomEvent, OnPlayerLeftRoomEvent, OnPlayerEnteredRoomEvent, OnNewRoomCreated);
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

    public Room GetCurrentRoom()
    {
        Room currentRoom = photonPunManager.GetCurrenRoom();
        return currentRoom;
    }

    public void LoadScene(int scene)
    {
        photonPunManager.LoadSceneForAll(scene);
    }

    public void CreatePlayer(Transform spawnPos)
    {
        photonPunManager.InstantiatePlayer(spawnPos);
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

