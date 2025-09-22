using Photon.Pun;
using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    [SerializeField] private Transform itemHolder;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private float interactDistance;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private Camera cam;

    private ItemBase currentItem;
    private Weapon currentWeapon;
    private PlayerContext playerContext;
    private PhotonView photonView;

    void Start()
    {
        playerContext = GetComponent<PlayerContext>();
        photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            cam.enabled = false;
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (playerContext.HandleInputs.IsInteracting())
            {
                TryPickup();
            }

            if (playerContext.HandleInputs.DropInput())
            {
                DropHeld();
            }
        }
    }

    [PunRPC]
    private void DropItem(int viewId, Vector3 dropPos)
    {
        PhotonView view = PhotonView.Find(viewId);
        if (view != null)
        {
            ItemBase item = view.GetComponent<ItemBase>();
            item.transform.SetParent(null);
            item.GetComponent<Rigidbody>().isKinematic = false;
            item.transform.position = dropPos;
        }
    }
    private void DropHeld()
    {
        if (currentItem != null)
        {
            int viewId = currentItem.GetComponent<PhotonView>().ViewID;
            Vector3 dropPos = transform.position + transform.forward;
            photonView.RPC("DropItem", RpcTarget.All, viewId, dropPos);
            currentItem = null;
        }
    }

    void TryPickup()
    {
        if (currentItem != null || currentWeapon != null) return;

        Vector3 rayOrigin = cam.transform.position + cam.transform.forward * 0.5f;
        if (Physics.Raycast(rayOrigin, cam.transform.forward, out RaycastHit hit, interactDistance, interactableMask))
        {
            Ipickuppeable ipickuppeable = hit.collider.GetComponent<Ipickuppeable>();

            if (ipickuppeable != null)
            {
                ItemBase itemPicked = ipickuppeable.PickUp();
                int viewId = itemPicked.gameObject.GetComponent<PhotonView>().ViewID;
                Debug.Log(viewId);
                photonView.RPC("SetParent", RpcTarget.All, viewId);
                currentItem = itemPicked;

            }
        }
    }

    [PunRPC]
    private void SetParent(int viewId)
    {
        PhotonView view = PhotonView.Find(viewId);
        if (view != null)
        {
            ItemBase item = view.GetComponent<ItemBase>();
            item.transform.SetParent(itemHolder);
            item.GetComponent<Rigidbody>().isKinematic = true;
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        weapon.playerCamera = cam; // referencia de la cámara del jugador
        weapon.canAttack = true;
    }
}