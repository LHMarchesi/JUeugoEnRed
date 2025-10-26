using UnityEngine;
public class ItemBase : MonoBehaviour, Ipickuppeable
{
    public ItemStats stats;
    public bool isHeld = false;
    public PlayerItemHandler lastPlayerHolder;
    //  public ObjectPooler pooler;

    
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

 /* public void SetPool(ObjectPooler pooler)
    {
        this.pooler = pooler;
    }

    public ObjectPooler GetPool()
    {
        return pooler;
    }
 */
}
