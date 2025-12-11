using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] TutorialManager tutorialManager;
    [SerializeField] TMP_Text text;
    private void OnTriggerEnter(Collider other)
    {
        var room = Photon.Pun.PhotonNetwork.CurrentRoom;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.EmptyRoomTtl = 0;
        roomOptions.PlayerTtl = 0;
        if (other.CompareTag("Player"))
        {
            if (tutorialManager.isTutorialComplete)
            {
                room.IsOpen = false;
                room.IsVisible = false;
                foreach (var p in PhotonNetwork.PlayerListOthers)
                {
                    PhotonNetwork.CloseConnection(p);
                }
                TransitionManager.Instance.PlayTransitionAndLoadScene(TransitionType.FadeOut, 0);
                ConnectionManager.Instance.photonPunManager.LeaveRoom();
                GameManager.Instance.ChangeGameState(new GameState());
            }
            else
            {
                text.gameObject.SetActive(true);
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.gameObject.SetActive(false);
        }
    }
}
