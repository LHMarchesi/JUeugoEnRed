using Photon.Pun;
using UnityEngine;
using UnityEngine.Analytics;
using Unity.Services.Analytics;

public class Spawner : MonoBehaviourPunCallbacks
{
    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public int maxObjects = 15;

    [Header("Interval Settings")]
    public float minInitialInterval;
    public float maxInitialInterval;

    public float minIntervalIncrease;
    public float maxIntervalIncrease;

    [Header("Prefabs")]
    public GameObject[] prefabs;

    private int spawnedCount = 0;
    private float currentInterval;

    public GameObject MinigameCuca;
    public float spawnDelay = 60;
    public bool hasSpawned = false;
    void Start()
    {
        
        if (!PhotonNetwork.IsMasterClient)
            return;

        // Intervalo inicial aleatorio
        currentInterval = Random.Range(minInitialInterval, maxInitialInterval);
        currentInterval = Mathf.Abs(currentInterval);
        // Calculo delay de minijuego
        //float delay = Random.Range(10, 21) + currentInterval * 2 + 1;
        //spawnDelay += delay;
    }

    public void StartSpawning()
    {
        Invoke(nameof(SpawnObject), currentInterval);
    }

    public void SpawnObject()
    {
        if (spawnedCount >= maxObjects)
        {
            Debug.Log("Se alcanzó el máximo de objetos.");
            return;
        }

        // Spawn aleatorio
        string prefab = prefabs[Random.Range(0, prefabs.Length)].name;
        PhotonNetwork.Instantiate(
            prefab,
            spawnPoint.position,
            spawnPoint.rotation
        );
        
        CustomEvent myEvent = new CustomEvent("COMPONENT_SPAWNED")
        {
            { "ComponentType", prefab }
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        Analytics.FlushEvents();
        Debug.Log("objeto" + prefab + "creado");

    spawnedCount++;

        // Aumento aleatorio del intervalo
        float randomIncrease = Random.Range(minIntervalIncrease, maxIntervalIncrease);
        currentInterval += randomIncrease;

        // Nuevo intervalo también aleatorio entre min y max (si querés una variación constante)
        currentInterval = Mathf.Clamp(
            Random.Range(minInitialInterval, maxInitialInterval) + randomIncrease,
            minInitialInterval,
            maxInitialInterval * 3f // por si el aumento lo hace crecer
        );

        // Invocar el siguiente spawn
        Invoke(nameof(SpawnObject), currentInterval);
    }

    public void SpawnMinigame() 
    {       
        if (!hasSpawned)
        {
            hasSpawned = true;
            Invoke(nameof(SpawnCuca), spawnDelay);
        }

    }
    public void SpawnCuca()
    {
        MinigameCuca.SetActive(true);
    }
}
