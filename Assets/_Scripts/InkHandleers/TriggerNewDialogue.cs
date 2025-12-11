using UnityEngine;

public class TriggerNewDialogue : MonoBehaviour
{
    [SerializeField] private TextAsset _InkJsonFile;

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.CompareTag("Player"))
        {
            PlayerContext playerContext = collision.GetComponent<PlayerContext>();
            playerContext.HandleInputs.SetPaused(true);
            ScriptReader.Instance.LoadStory(_InkJsonFile);
            Destroy(this.gameObject);
        }
    }
}