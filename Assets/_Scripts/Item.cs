using Photon.Pun;
using UnityEngine;
public enum ItemType
{
    down, middle, top
}

public interface Ipickuppeable
{
    Item PickUp();
    void Drop();
}
public class Item : MonoBehaviourPun, Ipickuppeable
{
    public ItemStats stats;
    
    public void Drop()
    {
        transform.SetParent(null);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    public Item PickUp()
    {
        return this;
    }
}
