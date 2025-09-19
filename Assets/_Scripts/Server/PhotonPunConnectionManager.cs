using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPunConnectionManager : MonoBehaviourPunCallbacks
{
    public Action OnConnectedToServer;
    public Action OnJoinedRoomEvent;
    public Action<List<RoomInfo>> OnNewRoomCreated;
    public List<RoomInfo> activeRooms = new List<RoomInfo>();

    public Action OnPlayerEnteredRoomEvent;
    public Action OnPlayerLeftRoomEvent;

    public void init(Action onJoinRoom, Action onPlayerEnterCallback, Action onPlayerLeftCallback, Action<List<RoomInfo>> onRoomCreated = null)
    {
        OnJoinedRoomEvent = onJoinRoom;
        OnNewRoomCreated = onRoomCreated;
        OnPlayerEnteredRoomEvent = onPlayerEnterCallback;
        OnPlayerLeftRoomEvent = onPlayerLeftCallback;
    }

    public void AddToRoomList(RoomInfo roomInfo)
    {
        activeRooms.Add(roomInfo);
        OnNewRoomCreated?.Invoke(activeRooms);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UnityEngine.Debug.Log("Player Entered Room: " + newPlayer.NickName);
        OnPlayerEnteredRoomEvent?.Invoke();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UnityEngine.Debug.Log("Player Left Room: " + otherPlayer.NickName);
        OnPlayerLeftRoomEvent?.Invoke();
    }

    public Room GetCurrenRoom()
    {
        Debug.Log("name:" + PhotonNetwork.CurrentRoom);
        return PhotonNetwork.CurrentRoom;
    }

    public void SetNickname(string nickname)
    {
        PhotonNetwork.NickName = nickname;
    }
    public void JoinLobby()
    {
        UnityEngine.Debug.Log("JoinedLobby");
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.EmptyRoomTtl = 100;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("RoomCreated");
        AddToRoomList(PhotonNetwork.CurrentRoom);
    }

    public void JoinRoom (Room room)
    {
        PhotonNetwork.JoinRoom(room.Name);
    }

    public void ConnectToServer(Action OnConnect = null)
    {
        PhotonNetwork.ConnectUsingSettings();
        OnConnectedToServer += OnConnect;
    }

    public override void OnJoinedRoom()
    {
        UnityEngine.Debug.Log("Joined Room");
        OnJoinedRoomEvent?.Invoke();
    }

    public override void OnConnectedToMaster()
    {
        UnityEngine.Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        OnConnectedToServer?.Invoke();
    }

    public void LoadSceneForAll(int sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    public void InstantiatePlayer(Transform transform)
    {
        PhotonNetwork.Instantiate("PlayerPrefab", transform.position, Quaternion.identity);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        OnNewRoomCreated?.Invoke(roomList);
        activeRooms.Clear(); 
        foreach (var room in roomList)
        {
            if (room.IsOpen) 
            {
                AddToRoomList(room);
            }
        }
    }
}

