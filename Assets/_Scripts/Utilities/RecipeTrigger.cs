using Photon.Pun;
using UnityEngine;

public class RecipeTrigger : MonoBehaviour
{
    public CraftingRecipe craftingRecipeOnTrigger;
    public GameObject cardHolded;

    private void OnTriggerEnter(Collider other)
    {
        KidCard kidCard = other.GetComponent<KidCard>();
        if (kidCard != null)
        {
            cardHolded = kidCard.gameObject;
            craftingRecipeOnTrigger = kidCard.GetCurrentRecipe();
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
            PhotonNetwork.Destroy(cardHolded);
        }
    }
}
