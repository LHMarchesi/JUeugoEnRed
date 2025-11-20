using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInteractionPanel : MonoBehaviourPun, IPointerClickHandler
{
    public RectTransform canvasRect;
   public UIMartilloAnimator martilloAnimator;
    public List<UICucaracha> cucarachas = new List<UICucaracha>();

    private List<(int actor, UICucaracha cuc)> hitBuffer = new();
    private float bufferWindow = 0.1f;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, eventData.position, eventData.pressEventCamera, out var localPoint)) return;

        // chequeo si tocó una cucaracha
        UICucaracha cucHit = null;
        foreach (var cuc in cucarachas)
        {
            if (!cuc.IsAlive) continue;
            if (RectTransformUtility.RectangleContainsScreenPoint(cuc.rect, eventData.position))
            {
                Debug.Log("Cucaracha hit: " + cuc.ID);
                cucHit = cuc;
                break;
            }
        }

        int actor = PhotonNetwork.LocalPlayer.ActorNumber;

        photonView.RPC("RPC_OnClick", RpcTarget.All, actor, localPoint.x, localPoint.y, cucHit ? cucHit.ID : -1);
    }

    [PunRPC]
    void RPC_OnClick(int actor, float x, float y, int cucID)
    {
        UICucaracha cuc = cucID >= 0 ? cucarachas[cucID] : null;

        hitBuffer.Add((actor, cuc));
        StartCoroutine(ProcessBuffer(x, y));
    }

    private IEnumerator ProcessBuffer(float x, float y)
    {
        yield return new WaitForSeconds(bufferWindow);

        if (hitBuffer.Count == 1)
        {
            var (actor, cuc) = hitBuffer[0];

            // un solo jugador golpeó
            if (cuc == null)
                martilloAnimator.HitGround(actor, new Vector2(x, y));
            else
                HitSingle(actor, cuc, new Vector2(x, y));
        }
        else
        {
            // más de uno golpeo casi juntos
            var cuc = hitBuffer[0].cuc;
            if (cuc != null && AllHitSameCucaracha())
            {
                // golpe simultáneo
                martilloAnimator.HitSimultaneous(
                    hitBuffer.ConvertAll(h => h.actor),
                    cuc.rect.anchoredPosition);
           //     cuc.KillSimultaneous();
            }
            else
            {
                // golpes distintos ? procesarlos individualmente
                foreach (var (actor, cucSingle) in hitBuffer)
                {
                    if (cucSingle == null)
                        martilloAnimator.HitGround(actor, new Vector2(x, y));
                    else
                        HitSingle(actor, cucSingle, cucSingle.rect.anchoredPosition);
                }
            }
        }

        hitBuffer.Clear();
    }

    private bool AllHitSameCucaracha()
    {
        int firstID = hitBuffer[0].cuc.ID;
        foreach (var h in hitBuffer)
        {
            if (h.cuc == null || h.cuc.ID != firstID)
                return false;
        }
        return true;
    }

    private void HitSingle(int actor, UICucaracha cuc, Vector2 pos)
    {
        if (cuc.IsAlive)
        {
           // cuc.Kill(actor);
            martilloAnimator.HitCucaracha(actor, pos);
        }
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        gameObject.SetActive(true);
    }
}
