using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviourPunCallbacks
{
    public Transform spawnPoint;
    public bool canSpawn;
    public float spawnInterval;
    public GameObject[] prefabs;

    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (canSpawn)
        {
            StartSpawning();
        }
    }

    public void StartSpawning()
    {
        InvokeRepeating(nameof(SpawnObject), 0f, spawnInterval);
    }

    public void StopSpawning()
    {
        CancelInvoke(nameof(SpawnObject));
    }

    public void SpawnObject()
    {
        Debug.Log("Spawning object...");
        GameObject obj = Instantiate(prefabs[Random.Range(0, prefabs.Length)], spawnPoint.position, spawnPoint.rotation);
    }
}
