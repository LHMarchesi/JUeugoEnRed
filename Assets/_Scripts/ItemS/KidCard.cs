using Photon.Pun;
using System;
using UnityEngine;

public class KidCard : ItemBase
{
    public CraftingRecipe[] allRecipes;
    private CraftingRecipe currentRecipe;

    public PhotonView view;
    private void Start()
    {
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

        Debug.Log("Last player holder: " + lastPlayerHolder);
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