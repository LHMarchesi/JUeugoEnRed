using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 direction = Vector3.right;
    PlayerController playerController;

    private void OnCollisionStay(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb != null)
        {
            Vector3 move = direction.normalized * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }
        else if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Player on conveyor belt");
            playerController = collision.collider.GetComponent<PlayerController>();
            playerController.SetConveyorSpeed(direction.normalized * speed);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerController.SetConveyorSpeed(Vector3.zero);
        }
    }
}