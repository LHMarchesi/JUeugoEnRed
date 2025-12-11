using System;
using UnityEngine;
public class ItemBase : MonoBehaviour, Ipickuppeable
{
    public ItemStats stats;
    public bool isHeld = false;
    public PlayerItemHandler lastPlayerHolder;
    public Action OnPickedUp;

    public virtual void Drop()
    {
        isHeld = false;
    }

    public virtual ItemBase PickUp(PlayerItemHandler playerHolder)
    {
        OnPickedUp?.Invoke();
        lastPlayerHolder = playerHolder;
        Debug.Log("(ittembase)Last player holder: " + lastPlayerHolder);
        isHeld = true;
        return this;
    }
}
