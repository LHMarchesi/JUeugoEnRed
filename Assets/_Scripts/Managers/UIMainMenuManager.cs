using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuManager : MonoBehaviour
{
    [SerializeField] TMP_InputField playerName;
    [SerializeField] TextMeshProUGUI playerNameTxt;
    [SerializeField] Button connectButton;
    [SerializeField] Action onConnectButtonClicked;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject SearchRoomPanel;

    private void Start()
    {
        connectButton.onClick.AddListener(HandleConnectClick);

        // Al inicio mostramos panel de loading hasta conectar
        loadingPanel.SetActive(true);
        playerName.gameObject.SetActive(true);
        playerNameTxt.gameObject.SetActive(false);
        connectButton.interactable = false;

        ConnectionManager.Instance.Init();
        ConnectionManager.Instance.ConnectedToServer(UnShowLoadingPanel);
    }
    public void HandleConnectClick()
    {
        onConnectButtonClicked?.Invoke();
        ConnectionManager.Instance.SetNickName(playerName.text);

        playerNameTxt.gameObject.SetActive(true);
        playerNameTxt.text = playerName.text;


        // ConnectionManager.Instance.JoinOrCreateRoom(GoToGameScene);
    }

    private void UnShowLoadingPanel()
    {
        loadingPanel.SetActive(false);
        connectButton.interactable = true;
    }

    public void GoToGameScene()
    {
        Debug.Log("Loading Game Scene...");
        ConnectionManager.Instance.LoadScene(2);
        GameManager.Instance.ChangeGameState(new GameState());
    }
}