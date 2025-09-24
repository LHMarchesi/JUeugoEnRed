using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerManager : Singleton<UIPlayerManager>
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject recipePanel;
    [SerializeField] private Image recipeImageUI;
    [SerializeField] private TextMeshProUGUI recipeTextUI;
    [SerializeField] private PhotonView view;

    private void Start()
    {
        // Asegúrate que el panel esté oculto al inicio
        recipePanel.SetActive(false);
    }

    public void ShowRecipe(CraftingRecipe recipe)
    {
        if (view != null && view.IsMine)
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
    }

    public void HideRecipe()
    {
        if (view != null && view.IsMine)
        {
            recipePanel.SetActive(false);
        }
    }
}
