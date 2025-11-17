using UnityEngine;
public class ItemBase : MonoBehaviour, Ipickuppeable
{
    public ItemStats stats;
    public bool isHeld = false;
    public PlayerItemHandler lastPlayerHolder;
    
    public virtual void Drop()
    {
        isHeld = false;
    }

    public virtual ItemBase PickUp(PlayerItemHandler playerHolder)
    {
        lastPlayerHolder = playerHolder;
        Debug.Log("(ittembase)Last player holder: " + lastPlayerHolder);
        isHeld = true;
        return this;
    }
}
