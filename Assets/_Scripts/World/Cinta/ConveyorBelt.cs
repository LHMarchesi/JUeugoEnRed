using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform direccion;

    private void OnTriggerStay(Collider other)
    {
        ItemBase item = other.GetComponent<ItemBase>();
        if (item != null && !item.isHeld)
        {
            Transform itemTransform = other.transform;

            // Mueve el objeto hacia la posición de "direccion"
            itemTransform.position = Vector3.MoveTowards(
                itemTransform.position,          // posición actual
                direccion.position,              // posición objetivo
                speed * Time.deltaTime           // velocidad de movimiento
            );
        }
    }
}