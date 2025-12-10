using UnityEngine;

public class TriggerNewDialogue : MonoBehaviour
{
    [SerializeField] private TextAsset _InkJsonFile;

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.CompareTag("Player"))
        {
            ScriptReader.Instance.LoadStory(_InkJsonFile);
            Destroy(this.gameObject);
        }
    }
}