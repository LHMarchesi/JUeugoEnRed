using System.Collections;
using UnityEngine;
using TMPro;
public class LevelManager : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] float gameTime = 90f;
    [SerializeField] TMP_Text timerText;

    bool gameStarted = false;
    Coroutine gameTimer;
    void Start()
    {
        ConnectionManager.Instance.CreatePlayer(spawnPoint);
        if (!gameStarted)
        {
            gameTimer = StartCoroutine(GameTimer());
            gameStarted = true;
        }
    }


    IEnumerator GameTimer()
    {
        float timeLeft = gameTime;
        timeLeft += 1; // Ajuste para compensar el primer decremento inmediato
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            timeLeft--;
            int minutes = Mathf.FloorToInt(timeLeft / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        }
        Debug.Log("Time's up! Game Over.");
    }
}
