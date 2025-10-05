using Photon.Pun;
using UnityEngine;

public interface Iweapon
{
    void Attack();
}

public class Weapon : ItemBase, Iweapon
{
    [Header("Weapon Stats")]
    public float attackRange = 3f;
    public LayerMask hitMask;
    public bool canAttack;
    public Camera playerCamera;
    private PhotonView view;

    public override ItemBase PickUp()
    {
        canAttack = true;
        return this;
    }

    public override void Drop()
    {
        base.Drop();
        canAttack = false;
    }
    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        // Detectar click izquierdo
        if (Input.GetMouseButtonDown(0) && canAttack )
        {
            Attack();
        }
    }

    public void Attack()
    {

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackRange, hitMask))
        {
            Platform platform = hit.collider.GetComponentInParent<Platform>();
            if (platform != null)
            {
                platform.TryCraft();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (playerCamera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * attackRange);
        }
    }
}
