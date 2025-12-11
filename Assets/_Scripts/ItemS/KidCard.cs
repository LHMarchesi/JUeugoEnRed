using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class KidCard : ItemBase
{
    public TextMeshProUGUI recipeName;
    public TextMeshProUGUI recipeIngredients;
    public Image Icon;
    public CraftingRecipe[] allRecipes;
    public PhotonView view;
    public bool isLocal;

    private CraftingRecipe currentRecipe;
    private void Start()
    {
        if (isLocal)
        {
            int index = UnityEngine.Random.Range(0, allRecipes.Length);
            SetRecipe(index);
        } else
        if (PhotonNetwork.IsMasterClient)
        {
            // Asignar receta aleatoria
            int index = UnityEngine.Random.Range(0, allRecipes.Length);
            view.RPC("SetRecipe", RpcTarget.AllBuffered, index);
        }
    }

    public override ItemBase PickUp(PlayerItemHandler playerHolder)
    {
        var pickedUp = base.PickUp(playerHolder);
        PlayerItemHandler lastPlayerHolder = pickedUp.lastPlayerHolder;

        return pickedUp;
    }

    public override void Drop()
    {
        UIPlayerManager.Instance.HideRecipe();
    }

    public CraftingRecipe GetCurrentRecipe()
    {
        return currentRecipe;
    }


    [PunRPC]
    private void SetRecipe(int index)
    {
        currentRecipe = allRecipes[index];
        string ingredientesTexto = "";
        foreach (var item in currentRecipe.requiredItems)
        {
            ingredientesTexto += $"\n• {item.itemName}";
        }

        recipeIngredients.text = $"Ingredientes: {ingredientesTexto}";
        recipeName.text = currentRecipe.recipeName;
        Icon.sprite = currentRecipe.recipeIcon;
    }
}