using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ConnectionManager : Singleton<ConnectionManager>
{
    public PhotonPunConnectionManager photonPunManager;
    public Action OnConnectedToServer;
    public Action OnJoinedRoomEvent;
    private List<RoomInfo> roomList = new List<RoomInfo>();

    public Action OnPlayerEnteredRoomEvent;
    public Action OnPlayerLeftRoomEvent;

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
        Room currentRoom = photonPunManager.GetCurrenRoom();
        return currentRoom != null ? currentRoom.Name : "No Room";
    }

    public void JoinOrCreateRoom(Action OnJoinRoom = null)
    {
        if (OnJoinRoom != null)
            OnJoinedRoomEvent += OnJoinRoom;

        photonPunManager.JoinOrCreateRoom();
    }

    public void CreatePlayer(Transform spawnPos)
    {
        photonPunManager.InstantiatePlayer(spawnPos);
    }

    public void CreateRoom(string roomName)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            photonPunManager.CreateRoom(roomName);
        }
        else
        {
            //   Debug.WriteLine("Not connected yet. Will create room after connecting...");
            OnConnectedToServer += () => photonPunManager.CreateRoom(roomName);
        }
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

