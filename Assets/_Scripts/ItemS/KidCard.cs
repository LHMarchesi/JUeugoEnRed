using Photon.Pun;
using UnityEngine;

public class KidCard : ItemBase
{
    public CraftingRecipe[] allRecipes;
    CraftingRecipe currentRecipe;

    private void Start()
    {
        if (allRecipes.Length == 0)
        {
            Debug.LogError("No hay recetas cargadas");
            return;
        }

        int index = Random.Range(0, allRecipes.Length);
        currentRecipe = allRecipes[index];
    }

    public override ItemBase PickUp()
    {
        var pickedUp = base.PickUp();

        UIManager.Instance.ShowRecipe(currentRecipe);

        return pickedUp;
    }

    public override void Drop()
    {
        UIManager.Instance.HideRecipe();
    }
}