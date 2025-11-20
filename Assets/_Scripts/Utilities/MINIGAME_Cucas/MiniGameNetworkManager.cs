using Photon.Pun;
using System.Collections;
using UnityEngine;

public class MiniGameNetworkManager : MonoBehaviourPunCallbacks
{
    public static MiniGameNetworkManager Instance;
    public InteractionButton startButton;
    //  public InteractionButton closeButton;

    [Header("Prefab del panel del minijuego")]
    public GameObject miniGamePanelPrefab;

    private GameObject currentMiniGame;
    private bool alreadyStarted;
    private bool hasProcessedCloseInteraction;
    private bool hasProcessedOpenInteraction;

    PlayerContext playerContext => PlayerContext.LocalPlayer;
    Coroutine endMinigameDelay;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


    }

    private void Update()
    {
        if (startButton.IsOn && !alreadyStarted && !hasProcessedOpenInteraction)
        {
            hasProcessedOpenInteraction = true;
            StartMiniGame();
            endMinigameDelay = StartCoroutine(CloseAfterDelay(30f));
        }

    }

    // Esto se llama desde un botón, SOLO el MasterClient inicia el minijuego
    public void StartMiniGame()
    {
        photonView.RPC("RPC_CreateMiniGame", RpcTarget.AllBuffered);
    }
    public void CloseMiniGame()
    {
        photonView.RPC("RPC_CloseMiniGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RPC_CreateMiniGame()
    {
        UIPlayerManager.Instance.ShowMinigame(true);
    }

    [PunRPC]
    private void RPC_CloseMiniGame()
    {
        UIPlayerManager.Instance.ShowMinigame(false);
    }
    IEnumerator CloseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        CloseMiniGame();
    }
}
