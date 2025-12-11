using Photon.Pun;
using System;
using UnityEngine;

public class RecipeTrigger : MonoBehaviour
{
    public CraftingRecipe craftingRecipeOnTrigger;
    public KidCard cardHolded;
    public Action onRecipePlaced;

    private void OnTriggerEnter(Collider other)
    {
        KidCard kidCard = other.GetComponent<KidCard>();
        if (kidCard != null)
        {
            onRecipePlaced?.Invoke();
            cardHolded = kidCard;
            craftingRecipeOnTrigger = kidCard.GetCurrentRecipe();
            kidCard.lastPlayerHolder.DropHeld();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        craftingRecipeOnTrigger = null;
        cardHolded = null;
    }

    public int GetFinalItemID()
    {
        KidCard kidCard = cardHolded.GetComponent<KidCard>();
        GameObject finalItemPrefab = kidCard.GetCurrentRecipe().finalItemPrefab;

        return finalItemPrefab.GetComponent<ItemBase>().stats.itemID;
    }

    public void DestroyCard()
    {
        if (cardHolded != null)
        {
            PhotonNetwork.Destroy(cardHolded.gameObject);
        }
    }
}
