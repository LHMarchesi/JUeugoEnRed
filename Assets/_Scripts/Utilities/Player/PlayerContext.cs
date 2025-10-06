using Photon.Pun;
using UnityEngine;

public class PlayerContext : MonoBehaviour // Save valuable data from the player
{
    public static PlayerContext LocalPlayer;
    private HandleInputs handleInputs;
    private HandleAnimations handleAnimations;
    private PlayerController playerController;
    private UIPlayerManager playerUI;
    private PhotonView photonView;
    public int ownSocres;
    public HandleAnimations HandleAnimations { get => handleAnimations; set => handleAnimations = value; }
    public HandleInputs HandleInputs { get => handleInputs; set => handleInputs = value; }
    public PlayerController PlayerController { get => playerController; set => playerController = value; }
    public UIPlayerManager PlayerUI { get => playerUI; set => playerUI = value; }

    void Awake()
    {
        photonView = GetComponent<PhotonView>();

        HandleInputs = GetComponent<HandleInputs>();
        HandleAnimations = GetComponentInChildren<HandleAnimations>();
        playerController = GetComponent<PlayerController>();
        playerUI = GetComponentInChildren<UIPlayerManager>();

        if (photonView.IsMine)
            LocalPlayer = this;
    }
}
