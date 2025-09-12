using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviourPunCallbacks
{
    private int iDCount;
    public ObjectPooler objectPooler;
    public Transform spawnPoint;
    public bool canSpawn;
    public float spawnInterval;
    public GameObject[] prefabs;

    void Start()
    {
        iDCount = 0;
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
        GameObject obj = PhotonNetwork.Instantiate(prefabs[Random.Range(0, prefabs.Length)].name, spawnPoint.position, spawnPoint.rotation);
        ItemHandler.Instance.AddToDictionary(iDCount, obj);
    }
}
