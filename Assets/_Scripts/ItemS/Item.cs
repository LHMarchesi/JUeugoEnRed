using Photon.Pun;
using UnityEngine;
public class Item : MonoBehaviourPun, Ipickuppeable
{
    public ItemStats stats;
    public ObjectPooler pooler;
    public bool isHeld;
    public virtual void Drop()
    {
        isHeld = false;
    }

    public virtual Item PickUp()
    {
        isHeld = true;
        return this;
    }

    public void SetPool(ObjectPooler pooler)
    {
        this.pooler = pooler;
    }

    public ObjectPooler GetPool()
    {
        return pooler;
    }
}
