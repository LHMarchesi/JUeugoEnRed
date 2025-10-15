using Photon.Pun;
using UnityEngine;

public class DestroyItem : MonoBehaviour
{
    [SerializeField] Transform coalSpawnPosition;
    [SerializeField] GameObject coalPrefab;
    private void OnTriggerEnter(Collider other)
    {
        Ipickuppeable ipickuppeable = other.GetComponent<Ipickuppeable>();
        if (ipickuppeable != null)
        {
            PhotonNetwork.Destroy(other.gameObject);
            PhotonNetwork.Instantiate(coalPrefab.name, coalSpawnPosition.position, Quaternion.identity);
        }
    }
}
