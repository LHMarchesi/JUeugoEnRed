using UnityEngine;
using Photon.Pun;
using System.Collections;
using System;

public class GiftboxScript : MonoBehaviourPun
{
    public RecipeTrigger recipeTrigger;
    public InteractionButton lever;
    private bool isOpen;
    [SerializeField] private GameObject receiverFeedback;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private LevelManager lvlManager;
    private Material defaultMaterial;
    private bool hasProcessedThisOpen;
    public Action OnItemSended;

    private void Start()
    {
        defaultMaterial = receiverFeedback.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (lever.IsOn && !isOpen && !hasProcessedThisOpen)
        {
            hasProcessedThisOpen = true;
            photonView.RPC("OpenBox", RpcTarget.AllBuffered);
        }
    }


    [PunRPC]
    public void OpenBox()
    {
        isOpen = true;
        receiverFeedback.GetComponent<Renderer>().material = blueMaterial;

        if (recipeTrigger.cardHolded == null)
        {
            Debug.Log("Falta Chrismas Card");
            receiverFeedback.GetComponent<Renderer>().material = redMaterial;
            StartCoroutine(CloseBoxAfterDelay());
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
                    OnItemSended?.Invoke();
                    PhotonNetwork.Destroy(item.gameObject);
                    receiverFeedback.GetComponent<Renderer>().material = greenMaterial;
                    recipeTrigger.DestroyCard();
                    lvlManager.AddPoints(lever.LastInteractedPlayer, recipeTrigger.craftingRecipeOnTrigger.points);
                }
                else
                {
                    Debug.Log("Item Incorrecto");
                    receiverFeedback.GetComponent<Renderer>().material = redMaterial;
                }
            }
        }
        lever.SetlastInteractedPlayerNull();
        StartCoroutine(CloseBoxAfterDelay());
    }


    [PunRPC]
    public void CloseBox()
    {
        receiverFeedback.GetComponent<Renderer>().material = defaultMaterial;
        isOpen = false;
        hasProcessedThisOpen = false;
    }

    IEnumerator CloseBoxAfterDelay()
    {
        yield return new WaitForSecondsRealtime(2f);
        photonView.RPC("CloseBox", RpcTarget.AllBuffered);
    }

}
