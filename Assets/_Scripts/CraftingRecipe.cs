using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Header("Configuracion de reseta")]
    public string recipeName;
    public RawImage recipeImage;
    public GameObject finalItemPrefab;

    [Header("Piezas requeridas")]
    public List<ItemStats> requiredItems;

}