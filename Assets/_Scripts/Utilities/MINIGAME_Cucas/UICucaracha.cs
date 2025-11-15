using Photon.Pun;
using UnityEngine;

public class UICucaracha : MonoBehaviour, IPunObservable
{
    public bool IsAlive = true;
    public RectTransform rect;
    public PhotonView view;
    public int ID;

    public float speed;

    private Vector2 direction;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        rect = GetComponentInParent<RectTransform>();

        if (PhotonNetwork.IsMasterClient)
            direction = Random.insideUnitCircle.normalized;

        ID = view.ViewID;
    }

    private void Update()
    {
        if (!IsAlive) return;

        // Solo el MasterClient genera movimiento
        if (PhotonNetwork.IsMasterClient)
        {
            // Movimiento random
            rect.anchoredPosition += direction * speed * Time.deltaTime;

            // Rebote contra bordes
            if (Mathf.Abs(rect.anchoredPosition.x) > 350f)
                direction.x *= -1;

            if (Mathf.Abs(rect.anchoredPosition.y) > 200)
                direction.y *= -1;
        }
    }

    // todos reciben: posición, dirección y estado de vida
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

    public void Kill(int actor)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        view.RPC("RPC_Kill", RpcTarget.AllBuffered, actor);
    }

    public void KillSimultaneous()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        view.RPC("RPC_KillSimultaneous", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RPC_Kill(int actor)
    {
        if (!IsAlive) return;

        IsAlive = false;
        gameObject.SetActive(false);
    }

    [PunRPC]
    private void RPC_KillSimultaneous()
    {
        if (!IsAlive) return;

        Debug.Log("RPC: Cuca simultánea destruida");
        IsAlive = false;
        gameObject.SetActive(false);
    }
}
