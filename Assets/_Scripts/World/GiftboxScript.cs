using UnityEngine;
using Photon.Pun;

public class GiftboxScript : MonoBehaviour
{
    public RecipeTrigger recipeTrigger;
    private bool checkBoxOpen = false;
    [SerializeField] private GameObject receiverFeedback;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material blueMaterial;
    private Material defaultMaterial;

    private void Start()
    {
        defaultMaterial = receiverFeedback.GetComponent<Renderer>().material;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entró al Giftbox: " + other.name);
        if (!checkBoxOpen) return;

        Ipickuppeable ipickuppeable = other.GetComponent<Ipickuppeable>();
        if (ipickuppeable != null)
        {
            ItemBase item = other.GetComponent<ItemBase>();
            if (item.stats.itemID == recipeTrigger.GetFinalItemID())  // Se coloca el Item correcto, destrulle la carta y el item, Suma puntos
            {
                UnityEngine.Debug.Log("Item Correcto");
                PhotonNetwork.Destroy(item.gameObject);
                receiverFeedback.GetComponent<Renderer>().material = greenMaterial;
                recipeTrigger.DestroyCard();
            }
            else 
            {
                UnityEngine.Debug.Log("Item incorrecto");
                PhotonNetwork.Destroy(item.gameObject);
                receiverFeedback.GetComponent<Renderer>().material = redMaterial;
            }
            Invoke("CloseBox", 2f);
        }
    }

    public void CloseBox()
    {
        receiverFeedback.GetComponent<Renderer>().material = defaultMaterial;
        checkBoxOpen = false;
    }

    public void OpenBox()
    {
        receiverFeedback.GetComponent<Renderer>().material = blueMaterial;
        checkBoxOpen = true;
    }
}
