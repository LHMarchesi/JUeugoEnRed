using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuManager : MonoBehaviour
{
    [SerializeField] TMP_InputField playerName;
    [SerializeField] TextMeshProUGUI playerNameTxt;
    [SerializeField] Button connectButton;
    [SerializeField] Button joinRandomButton;
    [SerializeField] Action onConnectButtonClicked;
    [SerializeField] GameObject SearchRoomPanel;

    private void Start()
    {
        connectButton.onClick.AddListener(HandleConnectClick);
        joinRandomButton.onClick.AddListener(HandleJoinRandomClick);

        // Al inicio mostramos panel de loading hasta conectar

        // Falta detectar si ya estamos conectados para cuando volvemos desde la partida al menu
        playerName.gameObject.SetActive(true);
        playerNameTxt.gameObject.SetActive(false);
        connectButton.interactable = false;


        if (!ConnectionManager.Instance.IsConnectedToServer())
        {
            ConnectionManager.Instance.Init();
            ConnectionManager.Instance.ConnectedToServer(UnShowLoadingPanel);
        }
        else
            connectButton.interactable = true;

    }
    public void HandleConnectClick()
    {
        onConnectButtonClicked?.Invoke();
        ConnectionManager.Instance.SetNickName(playerName.text);

        playerNameTxt.gameObject.SetActive(true);
        playerNameTxt.text = playerName.text;

    }

    public void HandleJoinRandomClick()
    {
        ConnectionManager.Instance.JoinOrCreateRoom(GoToGameScene);
    }

    private void UnShowLoadingPanel()
    {
        TransitionManager.Instance.PlayTransition(TransitionType.FadeIn);
        connectButton.interactable = true;
    }

    public void GoToGameScene()
    {
        Debug.Log("Loading Game Scene...");
        TransitionManager.Instance.PlayTransitionAndLoadScene(TransitionType.FadeOut, 2);
        GameManager.Instance.ChangeGameState(new GameState());
    }
}