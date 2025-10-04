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
    private void DropHeld()
    {
        if (currentItem != null)
        {
            int viewId = currentItem.GetComponent<PhotonView>().ViewID; // Obtén el ID del PhotonView del objeto que deseas soltar
            Vector3 dropPos = transform.position + transform.forward;
            currentItem.Drop();
            photonView.RPC("DropItem", RpcTarget.AllBuffered, viewId, dropPos); // Llama al método RPC para soltar el objeto en todos los clientes
            currentItem = null;
        }
    }

    void TryPickup()
    {
        if (currentItem != null) return;

        Vector3 rayOrigin = cam.transform.position + cam.transform.forward * 0.5f;
        if (Physics.Raycast(rayOrigin, cam.transform.forward, out RaycastHit hit, interactDistance, interactableMask))
        {
            Ipickuppeable ipickuppeable = hit.collider.GetComponent<Ipickuppeable>();
             if (ipickuppeable != null)
            {
                var pickedUp = ipickuppeable.PickUp();
                int viewId = pickedUp.gameObject.GetComponent<PhotonView>().ViewID; // Obtén el ID del PhotonView del objeto que deseas recoger
               
                photonView.RPC("SetParent", RpcTarget.AllBuffered, viewId); // Llama al método RPC para establecer el padre del objeto en todos los clientes
                currentItem = pickedUp;

                Weapon weapon = pickedUp.GetComponent<Weapon>();
                if (weapon != null)
                {
                    EquipWeapon(weapon);
                }
            }
        }
    }

    [PunRPC]
    private void SetParent(int viewId)  // RPC para establecer el padre del objeto en todos los clientes
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

    public void EquipWeapon(Weapon weapon)
    {
        weapon.PickUp();
        weapon.playerCamera = cam; // referencia de la cámara del jugador
    }
}