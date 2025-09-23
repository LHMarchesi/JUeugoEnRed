using Photon.Pun;
using UnityEngine;

public class KidCard : ItemBase
{
    public CraftingRecipe[] allRecipes;
    CraftingRecipe currentRecipe;
    PhotonView playerView;
    GameObject playerRoot;

    private void Start()
    {
        if (allRecipes.Length == 0)
        {
            Debug.LogError("No hay recetas cargadas");
            return;
        }

        int index = Random.Range(0, allRecipes.Length);
        currentRecipe = allRecipes[index];
    }

    public override ItemBase PickUp()
    {
        var pickedUp = base.PickUp();

        // buscamos al jugador que tiene el ItemHolder
        PhotonView playerView = GetComponentInParent<PhotonView>();

        if (playerView != null && playerView.IsMine) // solo el dueño local
        {
            GameObject playerRoot = playerView.gameObject; // acá sí es el PlayerPrefab(Clone)
            UIManager uiManager = playerRoot.GetComponentInChildren<UIManager>(true);

            if (uiManager != null)
            {
                uiManager.ShowRecipe(currentRecipe);
            }
            else
            {
                Debug.LogWarning(" No se encontró UIManager en el Player local.");
            }
        }

        return pickedUp;
    }

    public override void Drop()
    {
        if (playerView != null && playerView.IsMine) // aseguramos que es nuestro jugador local
        {
            UIManager uIManager = playerRoot.GetComponentInChildren<UIManager>();
            if (uIManager != null)
            {
                uIManager.HideRecipe();
            }
        }
        playerRoot = null;
        playerView = null;
    }
}