using UnityEngine;
public class LevelManager : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    void Start()
    {
        ConnectionManager.Instance.CreatePlayer(spawnPoint);
    }
}
