using UnityEngine;
using Photon.Pun;

public class GiftboxScript : MonoBehaviour
{
    public RecipeTrigger recipeTrigger;
    public InteractionButton lever;
    private bool isOpen;
    [SerializeField] private GameObject receiverFeedback;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material blueMaterial;
    private Material defaultMaterial;


    private void Start()
    {
        defaultMaterial = receiverFeedback.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (lever.IsOn & !isOpen)
        {
            OpenBox();
        }
    }

    public void OpenBox()
    {
        isOpen = true;
        receiverFeedback.GetComponent<Renderer>().material = blueMaterial;

        if (recipeTrigger.cardHolded == null)
        {
            Debug.Log("Falta Chrismas Card");
            receiverFeedback.GetComponent<Renderer>().material = redMaterial;
            Invoke("CloseBox", 2f);
            return;
        }

        // Detectar ítems dentro del área
        Collider[] items = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);

        foreach (Collider itemCollider in items)
        {
            Ipickuppeable ipickuppeable = itemCollider.GetComponent<Ipickuppeable>();
            if (ipickuppeable != null)
            {
                ItemBase item = itemCollider.GetComponent<ItemBase>();

                if (item.stats.itemID == recipeTrigger.GetFinalItemID())
                {
                    Debug.Log("Item Correcto");
                    PhotonNetwork.Destroy(item.gameObject);
                    receiverFeedback.GetComponent<Renderer>().material = greenMaterial;
                    recipeTrigger.DestroyCard();
                    lever.LastInteractedPlayer.ownSocres += recipeTrigger.craftingRecipeOnTrigger.points;
                }
                else
                {
                    Debug.Log("Item Incorrecto");
                    PhotonNetwork.Destroy(item.gameObject);
                    receiverFeedback.GetComponent<Renderer>().material = redMaterial;
                    
                }
            }
        }
        Debug.Log(lever.LastInteractedPlayer.ownSocres+ " points");
        lever.SetlastInteractedPlayerNull();        
        Invoke("CloseBox", 2f);
    }
    public void CloseBox()
    {
        receiverFeedback.GetComponent<Renderer>().material = defaultMaterial;
        isOpen = false;
    }

}
