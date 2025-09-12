using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
    [Header("Pool Settings")]
    public GameObject[] prefabs;
    public int poolSizePerPrefab = 3;
    public PhotonView view;
    private Dictionary<string, List<GameObject>> pools;

    public override void Awake()
    {
 
        
            //Debug.Log("i am master");
            pools = new Dictionary<string, List<GameObject>>();

            foreach (var prefab in prefabs)
            {
                List<GameObject> prefabPool = new List<GameObject>();
                for (int i = 0; i < poolSizePerPrefab; i++)
                {
                    GameObject obj = PhotonNetwork.Instantiate(prefab.name, transform.position, Quaternion.identity);
                    obj.SetActive(false);
                    prefabPool.Add(obj);
                }

                pools.Add(prefab.name, prefabPool);

                Item item = prefab.GetComponent<Item>();
                if (item != null)
                    item.SetPool(this);
            
            }
        
        
    }

    public GameObject GetRandomPooledObject()
    {
        if (prefabs.Length == 0)
        {
            return null;
        }

        // Elegir prefab aleatoriamente
        int randomIndex = Random.Range(0, prefabs.Length);
        string selectedName = prefabs[randomIndex].name;

        if (!pools.ContainsKey(selectedName))
        {
            Debug.LogWarning($"No pool found for {selectedName}");
            return null;
        }

        // Buscar objeto inactivo en ese pool
        foreach (var obj in pools[selectedName])
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        return null;
    }

    public void ReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform); // opcional: para mantener orden en jerarquía
    }
}