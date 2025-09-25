using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GiftboxScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Ipickuppeable ipickuppeable = other.GetComponent<Ipickuppeable>();
        if (ipickuppeable != null)
        {
            PhotonNetwork.Destroy(other.gameObject);

        }
    }
}
