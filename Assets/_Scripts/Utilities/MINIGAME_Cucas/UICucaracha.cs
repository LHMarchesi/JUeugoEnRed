using Photon.Pun;
using UnityEngine;

public class UICucaracha : MonoBehaviour
{
    public bool IsAlive = true;
    public RectTransform rect;
    public PhotonView view;
    public int ID;
    public float speed;
    private Vector2 direction;

    void Awake()
    {
        view = GetComponent<PhotonView>();
        rect = GetComponent<RectTransform>();
        transform.SetParent(UIPlayerManager.Instance.spawnCanvasRect, false);
        if (PhotonNetwork.IsMasterClient)
            direction = Random.insideUnitCircle.normalized;
    }

    void Update()
    {
        if (!IsAlive) return;

        if (PhotonNetwork.IsMasterClient)
        {
            rect.anchoredPosition += direction * speed * Time.deltaTime;

            if (Mathf.Abs(rect.anchoredPosition.x) > 600) direction.x *= -1;
            if (Mathf.Abs(rect.anchoredPosition.y) > 600) direction.y *= -1;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rect.anchoredPosition);
            stream.SendNext(direction);
            stream.SendNext(IsAlive);
        }
        else
        {
            rect.anchoredPosition = (Vector2)stream.ReceiveNext();
            direction = (Vector2)stream.ReceiveNext();
            IsAlive = (bool)stream.ReceiveNext();

            gameObject.SetActive(IsAlive);
        }
    }

    public void Kill()
    {
        view.RPC("RPC_Kill", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void RPC_Kill()
    {
        if (!IsAlive) return;
        IsAlive = false;
        gameObject.SetActive(false);
    }

}
