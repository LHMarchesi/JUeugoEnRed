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
    private Weapon currentWeapon;
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

                Weapon weaponPicked = iweapon as Weapon;   // casteo directo a Weapon
                currentWeapon = weaponPicked;

                weaponPicked.GetComponent<Rigidbody>().isKinematic = true;
                weaponPicked.transform.SetParent(weaponHolder);
                weaponPicked.transform.localPosition = Vector3.zero;
                weaponPicked.transform.localRotation = Quaternion.identity;

                EquipWeapon(weaponPicked);
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

    public void EquipWeapon(Weapon weapon)
    {
        weapon.playerCamera = cam; // referencia de la cámara del jugador
        weapon.canAttack = true;
    }
}