using Photon.Pun;
using UnityEngine;

public class OrderData: Item
{
    CraftingRecipe recipe;
    GameObject player;
    public override void Drop()
    {
        base.Drop();
    }
    public override Item PickUp()
    {
        return base.PickUp();
    }
}