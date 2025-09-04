using TMPro;
using UnityEngine;


public class RecipeManager : MonoBehaviour
{
    [Header("Recetas disponibles")]
    public CraftingRecipe[] allRecipes;

    [Header("Referencias")]
    public Platform craftingPlatform;
    public TextMeshProUGUI uiText;

    private CraftingRecipe currentRecipe;

    private void Start()
    {
        ChooseRandomRecipe();
    }

    private void ChooseRandomRecipe()
    {
        if (allRecipes.Length == 0)
        {
            Debug.LogError("⚠ No hay recetas cargadas en el RecipeManager");
            return;
        }

        int index = Random.Range(0, allRecipes.Length);
        currentRecipe = allRecipes[index];

        // Asignar a la plataforma
        craftingPlatform.currentRecipe = currentRecipe;

        // Mostrar en UI
        uiText.text = "Receta actual: " + currentRecipe.recipeName;
    }
}