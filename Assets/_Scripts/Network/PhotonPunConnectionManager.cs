using Photon.Pun;
using Photon.Realtime; //Using Photon Pun & Realtime namespace
using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPunConnectionManager : MonoBehaviourPunCallbacks
{
    public Action OnConnectedToServer;
    public Action OnJoinedRoomEvent;
    public Action<List<RoomInfo>> OnNewRoomCreated;

    public Action OnPlayerEnteredRoomEvent;
    public Action OnPlayerLeftRoomEvent;

    public void init(Action onJoinRoom, Action onPlayerEnterCallback, Action onPlayerLeftCallback, Action<List<RoomInfo>> onRoomCreated = null)
    {
        OnJoinedRoomEvent = onJoinRoom;
        OnNewRoomCreated = onRoomCreated;
        OnPlayerEnteredRoomEvent = onPlayerEnterCallback;
        OnPlayerLeftRoomEvent = onPlayerLeftCallback;
    }

    public void ConnectToServer(Action OnConnect = null)
    {
        PhotonNetwork.ConnectUsingSettings();
        OnConnectedToServer += OnConnect;
    }

    public void LoadSceneForAll(int sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    public void InstantiatePlayer(Transform transform)
    {
        PhotonNetwork.Instantiate("PlayerPrefab", transform.position, Quaternion.identity);
    }
    public void SetNickname(string nickname)
    {
        PhotonNetwork.NickName = nickname;
    }

    public Room GetCurrenRoom()
    {
        return PhotonNetwork.CurrentRoom;
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
   
    public void JoinOrCreateRoom(Action OnJoin = null)
    {
        RoomOptions options = new RoomOptions
        {
            MaxPlayers = 2,
            IsOpen = true,
            IsVisible = true
        };

        // Intenta unirse a una sala aleatoria, si no existe la crea
        OnJoinedRoomEvent = OnJoin;
        PhotonNetwork.JoinRandomOrCreateRoom(null, 0, MatchmakingMode.FillRoom, null, null, null, options);
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

   
}

