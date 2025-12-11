
using Photon.Pun;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] public bool isTutorialComplete = false;

    [Header("OnPickedCard Dialogue")]
    [SerializeField] ItemBase kidCard;
    [SerializeField] private TextAsset dialogue04_OnCardPickedUp;
    private bool dialogue04Played = false;

    [Header("OnCraft Dialogue")]
    [SerializeField] private Platform platform;
    [SerializeField] private TextAsset dialogue05_OnCorrectCraft;
    private bool dialogue05Played = false;


    [Header("OnRecipePlaced Dialogue")]
    [SerializeField] RecipeTrigger recipeTrigger;
    [SerializeField] private TextAsset dialogue07_OnRecipePlaced;
    private bool dialogue07Played = false;

    [Header("OnItemSended Dialogue")]
    [SerializeField] GiftboxScript giftboxScript;
    [SerializeField] private TextAsset dialogue08_OnRecipePlaced;
    private bool dialogue08Played = false;

    void Start()
    {
        if (!ConnectionManager.Instance.IsConnectedToServer())
        {
            ConnectionManager.Instance.ConnectedToServer(null);
            ConnectionManager.Instance.photonPunManager.ConnectToServer();
        }
        else
        {
            // Already connected: just proceed to lobby
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        CreateTutorialRoom();
    }

    private void CreateTutorialRoom()
    {
        ConnectionManager.Instance.JoinOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        StartTutorial(); 
    }

    private void UnShowLoadingPanel()
    {
        TransitionManager.Instance.PlayTransition(TransitionType.FadeIn);
    }

    private void StartTutorial()
    {
        UnShowLoadingPanel();
        GameObject newPlayer = ConnectionManager.Instance.CreatePlayer(spawnPoint);
        newPlayer.transform.rotation = spawnPoint.rotation;

        kidCard.OnPickedUp += OnKidCardPicked;
        platform.OnCraftCompleted += OnCraftCompleted;
        recipeTrigger.onRecipePlaced += OnRecipePlaced;
        giftboxScript.OnItemSended += OnItemSended;
    }

    private void OnItemSended()
    {
        if (dialogue08Played) return;

        dialogue08Played = true;
        Debug.Log("OnItemSended Triggered");
        StartCoroutine(WaitAndLoadDialogue(dialogue08_OnRecipePlaced, .5f));
        giftboxScript.OnItemSended -= OnItemSended;

        isTutorialComplete = true;
    }

    private void OnRecipePlaced()
    {
        if (dialogue07Played) return;
        dialogue07Played = true;

        StartCoroutine(WaitAndLoadDialogue(dialogue07_OnRecipePlaced, .5f));
        recipeTrigger.onRecipePlaced -= OnRecipePlaced;
    }

    private void OnCraftCompleted()
    {
        if (dialogue05Played) return;
        dialogue05Played = true;

        StartCoroutine(WaitAndLoadDialogue(dialogue05_OnCorrectCraft, 1f));
        platform.OnCraftCompleted -= OnCraftCompleted;
    }

    private void OnKidCardPicked()
    {
        if (dialogue04Played) return;

        dialogue04Played = true;
        StartCoroutine(WaitAndLoadDialogue(dialogue04_OnCardPickedUp, 1.25f));

        kidCard.OnPickedUp -= OnKidCardPicked;
    }

    IEnumerator WaitAndLoadDialogue(TextAsset dialogue, float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        ScriptReader.Instance.LoadStory(dialogue);
    }


}