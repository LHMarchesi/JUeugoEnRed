using UnityEngine;
using Photon.Pun;

public class GiftboxScript : MonoBehaviour
{
    public RecipeTrigger recipeTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (recipeTrigger.craftingRecipeOnTrigger == null) return;

        Ipickuppeable ipickuppeable = other.GetComponent<Ipickuppeable>();
        if (ipickuppeable != null)
        {
            ItemBase item = other.GetComponent<ItemBase>();
            if (item.stats.itemID == recipeTrigger.GetFinalItemID())  // Se coloca el Item correcto, destrulle la carta y el item, Suma puntos
            {
                UnityEngine.Debug.Log("Item Correcto");
                PhotonNetwork.Destroy(other.gameObject);
                recipeTrigger.DestroyCard();
            }
            else 
            {
                UnityEngine.Debug.Log("Item incorrecto");
            }

        }
    }
}
