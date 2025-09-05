using Photon.Pun;
using UnityEngine;
public enum ItemType
{
    none, down, middle, top
}
public interface Ipickuppeable
{
    Item PickUp();
    void Drop();
}
public class Item : MonoBehaviourPun, Ipickuppeable
{
    public ItemStats stats;
    public ObjectPooler pooler;
    public virtual void Drop()
    {
        transform.SetParent(null);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    public virtual Item PickUp()
    {
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
