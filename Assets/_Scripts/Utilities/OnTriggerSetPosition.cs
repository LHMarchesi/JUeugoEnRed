using System.Collections;
using UnityEngine;

public class OnTriggerSetPosition : MonoBehaviour
{
    public Transform objTransform;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = objTransform.position;
        other.transform.rotation = objTransform.rotation;
        StartCoroutine(DesableCollisionsAndWait(0.5f));
    }

    IEnumerator DesableCollisionsAndWait(float time)
    {
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        yield return new WaitForSeconds(time);
        col.enabled = true;
    }

}
