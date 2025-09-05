using UnityEngine;

public class Spawner : MonoBehaviour
{
    public ObjectPooler objectPooler;
    public Transform spawnPoint;
    public bool canSpawn;
    public float spawnInterval;

    void Start()
    {
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
        GameObject obj = objectPooler.GetRandomPooledObject();
        if (obj != null)
        {
            obj.transform.position = spawnPoint.position;
            obj.transform.rotation = spawnPoint.rotation;
            obj.SetActive(true);
        }
    }        
}