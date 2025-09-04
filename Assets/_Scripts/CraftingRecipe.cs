using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string recipeName;
    public GameObject finalItemPrefab;

    [Header("Piezas requeridas")]
    public List<ItemStats> requiredItems;
}