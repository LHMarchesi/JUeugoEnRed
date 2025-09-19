using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] TMP_InputField playerName;
    [SerializeField] Button connectButton;
    [SerializeField] Action onConnectButtonClicked;
    [SerializeField] GameObject loadingPanel;

    private void Awake()
    {
        connectButton.onClick.AddListener(HandleConnectClick);

        // Al inicio mostramos panel de loading hasta conectar
        loadingPanel.SetActive(true);
        connectButton.interactable = false;

        ConnectionManager.Instance.Init();
        ConnectionManager.Instance.ConnectedToServer(UnShowLoadingPanel);
    }
    public void HandleConnectClick()
    {
        onConnectButtonClicked?.Invoke();
        ConnectionManager.Instance.SetNickName(playerName.text);
        GoToLobbyScene();
    }

    private void UnShowLoadingPanel()
    {
        loadingPanel.SetActive(false);
        connectButton.interactable = true;
    }

    public void GoToLobbyScene()
    {
        Debug.Log("Loading Lobby Scene...");
        ConnectionManager.Instance.LoadScene(1);
    }
}