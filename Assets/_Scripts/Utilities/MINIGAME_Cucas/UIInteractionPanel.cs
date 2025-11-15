using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInteractionPanel : MonoBehaviourPun, IPointerClickHandler
{
    public RectTransform canvasRect;
    public UIMartilloAnimator martilloAnimator;
    public RectTransform[] spawnPoints;
    public List<UICucaracha> cucarachasList = new List<UICucaracha>();

    private List<(int actor, UICucaracha cuc)> hitBuffer = new();
    private float bufferWindow = 0.1f;
    private int cucasToSpawn = 3;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            SpawnCucas();
    }

    void SpawnCucas()
    {
        for (int i = 0; i < cucasToSpawn; i++)
        {
            int idx = Random.Range(0, spawnPoints.Length);

            // 1. Siempre instanciar en 0
            GameObject go = PhotonNetwork.Instantiate("UICucaracha", Vector3.zero, Quaternion.identity);

            // 2. SetParent correcto
            go.transform.SetParent(canvasRect, false);

            // 3. El master asigna el spawn real
            if (PhotonNetwork.IsMasterClient)
            {
                RectTransform r = go.GetComponent<RectTransform>();
                r.anchoredPosition = spawnPoints[idx].anchoredPosition;
            }

            UICucaracha cuc = go.GetComponent<UICucaracha>();
            cucarachasList.Add(cuc);
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 pos = eventData.position;

        // chequeo si tocó una cucaracha
        UICucaracha cucHit = null;
        foreach (var cuc in cucarachasList)
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

        photonView.RPC("RPC_OnClick", RpcTarget.All, actor, pos.x, pos.y, cucHit ? cucHit.view.ViewID : -1);
    }


    [PunRPC]
    IEnumerator RPC_OnClick(int actor, float x, float y, int cucViewID)
    {
        while (!gameObject.activeInHierarchy)
            yield return null;

        Debug.Log("Click recibido en todos los jugadores");

        UICucaracha cuc = null;

        if (cucViewID >= 0)
        {
            PhotonView pv = PhotonView.Find(cucViewID);
            if (pv != null)
            {
                cuc = pv.GetComponent<UICucaracha>();
            }
        }

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
                cuc.KillSimultaneous();
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
            cuc.Kill(actor);
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
