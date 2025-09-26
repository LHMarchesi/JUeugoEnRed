using Photon.Pun;
using System;
using UnityEngine;

public class KidCard : ItemBase
{
    public CraftingRecipe[] allRecipes;
    CraftingRecipe currentRecipe;
    public PhotonView view;
    private void Start()
    {
        if (allRecipes.Length == 0)
        {
            Debug.LogError("No hay recetas cargadas");
            return;
        }
        if (PhotonNetwork.IsMasterClient)
        {
            int index = UnityEngine.Random.Range(0, allRecipes.Length);
            Debug.Log($"[Master] Enviando receta índice: {index}");
            view.RPC("SetRecipe", RpcTarget.AllBuffered, index);
        }

    }

    public override ItemBase PickUp()
    {
        var pickedUp = base.PickUp();

        UIPlayerManager.Instance.ShowRecipe(currentRecipe);

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
    }
}