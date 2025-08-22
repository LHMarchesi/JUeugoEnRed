using Photon.Pun;
using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    public Transform itemHolder;         
    public float interactDistance;  
    public LayerMask interactableMask;    

    private GameObject currentItem;
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
        if (Input.GetMouseButtonDown(1)) // Bot�n derecho
        {
            if (currentItem == null)
            {
                TryPickup();
            }
            else
            {
                DropItem();
            }
        }
    }

    void TryPickup()
    {
        Vector3 rayOrigin = cam.transform.position + cam.transform.forward * 0.5f;
        if (Physics.Raycast(rayOrigin, cam.transform.forward, out RaycastHit hit, interactDistance, interactableMask))
        {
            if (hit.collider.CompareTag("PickUp")) // Solo objetos con esta etiqueta
            {
                currentItem = hit.collider.gameObject;
                currentItem.GetComponent<Rigidbody>().isKinematic = true; // Evita f�sica
                currentItem.transform.SetParent(itemHolder);
                currentItem.transform.localPosition = Vector3.zero;
                currentItem.transform.localRotation = Quaternion.identity;
            }
        }
    }

    void DropItem()
    {
        currentItem.transform.SetParent(null);
        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(cam.transform.forward * 2f, ForceMode.Impulse); // Opcional, que caiga adelante
        currentItem = null;
    }
}
