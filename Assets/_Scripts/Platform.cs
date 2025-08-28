using UnityEngine;

public class Platform : MonoBehaviour
{
    public Transform downHolder;
    public Transform middleHolder;
    public Transform topHolder;

    private void OnTriggerEnter(Collider other)
    {
        Item item = other.GetComponent<Item>();
        if (item != null)
        {
            Transform targetHolder = null;
            switch (item.stats.type)
            {
                case ItemType.down:
                    targetHolder = downHolder;
                    break;
                case ItemType.middle:
                    targetHolder = middleHolder;
                    break;
                case ItemType.top:
                    targetHolder = topHolder;
                    break;
            }

            if (targetHolder != null)
            {
                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;
            
                Vector3 originalScale = item.transform.localScale;

                item.transform.SetParent(targetHolder, true);
                item.transform.localPosition = Vector3.zero; 
                item.transform.localRotation = Quaternion.identity;
                item.transform.localScale = originalScale;
            }
        }

    }
}