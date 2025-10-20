using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyManager : MonoBehaviour
{
    [Space]
    [SerializeField] private TMP_InputField roomInputField;
    [SerializeField] private Button createRoomButton;

    void Start()
    {
        roomInputField.onValueChanged.AddListener(CheckRoomName);
        createRoomButton.onClick.AddListener(HandleCreateRoomClicked);

        ConnectionManager.Instance.OnJoinedRoom += HandleJoinedRoom;
    }

    private void CheckRoomName(string roomName)
    {
        createRoomButton.interactable = roomName.Length > 0;
        //popup de error
    }

    private void HandleCreateRoomClicked()
    {
        ConnectionManager.Instance.CreateRoom(roomInputField.text);
    }

    private void HandleJoinedRoom()
    {
        TransitionManager.Instance.PlayTransitionAndLoadScene(TransitionType.FadeOut, 2);
    }
}

