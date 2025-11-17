using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using System;
using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    [SerializeField] private Transform itemHolder;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private float interactDistance;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private Camera cam;
    private PlayerItemHandler playerItemHandler;

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
        Debug.Log("PlayerItemHandler: " + this);
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (playerContext.HandleInputs.IsInteracting())
            {
                TryInteract();
            }

            if (playerContext.HandleInputs.DropInput())
            {
                DropHeld();
            }
        }
    }

    public void DropHeld()
    {
        if (currentItem != null)
        {
            int viewId = currentItem.GetComponent<PhotonView>().ViewID; // Obtén el ID del PhotonView del objeto que deseas soltar
            Vector3 dropPos = transform.position + transform.forward;
            currentItem.Drop();
            photonView.RPC("DropItem", RpcTarget.AllBuffered, viewId, dropPos); // Llama al método RPC para soltar el objeto en todos los clientes
            currentItem = null;
        }
        else if (currentWeapon != null)
        {
            int viewId = currentWeapon.GetComponent<PhotonView>().ViewID;
            Vector3 dropPos = transform.position + transform.forward;
            photonView.RPC("DropItem", RpcTarget.AllBuffered, viewId, dropPos);
            currentWeapon.Drop();
            currentWeapon = null;
        }
    }

    void TryInteract()
    {
        if (currentItem != null || currentWeapon != null) return;

        Vector3 rayOrigin = cam.transform.position + cam.transform.forward * 0.5f;
        if (Physics.Raycast(rayOrigin, cam.transform.forward, out RaycastHit hit, interactDistance, interactableMask))
        {

            IInteractive interactable = hit.collider.GetComponent<IInteractive>();
            if (interactable != null)
            {
                interactable.Interact(playerContext);
                return;
            }

            Iweapon iWeapon = hit.collider.GetComponent<Iweapon>();
            if (iWeapon != null)
            {
                var weaponPicked =iWeapon.PickUp(this);
                int viewId = weaponPicked.gameObject.GetComponent<PhotonView>().ViewID; // ID del PhotonView del objeto que deseas recoger
                photonView.RPC("RemoveOriginalParent", RpcTarget.AllBuffered, viewId);
                photonView.RPC("WeaponSetParent", RpcTarget.AllBuffered, viewId); // Llama al método RPC para establecer el padre del objeto en todos los clientes
                weaponPicked.playerCamera = cam;
                currentWeapon = weaponPicked;
            }

            Ipickuppeable ipickuppeable = hit.collider.GetComponent<Ipickuppeable>();
            if (ipickuppeable != null)
            {
                var pickedUp = ipickuppeable.PickUp(this);
                int viewId = pickedUp.gameObject.GetComponent<PhotonView>().ViewID; // ID del PhotonView del objeto que deseas recoger
                photonView.RPC("RemoveOriginalParent", RpcTarget.AllBuffered, viewId);
                photonView.RPC("ItemSetParent", RpcTarget.AllBuffered, viewId);
                currentItem = pickedUp;
            }
        }
    }

    [PunRPC]
    private void ItemSetParent(int viewId)  // RPC para establecer el padre del objeto en todos los clientes
    {
        PhotonView view = PhotonView.Find(viewId);
        if (view != null)
        {
            var item = view.gameObject;
            item.transform.SetParent(itemHolder);
            item.GetComponent<Rigidbody>().isKinematic = true;
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
        }
    }

    [PunRPC]
    private void WeaponSetParent(int viewId)  // RPC para establecer el padre del objeto en todos los clientes
    {
        PhotonView view = PhotonView.Find(viewId);
        if (view != null)
        {
            var item = view.gameObject;
            item.transform.SetParent(weaponHolder);
            item.GetComponent<Rigidbody>().isKinematic = true;
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
        }
    }

    [PunRPC]
    private void RemoveOriginalParent(int viewId)
    {
        PhotonView view = PhotonView.Find(viewId);
        if (view != null)
        {
            var item = view.gameObject;
            PlayerItemHandler owner = item.GetComponentInParent<PlayerItemHandler>();
            if (owner != null)
                owner.SetItemNull();
        }
    }


    [PunRPC]
    private void DropItem(int viewId, Vector3 dropPos)
    {
        PhotonView view = PhotonView.Find(viewId);  // Encuentra el PhotonView usando el ID
        if (view != null)
        {
            if (currentItem is KidCard)
            {
                UIPlayerManager.Instance.HideRecipe();
            }
            var item = view.gameObject;
            item.GetComponent<Rigidbody>().isKinematic = false;
            item.transform.SetParent(null);
            item.transform.position = dropPos;
        }
    }

    public void SetItemNull()
    {
        currentItem = null;
        currentItem.transform.SetParent(null);
    }
}
