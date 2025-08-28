using Photon.Pun;
using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    public Transform itemHolder;
    public float interactDistance;
    public LayerMask interactableMask;

    private Item currentItem;
    private Camera cam;
    private PhotonView photonView;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        if (!photonView.IsMine)
        {
            GetComponent<Camera>().enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            if (currentItem == null)
            {
                TryPickup();
            }
            else
            {
                currentItem.Drop();
                currentItem = null;
            }
        }
    }

    void TryPickup()
    {
        Vector3 rayOrigin = cam.transform.position + cam.transform.forward * 0.5f;
        if (Physics.Raycast(rayOrigin, cam.transform.forward, out RaycastHit hit, interactDistance, interactableMask))
        {
            Ipickuppeable ipickuppeable = hit.collider.GetComponent<Ipickuppeable>();
            if (ipickuppeable != null) 
            {
                Item itemPicked = ipickuppeable.PickUp();
                currentItem = itemPicked;

                itemPicked.GetComponent<Rigidbody>().isKinematic = true;
                itemPicked.transform.SetParent(itemHolder);
                itemPicked.transform.localPosition = Vector3.zero;
                itemPicked.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
