using UnityEngine;

[CreateAssetMenu(fileName = "ItemStats", menuName = "Items/CreateNewItem")]
public class ItemStats : ScriptableObject
{
    public ItemType type;
    public string itemName;
    public string description;
}
