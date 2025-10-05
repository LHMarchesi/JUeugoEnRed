using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] TMP_InputField roomInputField;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Button createRoomButton;
    [SerializeField] Button playButton;
    [SerializeField] GameObject characterSelectionPanel;
    [SerializeField] GameObject roomSelectionPanel;
    [SerializeField] GameObject roomUiPrefab;
    [SerializeField] GameObject playerUiPrefab;
    [SerializeField] GameObject content;
    [SerializeField] GameObject playerLaoyout;


    public void Start()
    {
        playButton.interactable = false;
        characterSelectionPanel.SetActive(false);
        roomInputField.onValueChanged.AddListener(HandleRoomNameChange);
        createRoomButton.onClick.AddListener(OnCreateRoomClick);
        playButton.onClick.AddListener(OnCreateRoomClick);

        ConnectionManager.Instance.Init();
        ConnectionManager.Instance.JoinLobby();

        ConnectionManager.Instance.OnJoinedRoomEvent += HandleJoinedRoom;
        //ConnectionManager.Instance.OnNewRoomCreated += UpdateRoomListUI;
    }

    private void HandleJoinedRoom()
    {
        roomSelectionPanel.SetActive(false);
        characterSelectionPanel.SetActive(true);
        Instantiate(playerUiPrefab, playerLaoyout.transform);
        roomNameText.text = "Room Name: " + roomInputField.text;

        //if (ConnectionManager.Instance.GetCurrentRoom().PlayerCount == 2)
      //  {
          //  playButton.interactable = true;
        //}
    }

    private void HandleCreateRoom()
    {
        roomSelectionPanel.SetActive(false);
        characterSelectionPanel.SetActive(true);
        Instantiate(playerUiPrefab, content.transform);
        Instantiate(playerUiPrefab, playerLaoyout.transform);
        roomNameText.text = "Room Name: " + roomInputField.text;
    }

    private void OnCreateRoomClick()
    {
        ConnectionManager.Instance.CreateRoom(roomInputField.text);
        HandleCreateRoom();
        Instantiate(roomUiPrefab, content.transform);
    }

    private void HandleRoomNameChange(string roomName)
    {
        createRoomButton.interactable = roomName.Length > 0;
    }
    private void UpdateRoomListUI(List<RoomInfo> roomList)
    {
        // Clear the existing UI elements first
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate a UI element for each room in the list
        foreach (RoomInfo roomInfo in roomList)
        {
            // Ensure the room is visible and open
            if (roomInfo.IsVisible && roomInfo.IsOpen)
            {
                GameObject roomUI = Instantiate(roomUiPrefab, content.transform);
                TMP_Text roomText = roomUI.GetComponentInChildren<TMP_Text>();
                Button roomButton = roomUI.GetComponentInChildren<Button>();
                if (roomText != null)
                {
                    roomText.text = roomInfo.Name; // Set the room name in the UI
                }

                //roomButton.onClick.AddListener(OnJoinRoomButtonClick);
            }
        }
    }

    public void OnJoinRoomButtonClick(string roomName)
    {
        //ConnectionManager.Instance.(roomName);  // Joining the room by name
    }

    public void GoToGameScene()
    {
        Debug.Log("Loading Game Scene...");
        ConnectionManager.Instance.LoadScene(2);
    }
}

