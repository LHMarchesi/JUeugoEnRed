using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : Singleton<ItemHandler>
{
    public Dictionary<int, Item> Items;
    private int iDCount;

    public void AddToDictionary(int index, GameObject obj)
    {
        Item item = obj.GetComponent<Item>();
        item.ID = iDCount;
        Items.Add(iDCount, Items[iDCount]);
        iDCount++;
    }

    public Item GetFromDictionary(int index) { 
        Items.TryGetValue(index, out Item item);
        return item;
    }
}