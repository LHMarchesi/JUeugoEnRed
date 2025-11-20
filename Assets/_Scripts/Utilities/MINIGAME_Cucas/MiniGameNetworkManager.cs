using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniGameNetworkManager : MonoBehaviourPunCallbacks
{

    public UIMartilloAnimator martilloAnimator;
    public GameObject UIcuca;
    public RectTransform[] spawnPoints;
    //
    private List<(int actor, UICucaracha cuc)> hitBuffer = new();
    private float bufferWindow = 0.1f;
    private int cucasToSpawn = 3;

    private void Start()
    {
        Debug.Log("Minigame Cucas started");
        SpawnCucas(); // solo el master instancia
    }

    void SpawnCucas()
    {
        for (int i = 0; i < cucasToSpawn; i++)
        {
            int idx = Random.Range(0, spawnPoints.Length);

            GameObject go = PhotonNetwork.Instantiate(UIcuca.name, Vector3.zero, Quaternion.identity);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 pos = eventData.position;
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        photonView.RPC("RPC_OnClick", RpcTarget.All, actorNumber, pos.x, pos.y);
    }


    [PunRPC]
    void RPC_OnClick(int actor, float x, float y)
    {
        Debug.Log("Click on: " + x + ", " + y + " by actor " + actor);
        //  hitBuffer.Add((actor));
        //StartCoroutine(ProcessBuffer(x, y)); // inicio el buffer para procesar los hits juntos
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
                martilloAnimator.HitSimultaneous(hitBuffer.ConvertAll(h => h.actor), cuc.rect.anchoredPosition);
                //    cuc.KillSimultaneous();
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
    private void HitSingle(int actor, UICucaracha cuc, Vector2 pos)
    {
        if (cuc.IsAlive)
        {
            //    cuc.Kill(actor);
            martilloAnimator.HitCucaracha(actor, pos);
        }
    }

    private bool AllHitSameCucaracha()
    {
        //   int firstID = hitBuffer[0].cuc.ID;
        foreach (var h in hitBuffer)
        {
            //      if (h.cuc == null || h.cuc.ID != firstID)
            return false;
        }
        return true;
    }
}
