using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        ConnectionManager.Instance.JoinOrCreateRoom(GoToGameScene);
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
    }
}