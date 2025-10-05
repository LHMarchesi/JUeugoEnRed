using UnityEngine;
using Photon.Pun;
using System.Collections;

public class LeverScript : MonoBehaviourPun, IInteractive
{
    [SerializeField] private GiftboxScript linkedGiftBox;
    private bool isOn = false;
    private bool isBusy = false;

    public void Interact(PlayerContext player)
    {
        if (isBusy) return; // evita spam de interacción
        if (!photonView.IsMine) return;

        isBusy = true;
        photonView.RPC("ToggleLever", RpcTarget.AllBuffered, true);
        StartCoroutine(LeverSequence());
        Debug.Log("Interaction");
    }

    private IEnumerator LeverSequence()
    {
        // Mantiene la caja abierta 2 segundos
        yield return new WaitForSecondsRealtime(2f);

        photonView.RPC("ToggleLever", RpcTarget.AllBuffered, false);
        isBusy = false;
    }

    [PunRPC]
    private void ToggleLever(bool value)
    {
        isOn = value;

        if (isOn)
        {
            linkedGiftBox.OpenBox();
        }
        else
        {
            linkedGiftBox.CloseBox();
        }

        // Si tienes animador:
        // animator.SetBool("isOn", isOn);
    }
}