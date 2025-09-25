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

    private Dictionary<ItemType, ItemBase> placedItems = new Dictionary<ItemType, ItemBase>();


    private void OnTriggerEnter(Collider other)
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

                placedItems[item.stats.type] = item;
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
            if (!placedItems.ContainsKey(required.type)) return false;

            if (placedItems[required.type].stats != required)
                return false; // mismo tipo pero distinto item
        }
        return true;
    }

    public void TryCraft()
    {
        if (HasAllPieces())
        {
            Instantiate(currentRecipe.finalItemPrefab,
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

    private void ClearPlatform()
    {
        foreach (var kvp in placedItems)
        {
            Destroy(kvp.Value.gameObject);
            /*if (kvp.Value != null)
            {
                ObjectPooler pool = kvp.Value.GetPool();
                if (pool != null)
                {
                    pool.ReleaseObject(kvp.Value.gameObject);
                }
            }
            */
        }
        placedItems.Clear();
    }

    public void SetRecipe(CraftingRecipe recipe)
    {
        currentRecipe = recipe;
    }
}