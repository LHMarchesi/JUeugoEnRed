using TMPro;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] TutorialManager tutorialManager;
    [SerializeField] TMP_Text text;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tutorialManager.isTutorialComplete)
            {
                TransitionManager.Instance.PlayTransitionAndLoadScene(TransitionType.FadeOut, 0);
                ConnectionManager.Instance.LeaveRoom();
                GameManager.Instance.ChangeGameState(new MainMenuState());
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
