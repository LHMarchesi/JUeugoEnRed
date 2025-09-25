using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Transform downHolder;
    public Transform middleHolder;
    public Transform topHolder;
    public Transform craftedItemSpawn;

    [Header("Receta actual")]
    public CraftingRecipe currentRecipe = null;

   // private Dictionary<ItemType, ItemBase> placedItems = new Dictionary<ItemType, ItemBase>();
    private List<ItemType> totalItems = new List<ItemType>();
    private List<ItemBase> items = new List<ItemBase>();


    private void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (currentRecipe == null) return;

            ItemBase item = other.GetComponent<ItemBase>();
            if (item != null)
            {
                // Chequear el holder segun el tipo
                item.Drop(); // Soltar el item si estaba en mano
                Transform targetHolder = null;
                switch (item.stats.type)
                {
                    case ItemType.down: targetHolder = downHolder; break;
                    case ItemType.middle: targetHolder = middleHolder; break;
                    case ItemType.top: targetHolder = topHolder; break;
                }

                if (targetHolder != null)
                {
                    Rigidbody rb = item.GetComponent<Rigidbody>();
                    if (rb != null) rb.isKinematic = true;

                    //Vector3 originalScale = item.transform.localScale;

                    item.transform.SetParent(targetHolder, true);
                    item.transform.localPosition = Vector3.zero;
                    //item.transform.localScale = originalScale;

                    totalItems.Add(item.stats.type);
                    items.Add(item);

                }
                Debug.Log(items.Count + " items gameobject " + totalItems.Count + " items enums");
            }
        }
    }

    private void Update()
    {
        downHolder.transform.Rotate(Vector3.up * Time.deltaTime * 20);
        topHolder.transform.Rotate(Vector3.up * Time.deltaTime * 20);
        middleHolder.transform.Rotate(Vector3.up * Time.deltaTime * 20);
    }

    public bool HasAllPieces()
    {
        // Comprobar que todos los items de la receta están presentes
        foreach (var required in currentRecipe.requiredItems)
        {
            if (!totalItems.Contains(required.type)) return false;

            //if (placedItems[required.type].stats != required)
            //return false; // mismo tipo pero distinto item
        }
        return true;
    }

    public void TryCraft()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (HasAllPieces())
            {
                PhotonNetwork.Instantiate(currentRecipe.finalItemPrefab.name,
                            craftedItemSpawn.position + Vector3.up * 2,
                            Quaternion.identity);

                ClearPlatform();
            }
            else
            {
                Debug.Log(" Piezas incorrectas, se destruye todo.");
                ClearPlatform();
            }
        }
    }

    private void ClearPlatform()
    {

        for (int i = 0; i < totalItems.Count; i++)
        {
            Destroy(items[i].gameObject);
        }
        /*if (kvp.Value != null)
        {
            ObjectPooler pool = kvp.Value.GetPool();
            if (pool != null)
            {
                pool.ReleaseObject(kvp.Value.gameObject);
            }
        }
        */
        items.Clear();
        totalItems.Clear();
        //placedItems.Clear();
        Debug.Log(items.Count + " items gameobject " + totalItems.Count + " items enums");
    }

    public void SetRecipe(CraftingRecipe recipe)
    {
        currentRecipe = recipe;
    }
}