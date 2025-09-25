using UnityEngine;

public class RecipeTrigger : MonoBehaviour
{
    [SerializeField]private Platform platform;
   
    private void OnTriggerEnter(Collider other)
    {
        KidCard kidCard = other.GetComponent<KidCard>();
        platform.SetRecipe(kidCard.GetCurrentRecipe());
    }
    private void OnTriggerExit(Collider other)
    {
        platform.SetRecipe(null);
    }
}
