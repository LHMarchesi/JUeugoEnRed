using Photon.Pun;
using UnityEngine;

public class MiniGameNetworkManager : MonoBehaviourPunCallbacks
{

    public static MiniGameNetworkManager Instance;
    public InteractionButton startButton;

    [Header("Prefab del panel del minijuego")]
    public GameObject miniGamePanelPrefab;

    private GameObject currentMiniGame;
    private bool alreadyStarted;
    private bool hasProcessedThisInteraction;

    PlayerContext playerContext => PlayerContext.LocalPlayer;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


    }

    private void Update()
    {
        if (startButton.IsOn && !alreadyStarted && !hasProcessedThisInteraction)
        {
            hasProcessedThisInteraction = true;
            StartMiniGame();
        }
    }

    // Esto se llama desde un botón, SOLO el MasterClient inicia el minijuego
    public void StartMiniGame()
    {
        photonView.RPC("RPC_CreateMiniGame", RpcTarget.All);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerContext.HandleInputs.SetPaused(true);
    }
    public void CloseMiniGame()
    {
        photonView.RPC("RPC_CloseMiniGame", RpcTarget.All);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        playerContext.HandleInputs.SetPaused(false);
    }

    [PunRPC]
    private void RPC_CreateMiniGame()
    {
        // Evitar duplicados si se vuelve a crear
        if (currentMiniGame != null)
            Destroy(currentMiniGame);

        currentMiniGame = PhotonNetwork.Instantiate(miniGamePanelPrefab.name, Vector3.zero, Quaternion.identity);
        currentMiniGame.SetActive(true);

        // Por seguridad, activar manualmente todos los hijos
        foreach (Transform t in currentMiniGame.transform)
            t.gameObject.SetActive(true);
    }


    [PunRPC]
    private void RPC_CloseMiniGame()
    {
        if (currentMiniGame != null)
        {
            PhotonNetwork.Destroy(currentMiniGame);
            currentMiniGame = null;
        }
    }
}
