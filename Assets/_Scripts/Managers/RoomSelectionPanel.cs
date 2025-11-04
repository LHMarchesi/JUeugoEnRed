using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSelectionPanel : MonoBehaviour
{
    [SerializeField] private Transform contentTransform;
    [SerializeField] private RoomItemUI roomUIPrefab;
    [SerializeField] private Button refreshButton;

    private List<RoomItemUI> roomsUI = new List<RoomItemUI>();


    void Start()
    {
        refreshButton.onClick.AddListener(PopulateRoomsList);
        PopulateRoomsList();
    }

    //Note: Consider doing this only after the player ask for it
    public void PopulateRoomsList()
    {
        ClearRoomsList();

        List<RoomInfo> allRooms = ConnectionManager.Instance.GetAllRooms();
        Debug.Log("Populating rooms list. Total rooms: " + allRooms.Count);
        foreach (RoomInfo room in allRooms)
        {
            RoomItemUI roomUI = Instantiate(roomUIPrefab, contentTransform);
            roomUI.SetUp(room.Name, HandleJoinRoomRequest);
            roomsUI.Add(roomUI);
        }

    }

    private void ClearRoomsList()
    {
        foreach (RoomItemUI room in roomsUI)
        {
            Destroy(room.gameObject);
        }

        roomsUI.Clear();
    }

    private void HandleJoinRoomRequest(string roomName)
    {
          ConnectionManager.Instance.JoinSelectedRoom(roomName);
    }
}