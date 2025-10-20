using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum ChoiceSelected { Rock, Paper, Scissors }
public class RockPaperScissorsConnection : MonoBehaviour
{
    [SerializeField] GameObject loadingPanel;
    [SerializeField] TMP_InputField playerName;
    [SerializeField] Button connectButton;
    [SerializeField] GameObject playButtons;

    void Start()
    {
        connectButton.onClick.AddListener(HandleConnectClick);

        loadingPanel.SetActive(true);
        playButtons.SetActive(false);
        connectButton.interactable = false;
        ConnectionManager.Instance.Init();
        ConnectionManager.Instance.ConnectedToServer(UnShowLoadingPanel);

    }

    private void HandleConnectClick()
    {
        ConnectionManager.Instance.SetNickName(playerName.text);
        ConnectionManager.Instance.JoinOrCreateRoom();
        playButtons.SetActive(true);
        playerName.gameObject.SetActive(false);
        connectButton.gameObject.SetActive(false);
    }

    private void UnShowLoadingPanel()
    {
        loadingPanel.SetActive(false);
        connectButton.interactable = true;
    }
}
