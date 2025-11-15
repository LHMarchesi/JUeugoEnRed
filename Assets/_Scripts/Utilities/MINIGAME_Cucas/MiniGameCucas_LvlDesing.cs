using Photon.Pun;
using UnityEngine;

public class MiniGameCucas_LvlDesing : MonoBehaviour
{
    public InteractionButton lever;
    private bool isOpen;
    private bool hasProcessedThisOpen;
    PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (lever.IsOn && !isOpen && !hasProcessedThisOpen)
        {
            hasProcessedThisOpen = true;
            photonView.RPC("OpenMiniGame", RpcTarget.AllBuffered);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            photonView.RPC("CloseMiniGame", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void OpenMiniGame()
    {
        isOpen = true;
        UIPlayerManager.Instance.ShowMinigame(true);
    }

    [PunRPC]
    public void CloseMiniGame()
    {
        UIPlayerManager.Instance.ShowMinigame(false);

        isOpen = false;
        hasProcessedThisOpen = false;
    }
}
