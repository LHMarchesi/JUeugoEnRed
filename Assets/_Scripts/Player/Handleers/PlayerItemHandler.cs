using Photon.Pun;
using System;
using System.Net;
using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    public Transform itemHolder;
    public float interactDistance;
    public LayerMask interactableMask;

    private Item currentItem;
    private Item currentWeapon;
    private Camera cam;
    private PhotonView photonView;
    public Transform weaponHolder;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
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
            if (currentItem == null && currentWeapon == null)
            {
                TryPickup();
            }
            else
            {
                DropHeld();
            }
        }
    }

    private void DropHeld()
    {
        if (currentItem == null && currentWeapon == null) return;

        // Si tienes un item 
        if (currentItem != null)
        {
            currentItem.Drop();
            currentItem = null;
        }

        // Si tienes un arma
        if (currentWeapon != null)
        {
            currentWeapon.Drop();
            currentWeapon = null;
        }
    }

    void TryPickup()
    {
        Vector3 rayOrigin = cam.transform.position + cam.transform.forward * 0.5f;
        if (Physics.Raycast(rayOrigin, cam.transform.forward, out RaycastHit hit, interactDistance, interactableMask))
        {
            Iweapon iweapon = hit.collider.GetComponent<Iweapon>();
            Ipickuppeable ipickuppeable = hit.collider.GetComponent<Ipickuppeable>();


            if (iweapon != null)
            {
                if (currentWeapon != null)   // Si ya tenemos un arma, soltamos la anterior
                {
                    currentWeapon.Drop();
                    currentWeapon = null;
                }

                Item Weapon = iweapon as Item;
                currentWeapon = Weapon;

                Weapon.GetComponent<Rigidbody>().isKinematic = true;
                Weapon.transform.SetParent(weaponHolder);
                Weapon.transform.localPosition = Vector3.zero;
                Weapon.transform.localRotation = Quaternion.identity;
            }
            else
             if (ipickuppeable != null)
            {
                if (currentItem != null)  // Si ya tenemos un item, soltamos el anterior
                {
                    currentItem.Drop();
                    currentItem = null;
                }

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

public interface Iweapon
{
    void Attack();
}
