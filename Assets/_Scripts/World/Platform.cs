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
    public RecipeTrigger recipeTrigger;
    public PhotonView view;

    private List<ItemType> totalItems = new List<ItemType>();
    private List<ItemBase> items = new List<ItemBase>();
    int index = 0;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
            if (recipeTrigger.craftingRecipeOnTrigger == null) return;

            currentRecipe = recipeTrigger.craftingRecipeOnTrigger;

            ItemBase item = other.GetComponent<ItemBase>();
            if (item != null)
            {
                int viewID = item.gameObject.GetComponent<PhotonView>().ViewID;
            // Chequear el holder segun el tipo
            //item.Drop(); // Soltar el item si estaba en mano
                view.RPC("PutPiece", RpcTarget.AllBuffered, viewID);
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
        
            if (HasAllPieces())
            {
                PhotonNetwork.Instantiate(currentRecipe.finalItemPrefab.name,
                            craftedItemSpawn.position + Vector3.up * 2,
                            Quaternion.identity);

                view.RPC("ClearPlatform", RpcTarget.AllBuffered);
            }
            else
            {
                Debug.Log(" Piezas incorrectas, se destruye todo.");
                view.RPC("ClearPlatform", RpcTarget.AllBuffered);
            }
        
    }

    [PunRPC]
    public void ClearPlatform()
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

    [PunRPC]
    public void PutPiece(int ID)
    {
        PhotonView view = PhotonView.Find(ID);
        ItemBase item = view.GetComponent<ItemBase>();

        if (items.Count < 3)
        {
            items.Add(item);
            totalItems.Add(item.stats.type);
            AddItemToHolder(item, items.Count -1);
        }
        else
        {
            ItemOverflow(item);
        }

        /*switch (item.stats.type)
        {
            case ItemType.down: targetHolder = downHolder; break;
            case ItemType.middle: targetHolder = middleHolder; break;
            case ItemType.top: targetHolder = topHolder; break;
        }*/
        

        
        
    }

    public void SetRecipe(CraftingRecipe recipe)
    {
        currentRecipe = recipe;
    }
    private void AddItemToHolder(ItemBase item, int holderIndex)
    {
        Debug.Log("index: " + holderIndex);
        int trueIndex = holderIndex % 3;
        Transform holder = trueIndex switch
        {
            0 => downHolder,
            1 => middleHolder,
            2 => topHolder,
            _ => null
        };

        if (holder == null) return;

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        item.transform.SetParent(holder);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        items.Add(item);
        totalItems.Add(item.stats.type);
    }

    public void ItemOverflow(ItemBase newItem)
    {
        ItemBase oldItem = items[0];

        // Detach and drop to the floor
        oldItem.transform.SetParent(null);
        Rigidbody rb = oldItem.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = false;

        // Maybe add force to drop it out of the platform
        rb?.AddForce(Vector3.up * 2f, ForceMode.Impulse);


        items.RemoveAt(0);
        totalItems.RemoveAt(0);

        // 2) Shift remaining items to new holders
        for (int i = 0; i < items.Count; i++)
        {
            AddItemToHolder(items[i], i);
        }

        // 3) Place the new item in the last holder
        AddItemToHolder(newItem, items.Count);
    }
}
    
