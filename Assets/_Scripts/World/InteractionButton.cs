using Photon.Pun;
using System.Collections;
using UnityEngine;

public class InteractionButton : MonoBehaviourPun, IInteractive
{
    private bool isOn = false;
    private bool isBusy = false;
    public Material greenMaterial;
    private Material defaultMaterial;
    PlayerContext lastInteractedPlayer;
    public bool IsOn { get => isOn; private set { } }

    private void Awake()
    {
        defaultMaterial = gameObject.GetComponent<Renderer>().material;
    }

    public void Interact(PlayerContext player)
    {
        if (isBusy) return; // evita spam de interacción

        lastInteractedPlayer = player;
        isBusy = true;
        photonView.RPC("ToggleInteraction", RpcTarget.AllBuffered, true);
        StartCoroutine(InteractionSequence());
    }

    public void SetlastInteractedPlayerNull()
    {
        lastInteractedPlayer = null;
    }

    private IEnumerator InteractionSequence()
    {
        // Mantiene la caja abierta 2 segundos
        yield return new WaitForSecondsRealtime(2f);

        photonView.RPC("ToggleInteraction", RpcTarget.AllBuffered, false);
        isBusy = false;
    }

    [PunRPC]
    private void ToggleInteraction(bool value)
    {
        isOn = value;
        gameObject.GetComponent<Renderer>().material = isOn ? greenMaterial : defaultMaterial;

        // animator.SetBool("isOn", isOn);
    }

    public PlayerContext LastInteractedPlayer { get { return lastInteractedPlayer; } }
}