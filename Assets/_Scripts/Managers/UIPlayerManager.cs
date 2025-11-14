using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerManager : Singleton<UIPlayerManager>
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject recipePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI winnerNameText;
    [SerializeField] private Image recipeImageUI;
    [SerializeField] private TextMeshProUGUI recipeTextUI;
    private void Awake()
    {
        ShowWinScreen(false);
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

    public void ShowWinScreen(bool value, string winnerNickame = null)
    {
        winPanel.SetActive(value);
        if (!value) { return; }
        
        PlayerContext playerContext = GetComponent<PlayerContext>();
        playerContext.HandleInputs.SetPaused(true);
        winnerNameText.text = winnerNickame;
    }

    public void HideRecipe()
    {
        recipePanel.SetActive(false);
    }

    public void TogglePauseScreen(bool value)
    {
        pausePanel.gameObject.SetActive(value);
        if (value)
        {
            // Mostrar y desbloquear el cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Ocultar y bloquear el cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
