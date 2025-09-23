using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject recipePanel;
    [SerializeField] private Image recipeImageUI;
    [SerializeField] private TextMeshProUGUI recipeTextUI;

    private void Start()
    {
        // Asegúrate que el panel esté oculto al inicio
        recipePanel.SetActive(false);
    }

    public void ShowRecipe(CraftingRecipe recipe)
    {
        if (recipe == null) return;

        recipePanel.SetActive(true);

        // Texto
        string ingredientesTexto = "";
        foreach (var item in recipe.requiredItems)
        {
            ingredientesTexto += $"\n• {item.itemName}";
        }

        recipeTextUI.text = $"Receta: {recipe.recipeName}\nIngredientes:{ingredientesTexto}";

        // Imagen
        recipeImageUI.sprite = recipe.recipeIcon;
    }

    public void HideRecipe()
    {
        recipePanel.SetActive(false);
    }
}
