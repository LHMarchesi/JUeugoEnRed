using UnityEngine;

public class Weapon : Item, Iweapon
{
    public override Item PickUp()
    {
        return this;
    }
    public void Attack()
    {
        Debug.Log("Attacking with " + stats.itemName);
    }
}
