using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] TMP_InputField roomInputField;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Button createRoomButton;
    [SerializeField] GameObject characterSelectionPanel;
    [SerializeField] GameObject roomSelectionPanel;

    public void Start()
    {
        characterSelectionPanel.SetActive(false);
        roomInputField.onValueChanged.AddListener(HandleRoomNameChange);
        createRoomButton.onClick.AddListener(HandleCreateRoomClick);

        ConnectionManager.Instance.Init();
        ConnectionManager.Instance.JoinLobby();
        ConnectionManager.Instance.OnJoinedRoomEvent += HandleJoinedRoom;
    }

    private void HandleJoinedRoom()
    {
        roomSelectionPanel.SetActive(false);
        characterSelectionPanel.SetActive(true);
        roomNameText.text = "Room Name: " + ConnectionManager.Instance.GetCurrentRoomName();
    }

    private void HandleCreateRoomClick()
    {
        ConnectionManager.Instance.CreateRoom(roomInputField.text);
    }

    private void HandleRoomNameChange(string roomName)
    {
        createRoomButton.interactable = roomName.Length > 0;
    }
}

