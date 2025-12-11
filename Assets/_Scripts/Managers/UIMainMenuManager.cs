using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuManager : MonoBehaviour
{
    [SerializeField] TMP_InputField nicknameField;
    [SerializeField] TextMeshProUGUI playerNameTxt;
    [SerializeField] Button connectButton;
    [SerializeField] Button joinRandomButton;
    [SerializeField] Action onConnectButtonClicked;
    [SerializeField] GameObject SearchRoomPanel;

    private void Start()
    {
        connectButton.onClick.AddListener(HandleConnectClick);
        joinRandomButton.onClick.AddListener(HandleJoinRandomClick);
        nicknameField.onValueChanged.AddListener(CheckNickname);

        // Al inicio mostramos panel de loading hasta conectar

        // Falta detectar si ya estamos conectados para cuando volvemos desde la partida al menu
        nicknameField.gameObject.SetActive(true);
        playerNameTxt.gameObject.SetActive(false);
        connectButton.interactable = false;


        if (!ConnectionManager.Instance.IsConnectedToServer())
        {
            ConnectionManager.Instance.ConnectedToServer(UnShowLoadingPanel);
        }
        //else
            //connectButton.interactable = true;

    }
    private void CheckNickname(string arg0)
    {
        if (arg0.Length == 0)
        {
            connectButton.interactable = false;
            //popup de error
        }
        else
        {
            connectButton.interactable = true;
            LootLockerBootstrap.Instance.SetPlayerName(arg0);
        }

    }
    public void HandleConnectClick()
    {
        onConnectButtonClicked?.Invoke();
        ConnectionManager.Instance.SetNickName(nicknameField.text);
        ConnectionManager.Instance.JoinLobby();

        playerNameTxt.gameObject.SetActive(true);
        playerNameTxt.text = nicknameField.text;

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